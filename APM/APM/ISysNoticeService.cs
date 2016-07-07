using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace APM.Notice
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISysNoticeService”。
    [ServiceContract(CallbackContract = typeof(ISysNoticeServiceCallback))]
    public interface ISysNoticeService
    {
        [OperationContract(IsOneWay = true)]
        void OnLine();

        [OperationContract(IsOneWay = true)]
        void OffLine();
    }

    public interface ISysNoticeServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveNotice(string notice);
    }
}
