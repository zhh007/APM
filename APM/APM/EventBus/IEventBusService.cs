using System.ServiceModel;

namespace APM.EventBus
{
    [ServiceContract(CallbackContract = typeof(IEventBusServiceCallback))]
    public interface IEventBusService
    {
        /// <summary>
        /// 客户端上线
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void OnLine(string clientid);

        /// <summary>
        /// 客户端下线
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void OffLine(string clientid);

        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <param name="eventName">事件名</param>
        [OperationContract(IsOneWay = true)]
        void Subscribe(string eventName);

        /// <summary>
        /// 事件发布
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="parameter">事件参数</param>
        [OperationContract(IsOneWay = true)]
        void Publish(string eventName, string parameter);
    }

    public interface IEventBusServiceCallback
    {
        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="parameter">事件参数</param>
        [OperationContract(IsOneWay = true)]
        void ReceiveEvent(string eventName, string parameter);
    }
}
