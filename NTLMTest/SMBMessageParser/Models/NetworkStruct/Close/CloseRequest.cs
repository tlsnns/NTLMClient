using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Close
{
    class CloseRequest : SMB2Body
    {
        readonly ushort _StructureSize = 24;
        ushort _Flags;
        readonly uint _Reserved = 0;
        Guid _FileId;

        public ushort StructureSize => _StructureSize;
        public ushort Flags { get => _Flags; private set => _Flags = value; }
        public uint Reserved => _Reserved;
        public Guid FileId { get => _FileId; private set => _FileId = value; }
        public CloseRequest(Guid fileId)
        {
            Flags = 0;
            FileId = fileId;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(Flags.GetBytes());
            vs.AddRange(Reserved.GetBytes());
            vs.AddRange(FileId.ToByteArray());
            return vs;
        }
    }
}
