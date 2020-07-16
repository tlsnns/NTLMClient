using System.Collections.Generic;

namespace NTLMMessageParse.Models
{
    abstract class MetaDataPayload
    {
        ushort _Len;
        ushort _MaxLen;
        uint _BufferOffset;
        byte[] _Buffer;
        public ushort Len { get => _Len; protected set => _Len = value; }
        public ushort MaxLen { get => _MaxLen; protected set => _MaxLen = value; }
        public uint BufferOffset { get => _BufferOffset; protected set => _BufferOffset = value; }
        public byte[] Buffer { get => _Buffer; protected set => _Buffer = value; }

        abstract public List<byte> DumpMetaData();

        abstract public List<byte> DumpBuffer();
    }
}
