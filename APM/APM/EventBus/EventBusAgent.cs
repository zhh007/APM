using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace APM.EventBus
{
    public class EventBusAgent: IEventBusServiceCallback
    {
        private EventBusClient client = null;

        public EventBusAgent()
        {
            client = new EventBusClient(new System.ServiceModel.InstanceContext(this));
        }

        public void OffLine()
        {
            client.OffLine();
        }

        public void OnLine()
        {
            client.OnLine();
        }

        public void Subscribe(string name)
        {
            client.Subscribe(name);
        }

        public void Publish(string name, string data)
        {
            client.Publish(name, data);
        }

        public void ReceiveNotice(string name, string data)
        {
            Debug.WriteLine(" ReceiveNotice -> {0} , {1}", name, data);
            if(this.OnProcessNotice != null)
            {
                OnProcessNotice(name, data);
            }
        }

        public delegate void ProcessNotice(string name, string data);

        public event ProcessNotice OnProcessNotice;

    }
}
