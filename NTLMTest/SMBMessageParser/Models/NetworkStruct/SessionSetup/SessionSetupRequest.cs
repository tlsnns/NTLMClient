using Common;
using System.Collections.Generic;

namespace SMBMessageParser.Models.SessionSetup
{
    class SessionSetupRequest : SMB2Body
    {
        readonly ushort _StructureSize = 25;
        byte _Flags;
        byte _SecurityMode;
        byte[] _Capabilities;
        readonly byte[] _Channel = new byte[4];
        ushort _SecurityBufferOffset;
        ushort _SecurityBufferLength;
        byte[] _PreviousSessionId;
        byte[] _SecurityBuffer;

        public ushort StructureSize => _StructureSize;
        public byte Flags { get => _Flags; private set => _Flags = value; }
        public byte SecurityMode { get => _SecurityMode; private set => _SecurityMode = value; }
        public byte[] Capabilities { get => _Capabilities; private set => _Capabilities = value; }
        public byte[] Channel => _Channel;
        public ushort SecurityBufferOffset { get => _SecurityBufferOffset; private set => _SecurityBufferOffset = value; }
        public ushort SecurityBufferLength { get => _SecurityBufferLength; private set => _SecurityBufferLength = value; }
        public byte[] PreviousSessionId { get => _PreviousSessionId; private set => _PreviousSessionId = value; }
        public byte[] SecurityBuffer { get => _SecurityBuffer; private set => _SecurityBuffer = value; }
        public SessionSetupRequest(byte[] securityBuffer)
        {
            Flags = 0;
            SecurityMode = 1;
            Capabilities = new byte[4];
            SecurityBufferOffset = 64 + 24;
            SecurityBufferLength = (ushort)securityBuffer.Length;
            PreviousSessionId = new byte[8];
            SecurityBuffer = securityBuffer;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.Add(Flags);
            vs.Add(SecurityMode);
            vs.AddRange(Capabilities);
            vs.AddRange(Channel);
            vs.AddRange(SecurityBufferOffset.GetBytes());
            vs.AddRange(SecurityBufferLength.GetBytes());
            vs.AddRange(PreviousSessionId);
            vs.AddRange(SecurityBuffer);
            return vs;
        }
    }
}
