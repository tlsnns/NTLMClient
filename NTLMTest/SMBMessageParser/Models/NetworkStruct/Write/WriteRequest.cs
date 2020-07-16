using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Write
{
    class WriteRequest : SMB2Body
    {
        readonly ushort _StructureSize = 49;
        ushort _DataOffset;
        uint _Length;
        ulong _Offset;
        Guid _FileId;
        uint _Channel;
        uint _RemainingBytes;
        ushort _WriteChannelInfoOffset;
        ushort _WriteChannelInfoLength;
        uint _Flags;
        byte[] _Buffer;

        public ushort StructureSize => _StructureSize;
        public ushort DataOffset { get => _DataOffset; private set => _DataOffset = value; }
        public uint Length { get => _Length; private set => _Length = value; }
        public ulong Offset { get => _Offset; private set => _Offset = value; }
        public Guid FileId { get => _FileId; private set => _FileId = value; }
        public uint Channel { get => _Channel; private set => _Channel = value; }
        public uint RemainingBytes { get => _RemainingBytes; private set => _RemainingBytes = value; }
        public ushort WriteChannelInfoOffset { get => _WriteChannelInfoOffset; private set => _WriteChannelInfoOffset = value; }
        public ushort WriteChannelInfoLength { get => _WriteChannelInfoLength; private set => _WriteChannelInfoLength = value; }
        public uint Flags { get => _Flags; private set => _Flags = value; }
        public byte[] Buffer { get => _Buffer; private set => _Buffer = value; }

        public WriteRequest(ulong offset, Guid fileId, byte[] buffer)
        {
            DataOffset = 64 + 48;
            Length = (uint)buffer.Length;
            Offset = offset;
            FileId = fileId;
            Channel = 0;
            RemainingBytes = 0;
            WriteChannelInfoOffset = 0;
            WriteChannelInfoLength = 0;
            Flags = 0;
            Buffer = buffer;
        }

        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(DataOffset.GetBytes());
            vs.AddRange(Length.GetBytes());
            vs.AddRange(Offset.GetBytes());
            vs.AddRange(FileId.ToByteArray());
            vs.AddRange(Channel.GetBytes());
            vs.AddRange(RemainingBytes.GetBytes());
            vs.AddRange(WriteChannelInfoOffset.GetBytes());
            vs.AddRange(WriteChannelInfoLength.GetBytes());
            vs.AddRange(Flags.GetBytes());
            vs.AddRange(Buffer);
            return vs;
        }
    }
}
