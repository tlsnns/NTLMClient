using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.TreeConnect
{
    class TreeConnectResponse : SMB2Body
    {
        readonly ushort _StructureSize = 16;
        byte _ShareType;
        readonly byte _Reserved = 0;
        byte[] _ShareFlags;
        byte[] _Capabilities;
        byte[] _MaximalAccess;

        public ushort StructureSize => _StructureSize;
        public byte ShareType { get => _ShareType; private set => _ShareType = value; }
        public byte Reserved => _Reserved;
        public byte[] ShareFlags { get => _ShareFlags; private set => _ShareFlags = value; }
        public byte[] Capabilities { get => _Capabilities; private set => _Capabilities = value; }
        public byte[] MaximalAccess { get => _MaximalAccess; private set => _MaximalAccess = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static TreeConnectResponse Parser(byte[] vs, int offset)
        {
            return new TreeConnectResponse(vs, offset);
        }
        TreeConnectResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 16");
            }
            ShareType = vs[offset + 2];//2-1
            if (Reserved != vs[offset + 3])//3-1
            {
                throw new Exception("Reserved is error");
            }
            ShareFlags = new byte[4];
            Array.Copy(vs, offset + 4, ShareFlags, 0, 4);//4-4

            Capabilities = new byte[4];
            Array.Copy(vs, offset + 8, Capabilities, 0, 4);//8-4

            MaximalAccess = new byte[4];
            Array.Copy(vs, offset + 12, MaximalAccess, 0, 4);//12-4
        }
    }
}
