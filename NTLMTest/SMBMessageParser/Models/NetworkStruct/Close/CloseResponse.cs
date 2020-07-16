using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Close
{
    class CloseResponse : SMB2Body
    {
        readonly ushort _StructureSize = 60;
        ushort _Flags;
        readonly uint _Reserved = 0;
        DateTimeOffset _CreationTime;
        DateTimeOffset _LastAccessTime;
        DateTimeOffset _LastWriteTime;
        DateTimeOffset _ChangeTime;
        ulong _AllocationSize;
        ulong _EndofFile;
        uint _FileAttributes;

        public ushort StructureSize => _StructureSize;
        public ushort Flags { get => _Flags; private set => _Flags = value; }
        public uint Reserved => _Reserved;
        public DateTimeOffset CreationTime { get => _CreationTime; private set => _CreationTime = value; }
        public DateTimeOffset LastAccessTime { get => _LastAccessTime; private set => _LastAccessTime = value; }
        public DateTimeOffset LastWriteTime { get => _LastWriteTime; private set => _LastWriteTime = value; }
        public DateTimeOffset ChangeTime { get => _ChangeTime; private set => _ChangeTime = value; }
        public ulong AllocationSize { get => _AllocationSize; private set => _AllocationSize = value; }
        public ulong EndofFile { get => _EndofFile; private set => _EndofFile = value; }
        public uint FileAttributes { get => _FileAttributes; private set => _FileAttributes = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static CloseResponse Parser(byte[] vs, int offset)
        {
            return new CloseResponse(vs, offset);
        }
        CloseResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 60");
            }
            Flags = BitConverter.ToUInt16(vs, offset + 2);//2-2
            if (Reserved != BitConverter.ToUInt32(vs, offset + 4))//4-4
            {
                throw new Exception("Reserved is not 0");
            }
            CreationTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 8));//8-8
            LastAccessTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 16));//16-8
            LastWriteTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 24));//24-8
            ChangeTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 32));//32-8
            AllocationSize = BitConverter.ToUInt64(vs, offset + 40);//40-8
            EndofFile = BitConverter.ToUInt64(vs, offset + 48);//48-8
            FileAttributes = BitConverter.ToUInt32(vs, offset + 56);//56-4
        }
    }
}
