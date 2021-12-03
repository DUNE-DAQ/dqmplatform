"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chartHub").build();

if (typeof (document.getElementById('saveButton')) != 'undefined' && document.getElementById('saveButton') != null) {    // Exists.
    document.getElementById("saveButton").addEventListener("click", function () {

        var pannelName = document.getElementById("pannelName").value;

        if (pannelName != "") {
            connection.invoke("saveView", pannelName).catch(function (err) {
                return console.error(err.toString());
            });
            //event.preventDefault();

            document.getElementById('pannelName').value = "";
        }
    });
}

if (typeof (document.getElementById('sendButton')) != 'undefined' && document.getElementById('sendButton') != null) {    // Exists.

    document.getElementById("sendButton").addEventListener("click", function () {
        var dataDisplayId = document.getElementById("dataDisplayId").value;

        var element = document.createElement("div");
        element.setAttribute("id", "chartPlaceholder" + dataDisplayId);
        element.setAttribute("class", "column");
        document.getElementById('plotsContainer').appendChild(element);

        connection.invoke("GetDisplayList", dataDisplayId).catch(function (err) {
            return console.error(err.toString());
        });
        //event.preventDefault();
    });
}

function StartDisplayAtTime() {
    const urlParams = new URLSearchParams(window.location.search);
    const startTime = urlParams.get('startTime');
    connection.invoke("UpdateDisplayFromTime", dataDisplayIdList, startTime).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.start().then(function () {
    for (var i = 0; i < dataDisplayIdList.length; i++) {
        SendDataDisplayIdDisplayed(dataDisplayIdList[i]);
    }
}).catch(function (err) {
    return console.error(err.toString());
});

//Send to the hub a currently display Id dataDisplayId
function SendDataDisplayIdDisplayed(dataDisplayId) {
    //Check if the page asked to block updates
    if (typeof blockUpdates == "undefined") {
        connection.invoke("GetDisplayList", dataDisplayId).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        const urlParams = new URLSearchParams(window.location.search);
        const startTime = urlParams.get('startTime');
        connection.invoke("GetDisplayAtTime", dataDisplayId, startTime).catch(function (err) {
            return console.error(err.toString());
        });
    }
    //event.preventDefault();
}

connection.on("ReceiveTimeUpdate", function (time, formatedTime) {
    if (typeof (document.getElementById('timeDisplay')) != 'undefined' && document.getElementById('timeDisplay') != null) {
        document.getElementById("timeDisplay").innerHTML = time;
    }
    if (typeof (document.getElementById('startTime')) != 'undefined' && document.getElementById('startTime') != null) {
        document.getElementById('startTime').value = formatedTime;
    }
});

var dataList = [];

connection.on("ReceivePlotUpdate", function (plotUpdate) {

    var t0 = performance.now()    
    //Artificially fill plotUpdate.chartLabels array too simplify display of titles
    while (plotUpdate.chartLabels.length < 3) {
        plotUpdate.chartLabels.push("")
    }

    //Display the panels informations
    if (typeof (document.getElementById('runNumber')) != 'undefined' && document.getElementById('runNumber') != null && typeof (document.getElementById('triggerRecord')) != 'undefined' && document.getElementById('triggerRecord') != null) {
        if (typeof (plotUpdate.runNumber) != 'undefined' && plotUpdate.runNumber != null) {
            document.getElementById('runNumber').innerHTML = " Run number : " + plotUpdate.runNumber;
        }
        if (typeof (plotUpdate.metadata) != 'undefined' && plotUpdate.metadata != null) {
            document.getElementById('triggerRecord').innerHTML = " Trigger record : " + plotUpdate.metadata;
        }
    }

    //Checks if the array exists
    var data = [];
    var getData = dataList.filter(obj => {
        return obj.id === plotUpdate.chartName;
    })

    if (getData.length != 0) {
        data = getData[0].data;
    }

    for (var j = 0; j < plotUpdate.chartData.length; j++) {
        var index = -1;
        
        if (getData.length != 0) {
            index = getData[0].data.findIndex(data => data.traceId === plotUpdate.dataId + "_" + j);
        }

        if (index == -1)
            data.push({
                x: [],
                y: [],
                traceId: plotUpdate.dataId + "_" + j,
            });
        //dataList.push({ id: plotUpdate.chartName, data: data });
    }
    

    //If doesn't exist initiate an empty data 
    if (getData.length == 0) {
        dataList.push({ id: plotUpdate.chartName, data: data });
    }
    else { //data allready displayed, get data 
        const dataListIndex = dataList.findIndex(dataList => dataList.id == plotUpdate.chartName);
        dataList[dataListIndex].data == data;
    }
    var layout = {};
    //Update the data in y axis

    /*type: 'log',
    autorange: true
    tickvals: [...Array(plotUpdate.chartData[0].length).keys()],
    ticktext: HideLabels(plotUpdate.xLabels, 20),
    */

    switch (plotUpdate.chartType) {
        case "markers":
        case "lines":
        case "lines+markers":
            data = UpdateY(plotUpdate, data, plotUpdate.seriesLabels);
            layout = {
                title: plotUpdate.dataDisplayName + " updated: " + plotUpdate.chartTime + " UTC",
                xaxis: {
                    title: {
                        text: plotUpdate.chartLabels[0],
                    },
                    tickvals: [...Array(plotUpdate.chartData[0].length).keys()],
                    ticktext: plotUpdate.xLabels,
                    showticklabels: false,
                    showgrid: false

                },
                yaxis: {
                    title: {
                        text: plotUpdate.chartLabels[1],
                    },
                    type: plotUpdate.chartPlottingType,
                    showgrid: true,
                },
            };
            break;
        case "histogram":
            data = UpdateX(plotUpdate, data, plotUpdate.seriesLabels);
            data = UpdateY(plotUpdate, data, plotUpdate.seriesLabels);
            layout = {
                title: plotUpdate.dataDisplayName + " updated: " + plotUpdate.chartTime + " UTC",

                xaxis: {
                    title: {
                        text: plotUpdate.chartLabels[0],
                    },
                    tickvals: [...Array(plotUpdate.chartData[0].length).keys()],
                    ticktext: HideLabels(plotUpdate.xLabels, 20),
                },
                yaxis: {
                    title: {
                        text: plotUpdate.chartLabels[1],
                    },
                },
            };
            break;
        case "heatmap":
            data = UpdateZ(plotUpdate, data);
            layout = {
                title: plotUpdate.dataDisplayName + " updated: " + plotUpdate.chartTime + " UTC",

                xaxis: {
                    title: {
                        text: plotUpdate.chartLabels[0],
                    },
                    tickvals: [...Array(plotUpdate.chartData[0].length).keys()],
                    ticktext: plotUpdate.xLabels,
                    showticklabels: false,
                },

                yaxis: {
                    title: {
                        text: plotUpdate.chartLabels[1],
                    },
                    tickvals: [...Array(plotUpdate.chartData.length).keys()],
                    ticktext: plotUpdate.yLabels,
                    showticklabels: false,
                },
            };
            break;
        default:
            alert("Unknow data type");

    }

    //Draw the plot
    //react faster but does not manage concurrent request, if request too close in time, jsut ignores them....

    Plotly.newPlot(plotUpdate.chartName, data, layout);

    /*
    if (plotUpdate.isInit) {
        Plotly.newPlot(plotUpdate.chartName, data, layout);
    }
    else
    {
        Plotly.react(plotUpdate.chartName, data, layout);
    }
    */

    var t1 = performance.now()
    console.log("Call to plot took " + (t1 - t0) + " milliseconds.")
});

function HideLabels(labelsArray, hidingFactor) {

    //hides some labels to avoid overlapping
    if (labelsArray.length > 20) {
        for (var j = 0; j < labelsArray.length; j++) {
            if (j % 20 != 0) {
                labelsArray[j] = "";
            }
        }
    }
    return labelsArray;
}

function UpdateZ(plotUpdate, data) {

    var data2dim = [data.length];
    for (var j = 0; j < plotUpdate.chartData.length; j++) {
        data2dim[j] = UpdateAxis(plotUpdate, data[j].y, j)
    }

    var trace = {
        z: data2dim,
        type: "heatmap",
        colorbar: { title: plotUpdate.chartLabels[2] }
    };
    data = [trace];

    return data;
}

function UpdateY(plotUpdate, data, labels) {
    for (var j = 0; j < plotUpdate.chartData.length; j++) {

        const index = data.findIndex(data => data.traceId === plotUpdate.dataId + "_" + j);

        if (labels.length > index) {
            var trace = {
                y: UpdateAxis(plotUpdate, data[index].y, j),
                mode: plotUpdate.chartType,
                type: 'scatter',
                traceId: plotUpdate.dataId + "_" + j,
                name: labels[index],
            };
        }
        else {
            var trace = {
                y: UpdateAxis(plotUpdate, data[index].y, j),
                mode: plotUpdate.chartType,
                type: 'scatter',
                traceId: plotUpdate.dataId + "_" + j,
            };
        }
        
        //Add data at the right index, the index is the data and the position in a data
        data[index] = trace;
    }

    return data;
}

function UpdateX(plotUpdate, data, labels) {
    for (var j = 0; j < plotUpdate.chartData.length; j++) {

        const index = data.findIndex(data => data.traceId === plotUpdate.dataId + "_" + j);
        
        if (labels.length > index) {
            var trace = {
                x: UpdateAxis(plotUpdate, data[index].x, j),
                type: plotUpdate.chartType,
                traceId: plotUpdate.dataId + "_" + j,
                name: labels[index],
            };
        }
        else {
            var trace = {
                x: UpdateAxis(plotUpdate, data[index].x, j),
                type: plotUpdate.chartType,
                traceId: plotUpdate.dataId + "_" + j,
            };
        }
        //Add data at the right index, the index is the data and the position in a data
        data[index] = trace;    }

    return data;
}

function UpdateAxis(plotUpdate, axisCurrent, index) {
    //If this is not the graph initiation add the data at the end
    if (!plotUpdate.isInit) {
        //pushes new data and remove old ones if the plot's length is higher than requested
        for (var k = 0; k < plotUpdate.chartData[index].length; k++) {
            axisCurrent.push(plotUpdate.chartData[index][k]);
        }
    }
    else { //If this is the graph initiation, add data at the beginning
        var tempDataArray = []
        for (var k = 0; k < plotUpdate.chartData[index].length; k++) {
            tempDataArray.push(plotUpdate.chartData[index][k]);
        }
        axisCurrent = tempDataArray.concat(axisCurrent);
    }

    //Remove the excess from the plot
    if (axisCurrent.length > plotUpdate.chartLength && plotUpdate.chartLength != 0) {
        axisCurrent = axisCurrent.slice(axisCurrent.length - plotUpdate.chartLength);
    }
    //If plot size set to 0 
    else if (axisCurrent.length > plotUpdate.chartData[0].length)
    {
        axisCurrent = axisCurrent.slice(axisCurrent.length - plotUpdate.chartData[0].length);
    }

    return axisCurrent;
}