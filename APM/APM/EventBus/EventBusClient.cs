using System.Threading.Tasks;

namespace APM.EventBus
{
    internal class EventBusClient : System.ServiceModel.DuplexClientBase<IEventBusService>, IEventBusService
    {
        public EventBusClient(System.ServiceModel.InstanceContext callbackInstance) :
                base(callbackInstance)
        {
        }

        public EventBusClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) :
                base(callbackInstance, endpointConfigurationName)
        {
        }

        public EventBusClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) :
                base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public EventBusClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public EventBusClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(callbackInstance, binding, remoteAddress)
        {
        }

        public void OffLine()
        {
            base.Channel.OffLine();
        }

        public void OnLine()
        {
            base.Channel.OnLine();
        }

        public void Subscribe(string eventName)
        {
            base.Channel.Subscribe(eventName);
        }

        public void Publish(string eventName, string parameter)
        {
            base.Channel.Publish(eventName, parameter);
        }

        public async Task OnLineAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.OnLine();
            });
        }

        public async Task OffLineAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.OffLine();
            });
        }

        public async void SubscribeAsync(string eventName)
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.Subscribe(eventName);
            });
        }

        public async void PublishAsync(string eventName, string parameter)
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.Publish(eventName, parameter);
            });
        }
    }
}
