using DuneDaqMonitoringPlatform.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Actions
{

    //Singleton pseudo pattern
    public sealed class ChartDataMessenger : IChartDataMessage
    {
        //Implement lazy singleton mechanism
        private static readonly ChartDataMessenger instance = new ChartDataMessenger();
        // Explicit static constructor to tell compiler not to mark type as beforefieldinit  
        static ChartDataMessenger() { }
        private ChartDataMessenger() { }
        public static ChartDataMessenger Instance
        {
            get
            {
                return instance;
            }
        }

        public event EventHandler OnIncoming;

        object objectLock = new Object();
        event EventHandler IChartDataMessage.OnIncoming
        {
            add
            {
                lock (objectLock)
                {
                    OnIncoming += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    OnIncoming -= value;
                }
            }
        }

        public void ChartDataMessage(ChartData chartData)
        {
            // Raise IShape's event after the object is drawn.
            OnIncoming?.Invoke(chartData, EventArgs.Empty);
        }
    }

    public interface IChartDataMessage
    {
        // Raise this event on incoming message
        event EventHandler OnIncoming;
    }
}
