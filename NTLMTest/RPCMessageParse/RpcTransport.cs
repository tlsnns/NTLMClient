using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    abstract class RpcTransport
    {
        public abstract void SendDatas(List<byte> payloads);
        public abstract byte[] ReceiveDatas();
        public abstract void Close();
    }
}
