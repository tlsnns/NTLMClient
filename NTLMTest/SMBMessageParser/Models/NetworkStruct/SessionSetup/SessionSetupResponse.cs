using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.SessionSetup
{
    class SessionSetupResponse : SMB2Body
    {
        readonly ushort _StructureSize = 9;
        ushort _SessionFlags;
        ushort _SecurityBufferOffset;
        ushort _SecurityBufferLength;
        byte[] _SecurityBuffer;

        public ushort StructureSize => _StructureSize;
        public ushort SessionFlags { get => _SessionFlags; private set => _SessionFlags = value; }
        public ushort SecurityBufferOffset { get => _SecurityBufferOffset; private set => _SecurityBufferOffset = value; }
        public ushort SecurityBufferLength { get => _SecurityBufferLength; private set => _SecurityBufferLength = value; }
        public byte[] SecurityBuffer { get => _SecurityBuffer; private set => _SecurityBuffer = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static SessionSetupResponse Parser(byte[] vs, int offset)
        {
            return new SessionSetupResponse(vs, offset);
        }
        SessionSetupResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 9");
            }
            SessionFlags = BitConverter.ToUInt16(vs, offset + 2);//2-4
            SecurityBufferOffset = BitConverter.ToUInt16(vs, offset + 4);//4-6
            SecurityBufferLength = BitConverter.ToUInt16(vs, offset + 6);//6-8

            SecurityBuffer = new byte[SecurityBufferLength];
            Array.Copy(vs, SecurityBufferOffset, SecurityBuffer, 0, SecurityBufferLength);
        }
    }
}
