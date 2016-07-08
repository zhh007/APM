using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.EventBus
{
    public class AppEvent
    {
        private readonly static Lazy<EventBusAgent> _instance = new Lazy<EventBusAgent>(() => new EventBusAgent());
        private static Dictionary<string, List<Action<object>>> subscriber = new Dictionary<string, List<Action<object>>>();


        private static EventBusAgent Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        static AppEvent()
        {
            Instance.OnProcessNotice += Instance_OnProcessNotice;
        }

        private static void Instance_OnProcessNotice(string name, string data)
        {
            Debug.WriteLine("Instance_OnProcessNotice -> {0} , {1}", name, data);
            List<Action<object>> handlers = null;
            if (!subscriber.ContainsKey(name))
            {
                handlers = new List<Action<object>>();
            }
            else
            {
                handlers = subscriber[name];
            }
            foreach (var handler in handlers)
            {
                handler(data);
            }
        }

        public static void Subscribe(string name, Action<object> handler)
        {
            List<Action<object>> handlers = null;
            if(!subscriber.ContainsKey(name))
            {
                handlers = new List<Action<object>>();
                subscriber.Add(name, handlers);
            }
            else
            {
                handlers = subscriber[name];
            }
            handlers.Add(handler);

            Instance.Subscribe(name);
        }

        //public static void UnSubscribe<T>(string name, Action<T> handler)
        //    where T : new()
        //{
        //    Instance.UnSubscribe<T>(name, handler);
        //}

        public static void Publish(string name, object parameter)
        {
            //var d = Activator.CreateInstance<T>();
            Instance.Publish(name, parameter.ToString());
        }
    }
}
