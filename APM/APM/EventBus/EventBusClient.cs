using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace APM.EventBus
{
    public class EventBusClient : System.ServiceModel.DuplexClientBase<IEventBusService>, IEventBusService
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

        public void Subscribe(string name)
        {
            base.Channel.Subscribe(name);
        }

        public void Publish(string name, string data)
        {
            base.Channel.Publish(name, data);
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

        public async void SubscribeAsync(string name)
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.Subscribe(name);
            });
        }

        public async void PublishAsync(string name, string data)
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.Publish(name, data);
            });
        }
    }
}
