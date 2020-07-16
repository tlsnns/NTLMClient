using System;
using System.Collections.Generic;
using System.Text;
using Common;
using NTLMMessageParse.Models;
using NTLMMessageParse.Models.Challenge;

namespace NTLMMessageParse.Models.Authenticate
{
    class AuthenticateMessage : IBinary
    {
        public readonly byte[] Signature = Encoding.ASCII.GetBytes("NTLMSSP\0");//Fixed 8-byte signature
        public readonly uint MessageType = 0x3;//Fixed type message 3
        public MetaDataPayloadHex LmChallengeResponseField;
        public NtChallengeResponseFields NtChallengeResponseField;
        public MetaDataPayloadString DomainNameField;
        public MetaDataPayloadString UserNameField;
        public MetaDataPayloadString WorkstationField;
        public MetaDataPayloadString EncryptedRandomSessionKeyField;
        public NegotiateFlags NegotiateFlag;
        public Versions Version;

        public AuthenticateMessage(
            MetaDataPayloadHex lmChallengeResponseField,
            NtChallengeResponseFields ntChallengeResponseField,
            MetaDataPayloadString domainNameField,
            MetaDataPayloadString userNameField,
            MetaDataPayloadString workstationField,
            MetaDataPayloadString encryptedRandomSessionKeyField,
            NegotiateFlags negotiateFlag,
            Versions version
            )
        {
            LmChallengeResponseField = lmChallengeResponseField;
            NtChallengeResponseField = ntChallengeResponseField;
            DomainNameField = domainNameField;
            UserNameField = userNameField;
            WorkstationField = workstationField;
            EncryptedRandomSessionKeyField = encryptedRandomSessionKeyField;
            NegotiateFlag = negotiateFlag;
            Version = version;
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(Signature);
            vs.AddRange(MessageType.GetBytes());
            vs.AddRange(LmChallengeResponseField.DumpMetaData());
            vs.AddRange(NtChallengeResponseField.DumpMetaData());
            vs.AddRange(DomainNameField.DumpMetaData());
            vs.AddRange(UserNameField.DumpMetaData());
            vs.AddRange(WorkstationField.DumpMetaData());
            vs.AddRange(EncryptedRandomSessionKeyField.DumpMetaData());
            vs.AddRange(((uint)NegotiateFlag).GetBytes());
            vs.AddRange(Version.DumpBinary());

            vs.AddRange(LmChallengeResponseField.DumpBuffer());
            vs.AddRange(NtChallengeResponseField.DumpBuffer());
            vs.AddRange(DomainNameField.DumpBuffer());
            vs.AddRange(UserNameField.DumpBuffer());
            vs.AddRange(WorkstationField.DumpBuffer());
            vs.AddRange(EncryptedRandomSessionKeyField.DumpBuffer());

            return vs;

        }
        static public AuthenticateMessage CreateAuthenticateMessage(ChallengeMessage challengeMessage, string strUserName, byte[] passwordNTHash)
        {
            uint payloadPointer = 88 - 16;
            MetaDataPayloadHex lcrf = new MetaDataPayloadHex(payloadPointer, new byte[24]);
            payloadPointer += lcrf.Len;

            NtChallengeResponseFields ncrf = NtChallengeResponseFields.CreateNtChallengeResponseFields(challengeMessage, strUserName, passwordNTHash, ref payloadPointer);

            MetaDataPayloadString domainNameField = new MetaDataPayloadString(payloadPointer, challengeMessage.TargetNameField.Buffer);
            payloadPointer += domainNameField.Len;

            var userNames = Encoding.Unicode.GetBytes(strUserName);
            var len = (ushort)userNames.Length;
            MetaDataPayloadString userNameField = new MetaDataPayloadString(payloadPointer, userNames);
            payloadPointer += userNameField.Len;

            MetaDataPayloadString workstationField = new MetaDataPayloadString(payloadPointer, challengeMessage.TargetNameField.Buffer);
            payloadPointer += workstationField.Len;

            byte[] t = new byte[16];
            if (challengeMessage.NegotiateFlag.HasFlag(NegotiateFlags.NEGOTIATE_KEY_EXCH))
            {
                // EncryptedRandomSessionKey=MasterKey RC4-encrypts SecondaryMasterKey
                throw new NotImplementedException();
            }
            MetaDataPayloadString EncryptedRandomSessionKeyField = new MetaDataPayloadString(payloadPointer, t);
            payloadPointer += EncryptedRandomSessionKeyField.Len;

            return new AuthenticateMessage(lcrf, ncrf, domainNameField, userNameField, workstationField,
                EncryptedRandomSessionKeyField,
                challengeMessage.NegotiateFlag,
                new Versions()
                );
        }

        public T Parser<T>(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
}
