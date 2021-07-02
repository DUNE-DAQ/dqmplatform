using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Actions
{

    //Singleton pseudo pattern
    public sealed class InputsMessenger : IInputMessage
    {
        //Implement lazy singleton mechanism
        private static readonly InputsMessenger instance = new InputsMessenger();
        // Explicit static constructor to tell compiler not to mark type as beforefieldinit  
        static InputsMessenger() { }
        private InputsMessenger() { }
        public static InputsMessenger Instance
        {
            get
            {
                return instance;
            }
        }

        public event EventHandler OnIncoming;

        object objectLock = new Object();
        event EventHandler IInputMessage.OnIncoming
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

        public void InputMessage(string message)
        {
            // Raise IShape's event after the object is drawn.
            OnIncoming?.Invoke(message, EventArgs.Empty);
        }
    }

    public interface IInputMessage
    {
        // Raise this event on incoming message
        event EventHandler OnIncoming;
    }
}
