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
        private static List<EventClientInfo> clients = new List<EventClientInfo>();
        private static Dictionary<string, List<IEventBusServiceCallback>> subscribers = new Dictionary<string, List<IEventBusServiceCallback>>();

        public EventBusService()
        {
            Debug.WriteLine(string.Format("Service {0} created.", this.GetHashCode()), "EventBusService");
        }

        ~EventBusService()
        {
            Debug.WriteLine(string.Format("Service {0} destroyed.", this.GetHashCode()), "EventBusService");
        }

        public void OnLine(string clientid)
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            MessageProperties prop = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            EventClientInfo client = new EventClientInfo(clientid, endpoint.Address, endpoint.Port, callback);

            bool isadd = false;
            lock (_lock)
            {
                if (clients.Count(p => p.ClientID == clientid) == 0)
                {
                    clients.Add(client);
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
                for (int i = clients.Count - 1; i >= 0; i--)
                {
                    var client = clients[i];
                    if (client.Callback == callback)
                    {
                        Debug.WriteLine(string.Format("{0}, 下线了。", client), "EventBusService");
                        clients.RemoveAt(i);
                    }
                }
            }
        }

        private void Channel_Faulted(object sender, EventArgs e)
        {
            IEventBusServiceCallback callback = sender as IEventBusServiceCallback;
            var client = clients.Find(c => c.Callback == callback);
            Debug.WriteLine(string.Format("{0}, 通道异常。", client), "EventBusService");
        }

        public void OffLine(string clientid)
        {
            lock (_lock)
            {
                for (int i = clients.Count - 1; i >= 0; i--)
                {
                    if (clients[i].ClientID == clientid)
                    {
                        var client = clients[i];
                        Debug.WriteLine(string.Format("{0}, 下线了。", client), "EventBusService");
                        clients.RemoveAt(i);
                    }
                }
            }
        }

        public void Subscribe(string eventName)
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            var clientinfo = GetClientInfo(callback);
            Debug.WriteLine(string.Format("{0} Subscribe {1}.", clientinfo, eventName), "EventBusService");

            if (!subscribers.ContainsKey(eventName))
            {
                List<IEventBusServiceCallback> cblist = new List<IEventBusServiceCallback>();
                cblist.Add(callback);
                subscribers.Add(eventName, cblist);
            }
            else
            {
                List<IEventBusServiceCallback> cblist = subscribers[eventName];
                if (cblist != null && !cblist.Contains(callback))
                {
                    cblist.Add(callback);
                }
            }
        }

        public void Publish(string eventName, string parameter)
        {
            IEventBusServiceCallback callback = OperationContext.Current.GetCallbackChannel<IEventBusServiceCallback>();
            var clientinfo = GetClientInfo(callback);
            Debug.WriteLine(string.Format("{0} Publish {1}, {2}.", clientinfo, eventName, parameter), "EventBusService");

            if (subscribers.ContainsKey(eventName))
            {
                List<IEventBusServiceCallback> cblist = subscribers[eventName];
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
                clientList = new EventClientInfo[clients.Count];
                clients.CopyTo(clientList);

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

        private EventClientInfo GetClientInfo(IEventBusServiceCallback callback)
        {
            return clients.Find(p => p.Callback == callback);
        }
    }
}
