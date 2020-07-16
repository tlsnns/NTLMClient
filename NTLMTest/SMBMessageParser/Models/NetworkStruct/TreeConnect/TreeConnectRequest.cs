using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.TreeConnect
{
    class TreeConnectRequest : SMB2Body
    {
        readonly ushort _StructureSize = 9;
        byte[] _Reserved;
        ushort _PathOffset;
        ushort _PathLength;
        byte[] _Path;


        public ushort StructureSize => _StructureSize;
        public byte[] Reserved { get => _Reserved; private set => _Reserved = value; }
        public ushort PathOffset { get => _PathOffset; private set => _PathOffset = value; }
        public ushort PathLength { get => _PathLength; private set => _PathLength = value; }
        public byte[] Path { get => _Path; private set => _Path = value; }

        public TreeConnectRequest(byte[] path)
        {
            Reserved = new byte[2];
            PathOffset = 64 + 8;
            PathLength = (ushort)path.Length;
            Path = path;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(Reserved);
            vs.AddRange(PathOffset.GetBytes());
            vs.AddRange(PathLength.GetBytes());
            vs.AddRange(Path);
            return vs;
        }
    }
}
