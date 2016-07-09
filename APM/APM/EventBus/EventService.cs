using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace APM.EventBus
{
    public class EventService
    {
        private readonly static Lazy<EventBusAgent> _instance = new Lazy<EventBusAgent>(() => new EventBusAgent());
        private static Dictionary<string, List<Action<object>>> sublist = new Dictionary<string, List<Action<object>>>();

        private static EventBusAgent Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        static EventService()
        {
            Instance.OnProcessEvent += EventService_OnProcessEvent;
            Instance.OnLine();
        }

        private static void EventService_OnProcessEvent(string name, string data)
        {
            Debug.WriteLine("Instance_OnProcessNotice -> {0} , {1}", name, data);
            List<Action<object>> handlers = null;
            if (!sublist.ContainsKey(name))
            {
                handlers = new List<Action<object>>();
            }
            else
            {
                handlers = sublist[name];
            }
            foreach (var handler in handlers)
            {
                handler(data);
            }
        }

        public static void Subscribe(string eventName, Action<object> handler)
        {
            List<Action<object>> handlers = null;
            if(!sublist.ContainsKey(eventName))
            {
                handlers = new List<Action<object>>();
                sublist.Add(eventName, handlers);
            }
            else
            {
                handlers = sublist[eventName];
            }
            handlers.Add(handler);

            Instance.Subscribe(eventName);
        }

        //public static void UnSubscribe<T>(string name, Action<T> handler)
        //    where T : new()
        //{
        //    Instance.UnSubscribe<T>(name, handler);
        //}

        public static void Publish(string eventName, object parameter)
        {
            //var d = Activator.CreateInstance<T>();
            Instance.Publish(eventName, parameter.ToString());
        }

        public static void AddEvent<T>()
            where T : IDistributedEvent, new()
        {
            var evt = Activator.CreateInstance<T>();
            Subscribe(typeof(T).Name, p => {
                evt.Process(p.ToString());
            });
        }
    }
}
