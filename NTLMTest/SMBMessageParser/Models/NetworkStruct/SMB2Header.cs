using Common;
using System;
using System.Collections.Generic;

namespace SMBMessageParser.Models
{
    class SMB2Header : IBinary
    {
        readonly byte[] _ProtocolId = new byte[] { 0xfe, 0x53, 0x4d, 0x42 };
        readonly ushort _StructureSize = 64;
        ushort _CreditCharge;
        NTStateType _Status;
        ESMB2Command _Command;
        ushort _Credit;
        SMB2HeaderFlags _Flags;
        uint _NextCommand;
        ulong _MessageId;
        readonly byte[] _Reserved = new byte[] { 0xff, 0xfe, 0, 0 };
        uint _TreeId;
        ulong _SessionId;
        byte[] _Signature;
        public byte[] ProtocolId { get => _ProtocolId; }
        public ushort StructureSize { get => _StructureSize; }
        public ushort CreditCharge { get => _CreditCharge; private set => _CreditCharge = value; }
        public NTStateType Status { get => _Status; private set => _Status = value; }
        public ESMB2Command Command { get => _Command; private set => _Command = value; }
        public ushort Credit { get => _Credit; private set => _Credit = value; }
        public SMB2HeaderFlags Flag { get => _Flags; private set => _Flags = value; }
        public uint NextCommand { get => _NextCommand; private set => _NextCommand = value; }
        public ulong MessageId { get => _MessageId; private set => _MessageId = value; }
        public byte[] Reserved => _Reserved;
        public uint TreeId { get => _TreeId; private set => _TreeId = value; }
        public ulong SessionId { get => _SessionId; private set => _SessionId = value; }
        public byte[] Signature { get => _Signature; private set => _Signature = value; }
        public SMB2Header(ESMB2Command command, SMB2HeaderFlags flags, ulong messageId, uint treeId, ulong sessionId, byte[] signature)
        {
            CreditCharge = 0;
            Status = 0;
            Command = command;
            Credit = 1;
            Flag = flags;
            NextCommand = 0;
            MessageId = messageId;
            TreeId = treeId;
            SessionId = sessionId;
            Signature = signature;
        }
        public SMB2Header(ESMB2Command command, SMB2HeaderFlags flags, ulong messageId, uint treeId, ulong sessionId) : this(command, flags, messageId, treeId, sessionId, new byte[16])
        {
            if (flags.HasFlag(SMB2HeaderFlags.Signed))
            {
                throw new ArgumentException("arg flags can't has Signed flag,because Signature is zeor");
            }
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(ProtocolId);
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(CreditCharge.GetBytes());
            vs.AddRange(Status.GetBytes());
            vs.AddRange(Command.GetBytes());
            vs.AddRange(Credit.GetBytes());
            vs.AddRange(Flag.GetBytes());
            vs.AddRange(NextCommand.GetBytes());
            vs.AddRange(MessageId.GetBytes());
            vs.AddRange(Reserved);
            vs.AddRange(TreeId.GetBytes());
            vs.AddRange(SessionId.GetBytes());
            vs.AddRange(Signature);
            return vs;
        }
        public static SMB2Header Parser(byte[] vs, int offset)
        {
            return new SMB2Header(vs, offset);
        }
        SMB2Header(byte[] vs, int offset)
        {
            if (!ProtocolId.CompareArray(vs, offset, 4))//0-4
            {
                throw new Exception("ProtocolId is error");
            }
            if (StructureSize != BitConverter.ToUInt16(vs, offset + 4))//4-2
            {
                throw new Exception("StructureSize is not 64");
            }
            CreditCharge = BitConverter.ToUInt16(vs, offset + 6);//6-2
            Status = (NTStateType)BitConverter.ToUInt32(vs, offset + 8);//8-4
            Command = (ESMB2Command)BitConverter.ToUInt16(vs, offset + 12);//12-2
            Credit = BitConverter.ToUInt16(vs, offset + 14);//14-2
            Flag = (SMB2HeaderFlags)BitConverter.ToUInt32(vs, offset + 16);//16-4
            NextCommand = BitConverter.ToUInt32(vs, offset + 20);//20-4
            MessageId = BitConverter.ToUInt64(vs, offset + 24);//24-8
            if (!Reserved.CompareArray(vs, offset + 32, 4))//32-4
            {
                // throw new Exception("Reserved is not 0");
            }
            TreeId = BitConverter.ToUInt32(vs, offset + 36);//36-4
            SessionId = BitConverter.ToUInt64(vs, offset + 40);//40-8

            Signature = new byte[16];
            Array.Copy(vs, offset + 48, Signature, 0, 16);//48-16
        }
    }
}
