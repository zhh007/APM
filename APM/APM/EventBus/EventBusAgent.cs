using System.Diagnostics;

namespace APM.EventBus
{
    internal class EventBusAgent : IEventBusServiceCallback
    {
        private EventBusClient client = null;
        public delegate void ProcessEvent(string eventName, string parameter);
        public event ProcessEvent OnProcessEvent;

        public EventBusAgent()
        {
            client = new EventBusClient(new System.ServiceModel.InstanceContext(this));
        }

        public void OffLine(string clientid)
        {
            client.OffLine(clientid);
        }

        public void OnLine(string clientid)
        {
            client.OnLine(clientid);
        }

        public void Subscribe(string eventName)
        {
            client.Subscribe(eventName);
        }

        public void Publish(string eventName, string parameter)
        {
            client.Publish(eventName, parameter);
        }

        public void ReceiveEvent(string eventName, string parameter)
        {
            Debug.WriteLine(" ReceiveEvent -> {0} , {1}", eventName, parameter);
            if (this.OnProcessEvent != null)
            {
                OnProcessEvent(eventName, parameter);
            }
        }
    }
}
