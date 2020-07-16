using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Read
{
    class ReadResponse : SMB2Body
    {
        readonly ushort _StructureSize = 17;
        byte _DataOffset;
        readonly byte _Reserved = 0;
        uint _DataLength;
        uint _DataRemaining;
        readonly uint _Reserved2 = 0;
        byte[] _Buffer;

        public ushort StructureSize => _StructureSize;
        public byte DataOffset { get => _DataOffset; set => _DataOffset = value; }
        public byte Reserved => _Reserved;
        public uint DataLength { get => _DataLength; set => _DataLength = value; }
        public uint DataRemaining { get => _DataRemaining; set => _DataRemaining = value; }
        public uint Reserved2 => _Reserved2;
        public byte[] Buffer { get => _Buffer; set => _Buffer = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static ReadResponse Parser(byte[] vs, int offset)
        {
            return new ReadResponse(vs, offset);
        }
        ReadResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 17");
            }
            DataOffset = vs[offset + 2];//2-1
            if (Reserved != vs[offset + 3])//3-1
            {
                throw new Exception("Reserved is not 0");
            }
            DataLength = BitConverter.ToUInt32(vs, offset + 4);//4-4
            DataRemaining = BitConverter.ToUInt32(vs, offset + 8);//8-4
            if (Reserved2 != BitConverter.ToUInt32(vs, offset + 12))//12-4
            {
                throw new Exception("Reserved2 is not 0");
            }
            Buffer = new byte[DataLength];
            Array.Copy(vs, DataOffset, Buffer, 0, DataLength);
        }
    }
}
