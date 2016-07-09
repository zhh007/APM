namespace APM.EventBus
{
    /// <summary>
    /// 分布式事件
    /// </summary>
    public interface IDistributedEvent
    {
        /// <summary>
        /// 事件名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 事件描述
        /// </summary>
        string Detail { get; }

        /// <summary>
        /// 事件处理函数
        /// </summary>
        /// <param name="parameter">事件参数</param>
        void Process(string parameter);
    }
}
