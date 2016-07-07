using APM.Notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;
using System.Runtime.Caching;

namespace APM.Admin
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SysNoticeService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SysNoticeService.svc 或 SysNoticeService.svc.cs，然后开始调试。
    public class SysNoticeService : ISysNoticeService
    {
        private Timer _timer;
        private List<ISysNoticeServiceCallback> callbacks;

        public SysNoticeService()
        {
            callbacks = new List<ISysNoticeServiceCallback>();
            _timer = new Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ObjectCache cache = MemoryCache.Default;
                string message = cache["sysnotice_message"] as string;
                if (!string.IsNullOrEmpty(message))
                {
                    this.Send(message);
                }
            }
            catch (Exception)
            {
                
            }
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

        public void Send(string msg)
        {
            foreach (var item in callbacks)
            {
                try
                {
                    item.ReceiveNotice(msg);
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}
