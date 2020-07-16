using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    class CreateResponse : SMB2Body
    {
        readonly ushort _StructureSize = 89;
        EOplockLevel _OplockLevel;
        byte _Flags = 0;
        ECreateAction _CreateAction;
        DateTimeOffset _CreationTime;
        DateTimeOffset _LastAccessTime;
        DateTimeOffset _LastWriteTime;
        DateTimeOffset _ChangeTime;
        ulong _AllocationSize;
        ulong _EndofFile;
        uint _FileAttributes;
        readonly byte[] _Reserved2 = new byte[4];
        Guid _FileId;
        uint _CreateContextsOffset;
        uint _CreateContextsLength;

        public ushort StructureSize => _StructureSize;
        public EOplockLevel OplockLevel { get => _OplockLevel; private set => _OplockLevel = value; }
        public byte Flags { get => _Flags; private set => _Flags = value; }
        public ECreateAction CreateAction { get => _CreateAction; private set => _CreateAction = value; }
        public DateTimeOffset CreationTime { get => _CreationTime; private set => _CreationTime = value; }
        public DateTimeOffset LastAccessTime { get => _LastAccessTime; private set => _LastAccessTime = value; }
        public DateTimeOffset LastWriteTime { get => _LastWriteTime; private set => _LastWriteTime = value; }
        public DateTimeOffset ChangeTime { get => _ChangeTime; private set => _ChangeTime = value; }
        public ulong AllocationSize { get => _AllocationSize; private set => _AllocationSize = value; }
        public ulong EndofFile { get => _EndofFile; private set => _EndofFile = value; }
        public uint FileAttributes { get => _FileAttributes; private set => _FileAttributes = value; }
        public byte[] Reserved2 => _Reserved2;
        public Guid FileId { get => _FileId; private set => _FileId = value; }
        public uint CreateContextsOffset { get => _CreateContextsOffset; private set => _CreateContextsOffset = value; }
        public uint CreateContextsLength { get => _CreateContextsLength; private set => _CreateContextsLength = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static CreateResponse Parser(byte[] vs, int offset)
        {
            return new CreateResponse(vs, offset);
        }
        CreateResponse(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 16");
            }
            OplockLevel = (EOplockLevel)vs[offset + 2];//2-1
            Flags = vs[offset + 3];//3-1
            CreateAction = (ECreateAction)BitConverter.ToUInt32(vs, offset + 4);//4-4
            CreationTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 8));//8-8
            LastAccessTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 16));//16-8
            LastWriteTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 24));//24-8
            ChangeTime = DateTimeOffset.FromFileTime(BitConverter.ToInt64(vs, offset + 32));//32-8
            AllocationSize = BitConverter.ToUInt64(vs, offset + 40);//40-8
            EndofFile = BitConverter.ToUInt64(vs, offset + 48);//48-8
            FileAttributes = BitConverter.ToUInt32(vs, offset + 56);//56-4
            if (!Reserved2.CompareArray(vs, offset + 60, 4))//60-4
            {
                //throw new Exception("Reserved2 is error");
            }
            var tmp = new byte[16];
            Array.Copy(vs, offset + 64, tmp, 0, 16);//64-16
            FileId = new Guid(tmp);
            CreateContextsOffset = BitConverter.ToUInt32(vs, offset + 80);//80-4
            CreateContextsLength = BitConverter.ToUInt32(vs, offset + 84);//84-4
        }
    }
}
