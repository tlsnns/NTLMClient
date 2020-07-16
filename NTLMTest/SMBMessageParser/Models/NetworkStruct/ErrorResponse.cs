using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct
{
    class ErrorResponse : SMB2Body
    {
        readonly ushort _StructureSize = 9;
        byte _ErrorContextCount;
        byte _Reserved;
        uint _ByteCount;
        byte[] _ErrorData;

        public ushort StructureSize => _StructureSize;
        public byte ErrorContextCount { get => _ErrorContextCount; private set => _ErrorContextCount = value; }
        public byte Reserved { get => _Reserved; private set => _Reserved = value; }
        public uint ByteCount { get => _ByteCount; private set => _ByteCount = value; }
        public byte[] ErrorDatas { get => _ErrorData; private set => _ErrorData = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static ErrorResponse Parser(byte[] vs, int offset)
        {
            return new ErrorResponse(vs, offset);
        }
        ErrorResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 9");
            }
            ErrorContextCount = vs[offset + 2];//2-1
            if (Reserved != vs[offset + 3])//3-1
            {
                throw new Exception("Reserved is not 0");
            }
            ByteCount = BitConverter.ToUInt32(vs, offset + 4);//4-4
            ErrorDatas = new byte[ByteCount];
            Array.Copy(vs, offset + 8, ErrorDatas, 0, ByteCount);
        }
    }
}
