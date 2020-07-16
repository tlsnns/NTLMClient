using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace SMBMessageParser.Models.NetworkStruct.IOCTL
{
    class IOCTLRequest : SMB2Body
    {
        readonly ushort _StructureSize = 57;
        readonly ushort _Reserved = 0;
        CTLCodeType _CtlCode;
        Guid _FileId;
        uint _InputOffset;
        uint _InputCount;
        uint _MaxInputResponse;
        uint _OutputOffset;
        uint _OutputCount;
        uint _MaxOutputResponse;
        CTLType _Flags;
        readonly uint _Reserved2 = 0;
        byte[] _Buffer;

        public ushort StructureSize => _StructureSize;
        public ushort Reserved => _Reserved;
        public CTLCodeType CtlCode { get => _CtlCode; private set => _CtlCode = value; }
        public Guid FileId { get => _FileId; private set => _FileId = value; }
        public uint InputOffset { get => _InputOffset; private set => _InputOffset = value; }
        public uint InputCount { get => _InputCount; private set => _InputCount = value; }
        public uint MaxInputResponse { get => _MaxInputResponse; private set => _MaxInputResponse = value; }
        public uint OutputOffset { get => _OutputOffset; private set => _OutputOffset = value; }
        public uint OutputCount { get => _OutputCount; private set => _OutputCount = value; }
        public uint MaxOutputResponse { get => _MaxOutputResponse; private set => _MaxOutputResponse = value; }
        public CTLType Flags { get => _Flags; private set => _Flags = value; }
        public uint Reserved2 => _Reserved2;
        public byte[] Buffer { get => _Buffer; set => _Buffer = value; }

        public IOCTLRequest(CTLCodeType ctlCode, Guid fileId, CTLType flags, byte[] buffer)
        {
            CtlCode = ctlCode;
            FileId = fileId;
            InputOffset = 64 + 56;
            InputCount = (uint)buffer.Length;
            MaxInputResponse = 4096;
            OutputOffset = 0;
            OutputCount = 0;
            MaxOutputResponse = 4096;
            Flags = flags;
            Buffer = buffer;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(Reserved.GetBytes());
            vs.AddRange(CtlCode.GetBytes());
            vs.AddRange(FileId.ToByteArray());
            vs.AddRange(InputOffset.GetBytes());
            vs.AddRange(InputCount.GetBytes());
            vs.AddRange(MaxInputResponse.GetBytes());
            vs.AddRange(OutputOffset.GetBytes());
            vs.AddRange(OutputCount.GetBytes());
            vs.AddRange(MaxOutputResponse.GetBytes());
            vs.AddRange(Flags.GetBytes());
            vs.AddRange(Reserved2.GetBytes());
            vs.AddRange(Buffer);
            return vs;
        }
    }
}
