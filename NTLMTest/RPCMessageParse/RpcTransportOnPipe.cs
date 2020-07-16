using SMBMessageParser.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    class RpcTransportOnPipe : RpcTransport
    {
        PipeClient PC;
        public RpcTransportOnPipe(string ipAddress, string strNamePipe, string userName, string password)
        {
            PC = new PipeClient(ipAddress, strNamePipe, PipeDirection.ReadWrite, userName, password);
            PC.Connect();
        }

        public override void Close()
        {
            PC.Close();
        }

        public override byte[] ReceiveDatas()
        {
            return PC.Read(8192);
        }

        public override void SendDatas(List<byte> payloads)
        {
            PC.Write(payloads.ToArray());
        }
    }
}
