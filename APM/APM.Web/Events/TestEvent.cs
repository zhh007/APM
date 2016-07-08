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
        public void Process(string parameter)
        {
            Debug.WriteLine("TestEvent -> " + parameter);
        }
    }
}