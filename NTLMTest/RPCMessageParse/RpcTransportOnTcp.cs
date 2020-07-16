using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    class RpcTransportOnTcp : RpcTransport
    {
        Socket SocketWorker;
        public RpcTransportOnTcp(string strIpAddress, int destinationPort)
        {
            IPAddress ipa = IPAddress.Parse(strIpAddress);
            IPEndPoint ipep = new IPEndPoint(ipa, destinationPort);
            SocketWorker = new Socket(ipep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketWorker.Connect(ipep);
            if (!SocketWorker.Connected)
            {
                throw new SocketException();
            }
        }
        public override void SendDatas(List<byte> payloads)
        {
            SocketWorker.Send(payloads.ToArray(), 0, payloads.Count, SocketFlags.None);
        }

        public override byte[] ReceiveDatas()
        {
            List<byte> vs = new List<byte>();
            try
            {
                byte[] tmpBuff = new byte[10];
                int c = SocketWorker.Receive(tmpBuff, SocketFlags.None);
                if (c != 10)
                {
                    throw new Exception("rpcTransport Receive Length error");
                }
                vs.AddRange(tmpBuff);

                var fragLength = BitConverter.ToUInt16(tmpBuff, 8);
                var lastLength = fragLength - 10;
                tmpBuff = new byte[lastLength];
                c = SocketWorker.Receive(tmpBuff, SocketFlags.None);
                if (c != lastLength)
                {
                    throw new Exception("rpcTransport Receive lastLength error");
                }
                vs.AddRange(tmpBuff);
            }
            catch
            {
                return null;
            }
            return vs.ToArray();
        }

        public override void Close()
        {
            try
            {
                SocketWorker.Shutdown(SocketShutdown.Receive);
                SocketWorker.Shutdown(SocketShutdown.Send);
                SocketWorker.Disconnect(false);
            }
            finally
            {
                SocketWorker.Close();
                SocketWorker.Dispose();
            }
        }
    }
}
