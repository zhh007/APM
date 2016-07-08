using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.EventBus
{
    public interface IDistributedEvent
    {
        void Process(string parameter);
    }
}
