using DuneDaqMonitoringPlatform.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Services
{
    public class PerformenceTimer
    {
        private static PerformanceTimeObject _TimerVariable = new PerformanceTimeObject {executionRound = 0, executionFinished = true };

        public static PerformanceTimeObject TimerVariable
        {
            get { return _TimerVariable; }
            set { _TimerVariable = value; }
        }

    }

    public class PerformanceTimeObject
    {
        public long executionTime { get; set; }
        public int executionRound { get; set; }
        public bool executionFinished { get; set; }

    }
}
