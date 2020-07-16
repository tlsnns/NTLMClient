using System;
using System.Collections.Generic;
using Common;

namespace SMBMessageParser.Models.Negotiate
{
    class NegotiateRequest : SMB2Body
    {
        readonly ushort _StructureSize = 36;
        ushort _DialectCount;
        ESecurityMode _SecurityMode;
        readonly byte[] _Reserved = new byte[2];
        byte[] _Capabilities;
        Guid _ClientGuid;
        byte[] _ClientStartTime;
        byte[] _Dialects;
        public ushort StructureSize => _StructureSize;
        public ushort DialectCount { get => _DialectCount; private set => _DialectCount = value; }
        public ESecurityMode SecurityMode { get => _SecurityMode; private set => _SecurityMode = value; }
        public byte[] Reserved => _Reserved;
        public byte[] Capabilities { get => _Capabilities; private set => _Capabilities = value; }
        public Guid ClientGuid { get => _ClientGuid; private set => _ClientGuid = value; }
        public byte[] ClientStartTime { get => _ClientStartTime; private set => _ClientStartTime = value; }
        public byte[] Dialects { get => _Dialects; private set => _Dialects = value; }
        public NegotiateRequest(ESecurityMode eSecurityMode, Guid clientGuid)
        {
            DialectCount = 1;
            SecurityMode = eSecurityMode;
            Capabilities = new byte[4];
            ClientGuid = clientGuid;
            ClientStartTime = new byte[8];
            Dialects = new byte[] { 02, 02 };
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(DialectCount.GetBytes());
            vs.AddRange(SecurityMode.GetBytes());
            vs.AddRange(Reserved);
            vs.AddRange(Capabilities);
            vs.AddRange(ClientGuid.ToByteArray());
            vs.AddRange(ClientStartTime);
            vs.AddRange(Dialects);
            return vs;
        }
    }
}
