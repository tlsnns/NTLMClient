using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SMBMessageParser
{
    public abstract class SMBTransport
    {
        public abstract void SendDatas(List<byte> payloads);
        public abstract byte[] ReceiveDatas();
        public abstract void Close();
    }
}
