using APM.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace APM.Admin
{
    public class EventBusService : IEventBusService
    {
        private static List<IEventBusServiceCallback> callbacks = new List<IEventBusServiceCallback>();
        private static Dictionary<string, List<IEventBusServiceCallback>> subscriber = new Dictionary<string, List<IEventBusServiceCallback>>();

        public EventBusService()
        {
            Debug.WriteLine("EventBusService created: " + this.GetHashCode().ToString());
        }

        ~EventBusService()
        {
            Debug.WriteLine("EventBusService destroyed: " + this.GetHashCode().ToString());
        }

        public void OnLine()
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            callbacks.Add(callback);

            MessageProperties prop = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            Debug.WriteLine(string.Format("client regist:{0}, {1}", endpoint.Address, endpoint.Port));
        }

        public void OffLine()
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            callbacks.Remove(callback);
        }

        public void Subscribe(string eventName)
        {
            Debug.WriteLine(string.Format("EventBusService.Subscribe : {0}", eventName));
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            if (!subscriber.ContainsKey(eventName))
            {
                List<IEventBusServiceCallback> cblist = new List<IEventBusServiceCallback>();
                cblist.Add(callback);
                subscriber.Add(eventName, cblist);
            }
            else
            {
                List<IEventBusServiceCallback> cblist = subscriber[eventName];
                if (cblist != null && !cblist.Contains(callback))
                {
                    cblist.Add(callback);
                }
            }
        }

        public void Publish(string eventName, string parameter)
        {
            Debug.WriteLine("EventBusService.Publish : {0} , {1}", eventName, parameter);
            if (subscriber.ContainsKey(eventName))
            {
                List<IEventBusServiceCallback> cblist = subscriber[eventName];
                if (cblist != null)
                {
                    foreach (var item in cblist)
                    {
                        try
                        {
                            item.ReceiveEvent(eventName, parameter);
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
