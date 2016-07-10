using APM.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace APM.Web.Events
{
    public class TestEvent : IDistributedEvent
    {
        public string Detail
        {
            get
            {
                return "";
            }
        }

        public string Name
        {
            get
            {
                return "";
            }
        }

        public void Process(string parameter)
        {
            Debug.WriteLine("APM.Web TestEvent -> " + parameter);
        }
    }
}