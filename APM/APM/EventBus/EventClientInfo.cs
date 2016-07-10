using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.EventBus
{
    public class EventClientInfo
    {
        public string ClientID { get; private set; }
        public string Address { get; private set; }
        public int Port { get; private set; }
        public IEventBusServiceCallback Callback { get; private set; }

        public EventClientInfo(string clientid, string address, int port, IEventBusServiceCallback callback)
        {
            this.ClientID = clientid;
            this.Address = address;
            this.Port = port;
            this.Callback = callback;
        }

        public override string ToString()
        {
            return string.Format("[{0}] -> {1}, {2}", this.ClientID, this.Address, this.Port);
        }
    }
}
