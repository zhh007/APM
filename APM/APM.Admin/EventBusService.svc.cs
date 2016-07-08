using APM.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;
using System.Runtime.Caching;
using System.Diagnostics;

namespace APM.Admin
{
    public class EventBusService : IEventBusService
    {
        private static List<IEventBusServiceCallback> callbacks;
        private static Dictionary<string, List<IEventBusServiceCallback>> subscriber = new Dictionary<string, List<IEventBusServiceCallback>>();

        public EventBusService()
        {
            callbacks = new List<IEventBusServiceCallback>();
        }

        public void OnLine()
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            callbacks.Add(callback);
        }

        public void OffLine()
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            callbacks.Remove(callback);
        }

        public void Subscribe(string name)
        {
            Debug.WriteLine(string.Format("EventBusService.Subscribe : {0}", name));
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            if(!subscriber.ContainsKey(name))
            {
                List<IEventBusServiceCallback> cblist = new List<IEventBusServiceCallback>();
                cblist.Add(callback);
                subscriber.Add(name, cblist);
            }
            else
            {
                List<IEventBusServiceCallback> cblist = subscriber[name];
                if(cblist != null && !cblist.Contains(callback))
                {
                    cblist.Add(callback);
                }
            }
        }

        public void Publish(string name, string data)
        {
            Debug.WriteLine("EventBusService.Publish : {0} , {1}", name, data);
            if (subscriber.ContainsKey(name))
            {
                List<IEventBusServiceCallback> cblist = subscriber[name];
                if (cblist != null)
                {
                    foreach (var item in cblist)
                    {
                        try
                        {
                            item.ReceiveNotice(name, data);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
    }
}
