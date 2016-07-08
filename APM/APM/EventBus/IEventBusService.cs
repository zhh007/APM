using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace APM.EventBus
{
    [ServiceContract(CallbackContract = typeof(IEventBusServiceCallback))]
    public interface IEventBusService
    {
        [OperationContract(IsOneWay = true)]
        void OnLine();

        [OperationContract(IsOneWay = true)]
        void OffLine();

        [OperationContract(IsOneWay = true)]
        void Subscribe(string name);

        [OperationContract(IsOneWay = true)]
        void Publish(string name, string data);
    }

    public interface IEventBusServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveNotice(string name, string data);
    }
}
