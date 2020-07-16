using System;
using System.Collections.Generic;
using System.Text;

namespace NTLMMessageParse.Models
{
    abstract class NTLMMessage
    {
        byte[] _Signature = Encoding.ASCII.GetBytes("NTLMSSP\0");//Fixed 8-byte signature
        uint _MessageType;
        private NegotiateFlags _NegotiateFlag;
        private Versions _Version;

        public byte[] Signature { get => _Signature;}
        public uint MessageType { get => _MessageType; protected set => _MessageType = value; }
        public NegotiateFlags NegotiateFlag { get => _NegotiateFlag; protected set => _NegotiateFlag = value; }
        public Versions Version { get => _Version; protected set => _Version = value; }

        abstract public List<byte> DumpBinary();
    }
}
