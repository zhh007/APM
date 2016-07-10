using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace APM.EventBus
{
    public class EventService
    {
        private readonly static Lazy<EventBusAgent> _instance = new Lazy<EventBusAgent>(() => new EventBusAgent());
        private static Dictionary<string, List<Action<object>>> sublist = new Dictionary<string, List<Action<object>>>();
        private static string ClientID = string.Empty;
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

            string str = AppDomain.CurrentDomain.BaseDirectory;
            ClientID = MD5(str);
            Instance.OnLine(ClientID);
        }

        public static string MD5(string stringToHash)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash.ToLower());
            byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashedEmailBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        private static void EventService_OnProcessEvent(string name, string data)
        {
            Debug.WriteLine(string.Format("Process {0}, {1}.", name, data), "EventService");
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
            try
            {
                Instance.Publish(eventName, parameter.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("发生异常,{0}", ex.Message));
            }
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
