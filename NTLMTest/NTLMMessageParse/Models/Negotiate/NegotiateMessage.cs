using System;
using System.Collections.Generic;
using Common;

namespace NTLMMessageParse.Models.Negotiate
{
    class NegotiateMessage : NTLMMessage
    {
        MetaDataPayloadString _DomainNameField;
        MetaDataPayloadString _WorkstationField;

        public MetaDataPayloadString DomainNameField { get => _DomainNameField; private set => _DomainNameField = value; }
        public MetaDataPayloadString WorkstationField { get => _WorkstationField; private set => _WorkstationField = value; }

        NegotiateMessage(NegotiateFlags negotiateFlags, MetaDataPayloadString domainNameField, MetaDataPayloadString workstationField)
        {
            MessageType = 0x1;
            NegotiateFlag = negotiateFlags;
            DomainNameField = domainNameField;
            WorkstationField = workstationField;
            Version = new Versions();
        }
        public NegotiateMessage(byte[] cm)
        {
            if (!Signature.CompareArray(cm, 0, 8))//0-8
            {
                throw new Exception("ChallengeMessage Signature Error");
            }
            MessageType = 1;
            if (MessageType != BitConverter.ToUInt32(cm, 8))//8-4
            {
                throw new Exception("ChallengeMessage MessageType Error");
            }
            NegotiateFlag = (NegotiateFlags)BitConverter.ToUInt32(cm, 12);//12-4
            DomainNameField = new MetaDataPayloadString(cm, 16);//16-8
            WorkstationField = new MetaDataPayloadString(cm, 24);//24-8
            if (NegotiateFlag.HasFlag(NegotiateFlags.NEGOTIATE_VERSION))
            {
                Version = new Versions(cm, 32);//32-8
            }
            if (NegotiateFlag.HasFlag(NegotiateFlags.NEGOTIATE_UNICODE))
            {
                DomainNameField.ToString();
                WorkstationField.ToString();
            }
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(Signature);
            vs.AddRange(MessageType.GetBytes());
            vs.AddRange(NegotiateFlag.GetBytes());
            vs.AddRange(DomainNameField.DumpMetaData());
            vs.AddRange(WorkstationField.DumpMetaData());
            vs.AddRange(Version.DumpBinary());

            vs.AddRange(DomainNameField.DumpBuffer());
            vs.AddRange(WorkstationField.DumpBuffer());
            return vs;
        }
        static public NegotiateMessage CreateDefaultNegotiateMessage()
        {
            NegotiateFlags NegotiateFlag = NegotiateFlags.NEGOTIATE_UNICODE
            | NegotiateFlags.REQUEST_TARGET
            | NegotiateFlags.NEGOTIATE_NTLM
            | NegotiateFlags.NEGOTIATE_EXTENDED_SESSIONSECURITY
            | NegotiateFlags.NEGOTIATE_128
            | NegotiateFlags.NEGOTIATE_SIGN
            | NegotiateFlags.NEGOTIATE_SEAL
            | NegotiateFlags.NEGOTIATE_VERSION;

            MetaDataPayloadString DomainNameField = new MetaDataPayloadString();
            MetaDataPayloadString WorkstationField = new MetaDataPayloadString();

            return new NegotiateMessage(NegotiateFlag, DomainNameField, WorkstationField);
        }
    }
}
