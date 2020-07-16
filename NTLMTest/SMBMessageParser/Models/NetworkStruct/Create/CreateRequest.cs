using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    class CreateRequest : SMB2Body
    {
        readonly ushort _StructureSize = 57;
        readonly byte _SecurityFlags = 0;
        EOplockLevel _RequestedOplockLevel;
        EImpersonationLevel _ImpersonationLevel;
        readonly byte[] _SmbCreateFlags = new byte[8];
        readonly byte[] _Reserved = new byte[8];
        FilePipePrinterAccessMaskFlags _DesiredAccess;
        uint _FileAttributes;
        ShareAccessFlags _ShareAccess;
        ECreateDisposition _CreateDisposition;
        uint _CreateOptions;
        ushort _NameOffset;
        ushort _NameLength;
        uint _CreateContextsOffset;
        uint _CreateContextsLength;
        byte[] _NameBuffer;

        public ushort StructureSize => _StructureSize;
        public byte SecurityFlags => _SecurityFlags;
        public EOplockLevel RequestedOplockLevel { get => _RequestedOplockLevel; set => _RequestedOplockLevel = value; }
        public EImpersonationLevel ImpersonationLevel { get => _ImpersonationLevel; set => _ImpersonationLevel = value; }
        public byte[] SmbCreateFlags => _SmbCreateFlags;
        public byte[] Reserved => _Reserved;
        public FilePipePrinterAccessMaskFlags DesiredAccess { get => _DesiredAccess; set => _DesiredAccess = value; }
        public uint FileAttributes { get => _FileAttributes; set => _FileAttributes = value; }
        public ShareAccessFlags ShareAccess { get => _ShareAccess; set => _ShareAccess = value; }
        public ECreateDisposition CreateDisposition { get => _CreateDisposition; set => _CreateDisposition = value; }
        public uint CreateOptions { get => _CreateOptions; set => _CreateOptions = value; }
        public ushort NameOffset { get => _NameOffset; set => _NameOffset = value; }
        public ushort NameLength { get => _NameLength; set => _NameLength = value; }
        public uint CreateContextsOffset { get => _CreateContextsOffset; set => _CreateContextsOffset = value; }
        public uint CreateContextsLength { get => _CreateContextsLength; set => _CreateContextsLength = value; }
        public byte[] NameBuffer { get => _NameBuffer; set => _NameBuffer = value; }

        public CreateRequest(FilePipePrinterAccessMaskFlags accessMask, ShareAccessFlags shareAccess, ECreateDisposition createDisposition, uint createOptions, byte[] name)
        {
            RequestedOplockLevel = EOplockLevel.SMB2_OPLOCK_LEVEL_NONE;
            ImpersonationLevel = EImpersonationLevel.Impersonation;
            DesiredAccess = accessMask;
            FileAttributes = 0;
            ShareAccess = shareAccess;
            CreateDisposition = createDisposition;
            CreateOptions = createOptions;
            NameOffset = 64 + 56;
            NameLength = (ushort)name.Length;
            CreateContextsOffset = 0;
            CreateContextsLength = 0;
            NameBuffer = name;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.Add(SecurityFlags);
            vs.Add((byte)RequestedOplockLevel);
            vs.AddRange(ImpersonationLevel.GetBytes());
            vs.AddRange(SmbCreateFlags);
            vs.AddRange(Reserved);
            vs.AddRange(DesiredAccess.GetBytes());
            vs.AddRange(FileAttributes.GetBytes());
            vs.AddRange(ShareAccess.GetBytes());
            vs.AddRange(CreateDisposition.GetBytes());
            vs.AddRange(CreateOptions.GetBytes());
            vs.AddRange(NameOffset.GetBytes());
            vs.AddRange(NameLength.GetBytes());
            vs.AddRange(CreateContextsOffset.GetBytes());
            vs.AddRange(CreateContextsLength.GetBytes());
            vs.AddRange(NameBuffer);
            return vs;
        }
    }
}
