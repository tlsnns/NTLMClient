using Common;
using System;
using System.Collections.Generic;

namespace SMBMessageParser.Models.Negotiate
{
    class NegotiateResponse : SMB2Body
    {
        readonly ushort _StructureSize = 65;
        ESecurityMode _SecurityMode;
        ushort _DialectRevision;
        readonly byte[] _Reserved = new byte[2];
        Guid _ServerGuid;
        byte[] _Capabilities;
        uint _MaxTransactSize;
        uint _MaxReadSize;
        uint _MaxWriteSize;
        DateTimeOffset _SystemTime;
        DateTimeOffset _ServerStartTime;
        ushort _SecurityBufferOffset;
        ushort _SecurityBufferLength;
        readonly byte[] _Reserved2 = new byte[4];
        byte[] _SecurityBuffer;

        public ushort StructureSize => _StructureSize;
        internal ESecurityMode SecurityMode { get => _SecurityMode; private set => _SecurityMode = value; }
        public ushort DialectRevision { get => _DialectRevision; private set => _DialectRevision = value; }
        public byte[] Reserved => _Reserved;
        public Guid ServerGuid { get => _ServerGuid; private set => _ServerGuid = value; }
        public byte[] Capabilities { get => _Capabilities; private set => _Capabilities = value; }
        public uint MaxTransactSize { get => _MaxTransactSize; private set => _MaxTransactSize = value; }
        public uint MaxReadSize { get => _MaxReadSize; private set => _MaxReadSize = value; }
        public uint MaxWriteSize { get => _MaxWriteSize; private set => _MaxWriteSize = value; }
        public DateTimeOffset SystemTime { get => _SystemTime; private set => _SystemTime = value; }
        public DateTimeOffset ServerStartTime { get => _ServerStartTime; private set => _ServerStartTime = value; }
        public ushort SecurityBufferOffset { get => _SecurityBufferOffset; private set => _SecurityBufferOffset = value; }
        public ushort SecurityBufferLength { get => _SecurityBufferLength; private set => _SecurityBufferLength = value; }
        public byte[] Reserved2 => _Reserved2;
        public byte[] SecurityBuffer { get => _SecurityBuffer; private set => _SecurityBuffer = value; }


        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static NegotiateResponse Parser(byte[] vs, int offset)
        {
            return new NegotiateResponse(vs, offset);
        }
        NegotiateResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 65");
            }
            SecurityMode = (ESecurityMode)BitConverter.ToUInt16(vs, offset + 2);//2-2
            DialectRevision = BitConverter.ToUInt16(vs, offset + 4);//4-2
            if (!Reserved.CompareArray(vs, offset + 6, 2))//6-2
            {
                throw new Exception("Reserved is error");
            }

            byte[] guid = new byte[16];
            Array.Copy(vs, offset + 8, guid, 0, 16);//8-16
            ServerGuid = new Guid(guid);

            Capabilities = new byte[4];
            Array.Copy(vs, offset + 24, Capabilities, 0, 4);//24-4

            MaxTransactSize = BitConverter.ToUInt32(vs, offset + 28);//28-4
            MaxReadSize = BitConverter.ToUInt32(vs, offset + 32);//32-4
            MaxWriteSize = BitConverter.ToUInt32(vs, offset + 36);//36-4

            long systemTime = BitConverter.ToInt64(vs, offset + 40);//40-8
            SystemTime = DateTimeOffset.FromFileTime(systemTime);

            long serverStartTime = BitConverter.ToInt64(vs, offset + 48);//48-8
            ServerStartTime = DateTimeOffset.FromFileTime(serverStartTime);

            SecurityBufferOffset = BitConverter.ToUInt16(vs, offset + 56);//56-2
            SecurityBufferLength = BitConverter.ToUInt16(vs, offset + 58);//58-2

            if (!Reserved2.CompareArray(vs, offset + 60, 4))//60-4
            {
             //   throw new Exception("Reserved2 is error");
            }
        }
    }
}
