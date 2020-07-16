using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Read
{
    class ReadRequest : SMB2Body
    {
        readonly ushort _StructureSize = 49;
        byte _Padding;
        byte _Flags;
        uint _Length;
        ulong _Offset;
        Guid _FileId;
        uint _MinimumCount;
        uint _Channel;
        uint _RemainingBytes;
        ushort _ReadChannelInfoOffset;
        ushort _ReadChannelInfoLength;
        readonly byte _Reserved = 0;//need this zero

        public ushort StructureSize => _StructureSize;
        public byte Padding { get => _Padding; private set => _Padding = value; }
        public byte Flags { get => _Flags; private set => _Flags = value; }
        public uint Length { get => _Length; private set => _Length = value; }
        public ulong Offset { get => _Offset; private set => _Offset = value; }
        public Guid FileId { get => _FileId; private set => _FileId = value; }
        public uint MinimumCount { get => _MinimumCount; private set => _MinimumCount = value; }
        public uint Channel { get => _Channel; private set => _Channel = value; }
        public uint RemainingBytes { get => _RemainingBytes; private set => _RemainingBytes = value; }
        public ushort ReadChannelInfoOffset { get => _ReadChannelInfoOffset; private set => _ReadChannelInfoOffset = value; }
        public ushort ReadChannelInfoLength { get => _ReadChannelInfoLength; private set => _ReadChannelInfoLength = value; }
        public byte Reserved => _Reserved;

        public ReadRequest(uint length, Guid fileId)
        {
            Padding = 64 + 16;
            Flags = 0;
            Length = length;
            Offset = 0;
            FileId = fileId;
            MinimumCount = 0;
            Channel = 0;
            RemainingBytes = 0;
            ReadChannelInfoOffset = 0;
            ReadChannelInfoLength = 0;
        }

        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.Add(Padding);
            vs.Add(Flags);
            vs.AddRange(Length.GetBytes());
            vs.AddRange(Offset.GetBytes());
            vs.AddRange(FileId.ToByteArray());
            vs.AddRange(MinimumCount.GetBytes());
            vs.AddRange(Channel.GetBytes());
            vs.AddRange(RemainingBytes.GetBytes());
            vs.AddRange(ReadChannelInfoOffset.GetBytes());
            vs.AddRange(ReadChannelInfoLength.GetBytes());
            vs.Add(Reserved);
            return vs;
        }
    }
}
