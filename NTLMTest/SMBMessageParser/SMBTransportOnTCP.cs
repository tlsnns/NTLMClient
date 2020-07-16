using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SMBMessageParser
{
    class SMBTransportOnTCP : SMBTransport
    {
        Socket SocketWorker;
        public SMBTransportOnTCP(string strIpAddress, int destinationPort)
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
            var tmp = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(payloads.Count));
            if (tmp[0] != 0)
            {
                throw new Exception("error SMB Transport package");
            }
            List<byte> vs = new List<byte>();
            vs.AddRange(tmp);
            vs.AddRange(payloads);
            SocketWorker.BeginSend(vs.ToArray(), 0, vs.Count, SocketFlags.None, null, null);
        }

        public override byte[] ReceiveDatas()
        {
            byte[] tmpBuff;
            try
            {
                tmpBuff = new byte[4];
                int c = SocketWorker.Receive(tmpBuff, 0, tmpBuff.Length, SocketFlags.None);
                if (c != 4 || tmpBuff[0] != 0)
                {
                    throw new Exception("smbTransport Receive error");
                }
                var streamProtocolLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(tmpBuff, 0));
                tmpBuff = new byte[streamProtocolLength];
                c = SocketWorker.Receive(tmpBuff, SocketFlags.None);
                if (c != streamProtocolLength)
                {
                    throw new Exception("smbTransport Receive Length error");
                }
            }
            catch
            {
                tmpBuff = null;
            }
            return tmpBuff;
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
