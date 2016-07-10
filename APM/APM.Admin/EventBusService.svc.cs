using APM.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Linq;

namespace APM.Admin
{
    public class EventBusService : IEventBusService
    {
        private static readonly object _lock = new object();
        private static List<EventClientInfo> callbacks = new List<EventClientInfo>();
        private static Dictionary<string, List<IEventBusServiceCallback>> subscriber = new Dictionary<string, List<IEventBusServiceCallback>>();

        public EventBusService()
        {
            Debug.WriteLine("EventBusService created: " + this.GetHashCode().ToString());
        }

        ~EventBusService()
        {
            Debug.WriteLine("EventBusService destroyed: " + this.GetHashCode().ToString());
        }

        public void OnLine(string clientid)
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();

            MessageProperties prop = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            Debug.WriteLine(string.Format("client regist:{0}, {1}", endpoint.Address, endpoint.Port));

            EventClientInfo client = new EventClientInfo(clientid, endpoint.Address, endpoint.Port, callback);

            bool isadd = false;
            lock (_lock)
            {
                if (callbacks.Count(p => p.ClientID == clientid) == 0)
                {
                    callbacks.Add(client);
                    isadd = true;
                }
            }

            if(isadd)
            {
                OperationContext.Current.Channel.Faulted += Channel_Faulted;
                OperationContext.Current.Channel.Closed += Channel_Closed;
                Debug.WriteLine(string.Format("{0}, 上线了。", client), "EventBusService");
            }
            else
            {
                Debug.WriteLine(string.Format("{0}, 不要重复上线。", client), "EventBusService");
            }
        }

        private void Channel_Closed(object sender, EventArgs e)
        {
            IEventBusServiceCallback callback = sender as IEventBusServiceCallback;
            lock (_lock)
            {
                for (int i = callbacks.Count - 1; i >= 0; i--)
                {
                    var client = callbacks[i];
                    if (client.Callback == callback)
                    {
                        Debug.WriteLine(string.Format("{0}, 下线了。", client), "EventBusService");
                        callbacks.RemoveAt(i);
                    }
                }
            }
        }

        private void Channel_Faulted(object sender, EventArgs e)
        {
            IEventBusServiceCallback callback = sender as IEventBusServiceCallback;
            var client = callbacks.Find(c => c.Callback == callback);
            Debug.WriteLine(string.Format("{0}, 通道异常。", client), "EventBusService");
        }

        public void OffLine(string clientid)
        {
            lock (_lock)
            {
                for (int i = callbacks.Count - 1; i >= 0; i--)
                {
                    if (callbacks[i].ClientID == clientid)
                    {
                        var client = callbacks[i];
                        Debug.WriteLine(string.Format("{0}, 下线了。", client), "EventBusService");
                        callbacks.RemoveAt(i);
                    }
                }
            }
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

        public void CheckCallbackChannels()
        {
            EventClientInfo[] clientList = new EventClientInfo[0];
            lock (_lock)
            {
                clientList = new EventClientInfo[callbacks.Count];
                callbacks.CopyTo(clientList);

                foreach (EventClientInfo registeredClient in clientList)
                {
                    ICommunicationObject callbackChannel = registeredClient.Callback as ICommunicationObject;

                    if (callbackChannel.State == CommunicationState.Closed || callbackChannel.State == CommunicationState.Faulted)
                    {
                        this.OffLine(registeredClient.ClientID);
                    }
                }
            }
        }
    }
}
