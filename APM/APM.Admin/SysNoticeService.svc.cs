using APM.Notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;
using System.Runtime.Caching;
using System.Diagnostics;

namespace APM.Admin
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SysNoticeService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SysNoticeService.svc 或 SysNoticeService.svc.cs，然后开始调试。
    public class SysNoticeService : ISysNoticeService
    {
        private static List<ISysNoticeServiceCallback> callbacks;
        private static Dictionary<string, List<ISysNoticeServiceCallback>> subscriber = new Dictionary<string, List<ISysNoticeServiceCallback>>();

        public SysNoticeService()
        {
            callbacks = new List<ISysNoticeServiceCallback>();
        }

        public void OnLine()
        {
            ISysNoticeServiceCallback callback = OperationContext.Current.GetCallbackChannel<ISysNoticeServiceCallback>();
            callbacks.Add(callback);
        }

        public void OffLine()
        {
            ISysNoticeServiceCallback callback = OperationContext.Current.GetCallbackChannel<ISysNoticeServiceCallback>();
            callbacks.Remove(callback);
        }

        public void Subscribe(string name)
        {
            Debug.WriteLine(string.Format("SysNoticeService.Subscribe : {0}", name));
            ISysNoticeServiceCallback callback = OperationContext.Current.GetCallbackChannel<ISysNoticeServiceCallback>();
            if(!subscriber.ContainsKey(name))
            {
                List<ISysNoticeServiceCallback> cblist = new List<ISysNoticeServiceCallback>();
                cblist.Add(callback);
                subscriber.Add(name, cblist);
            }
            else
            {
                List<ISysNoticeServiceCallback> cblist = subscriber[name];
                if(cblist != null && !cblist.Contains(callback))
                {
                    cblist.Add(callback);
                }
            }
        }

        public void Publish(string name, string data)
        {
            Debug.WriteLine("SysNoticeService.Publish : {0} , {1}", name, data);
            if (subscriber.ContainsKey(name))
            {
                List<ISysNoticeServiceCallback> cblist = subscriber[name];
                if (cblist != null)
                {
                    foreach (var item in cblist)
                    {
                        try
                        {
                            item.ReceiveNotice(name, data);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
    }
}
