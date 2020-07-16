using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.IOCTL
{
    class IOCTLResponse : SMB2Body
    {
        readonly ushort _StructureSize = 49;
        readonly ushort _Reserved = 0;
        CTLCodeType _CtlCode;
        Guid _FileId;
        uint _InputOffset;
        uint _InputCount;
        uint _OutputOffset;
        uint _OutputCount;
        CTLType _Flags;
        readonly uint _Reserved2 = 0;
        byte[] _Buffer;

        public ushort StructureSize => _StructureSize;
        public ushort Reserved => _Reserved;
        public CTLCodeType CtlCode { get => _CtlCode; private set => _CtlCode = value; }
        public Guid FileId { get => _FileId; private set => _FileId = value; }
        public uint InputOffset { get => _InputOffset; private set => _InputOffset = value; }
        public uint InputCount { get => _InputCount; private set => _InputCount = value; }
        public uint OutputOffset { get => _OutputOffset; private set => _OutputOffset = value; }
        public uint OutputCount { get => _OutputCount; private set => _OutputCount = value; }
        public CTLType Flags { get => _Flags; private set => _Flags = value; }
        public uint Reserved2 => _Reserved2;
        public byte[] Buffer { get => _Buffer; set => _Buffer = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static IOCTLResponse Parser(byte[] vs, int offset)
        {
            return new IOCTLResponse(vs, offset);
        }
        IOCTLResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 49");
            }
            if (Reserved != BitConverter.ToUInt16(vs, offset + 2))//2-2
            {
                throw new Exception("Reserved is not 0");
            }
            CtlCode = (CTLCodeType)BitConverter.ToUInt32(vs, offset + 4);//4-4

            var tmp = new byte[16];
            Array.Copy(vs, offset + 8, tmp, 0, 16);//8-16
            FileId = new Guid(tmp);

            InputOffset = BitConverter.ToUInt32(vs, offset + 24);//24-4
            InputCount = BitConverter.ToUInt32(vs, offset + 28);//28-4
            OutputOffset = BitConverter.ToUInt32(vs, offset + 32);//32-4
            OutputCount = BitConverter.ToUInt32(vs, offset + 36);//36-4
            Flags = (CTLType)BitConverter.ToUInt32(vs, offset + 40);//40-4
            if (Reserved2 != BitConverter.ToUInt32(vs, offset + 44))//44-4
            {
                throw new Exception("Reserved is not 0");
            }
            Buffer = new byte[OutputCount];
            Array.Copy(vs, OutputOffset, Buffer, 0, OutputCount);
        }
    }
}
