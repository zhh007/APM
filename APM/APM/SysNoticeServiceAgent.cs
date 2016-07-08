using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace APM.Notice
{
    public class SysNoticeServiceAgent : System.ServiceModel.DuplexClientBase<ISysNoticeService>, ISysNoticeService
    {
        public SysNoticeServiceAgent(System.ServiceModel.InstanceContext callbackInstance) :
                base(callbackInstance)
        {
        }

        public SysNoticeServiceAgent(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) :
                base(callbackInstance, endpointConfigurationName)
        {
        }

        public SysNoticeServiceAgent(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) :
                base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public SysNoticeServiceAgent(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public SysNoticeServiceAgent(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
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

        public async Task OnLineAsync()
        {
            //Debug.WriteLine("task 0");
            await Task.Factory.StartNew(() =>
            {
                base.Channel.OnLine();
            });
            //Debug.WriteLine("task 00");
        }

        public async Task OffLineAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                base.Channel.OffLine();
            });
        }

        public void Subscribe(string name)
        {
            base.Channel.Subscribe(name);
        }

        public void Publish(string name, string data)
        {
            base.Channel.Publish(name, data);
        }
    }
}
