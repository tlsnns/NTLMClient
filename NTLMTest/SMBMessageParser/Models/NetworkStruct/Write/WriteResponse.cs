using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Write
{
    class WriteResponse : SMB2Body
    {
        readonly ushort _StructureSize = 17;
        readonly ushort _Reserved = 0;
        uint _Count;
        readonly uint _Remaining = 0;
        readonly ushort _WriteChannelInfoOffset = 0;
        readonly ushort _WriteChannelInfoLength = 0;

        public ushort StructureSize => _StructureSize;
        public ushort Reserved => _Reserved;
        public uint Count { get => _Count; private set => _Count = value; }
        public uint Remaining => _Remaining;
        public ushort WriteChannelInfoOffset => _WriteChannelInfoOffset;
        public ushort WriteChannelInfoLength => _WriteChannelInfoLength;

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }

        public static WriteResponse Parser(byte[] vs, int offset)
        {
            return new WriteResponse(vs, offset);
        }
        WriteResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 17");
            }
            if (Reserved != BitConverter.ToUInt16(vs, offset + 2))//2-2
            {
                throw new Exception("Reserved is not 0");
            }
            Count = BitConverter.ToUInt32(vs, offset + 4);//4-4
            if (Remaining != BitConverter.ToUInt32(vs, offset + 8))//8-4
            {
                throw new Exception("Remaining is not 0");
            }
            if (WriteChannelInfoOffset != BitConverter.ToUInt16(vs, offset + 12))//12-2
            {
                throw new Exception("WriteChannelInfoOffset is not 0");
            }
            if (WriteChannelInfoLength != BitConverter.ToUInt16(vs, offset + 14))//14-2
            {
                throw new Exception("WriteChannelInfoLength is not 0");
            }
        }
    }
}
