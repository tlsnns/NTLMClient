using System;
using System.Collections.Generic;
using System.Text;
using NTLMMessageParse.Models.Authenticate;
using Common;

namespace NTLMMessageParse.Models.Challenge
{
    class ChallengeMessage
    {
        public readonly byte[] Signature = Encoding.ASCII.GetBytes("NTLMSSP\0");//Fixed 8-byte signature
        public readonly uint MessageType = 0x2;//Fixed type message 2
        public MetaDataPayloadString TargetNameField;
        public NegotiateFlags NegotiateFlag;
        public byte[] ServerChallenge = new byte[8];//8 byte
        public readonly byte[] Reserved = new byte[8];//8 byte
        public MetaDataVariableTargetInfos TargetInfoField;
        public Versions Version;

        public ChallengeMessage(byte[] cm)
        {
            if (cm.Length < 56)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (!Signature.CompareArray(cm, 0, 8))//0-8
            {
                throw new Exception("ChallengeMessage Signature Error");
            }
            if (MessageType != BitConverter.ToUInt32(cm, 8))//8-4
            {
                throw new Exception("ChallengeMessage MessageType Error");
            }
            TargetNameField = new MetaDataPayloadString(cm, 12);//12-8
            NegotiateFlag = (NegotiateFlags)BitConverter.ToUInt32(cm, 20);//20-4
            Array.Copy(cm, 24, ServerChallenge, 0, 8); //24-8
            if (!Reserved.CompareArray(cm, 32, 8))//32-8
            {
                throw new Exception("ChallengeMessage Reserved Error");
            }
            TargetInfoField = new MetaDataVariableTargetInfos(cm, 40);//40-8

            Version = new Versions(cm, 48);//48-8
        }
    }
    static class ChallengeMessageExtend
    {
        static public AuthenticateMessage CreateType3(this ChallengeMessage challengeMessage, string strUserName, string strPassword)
        {
            //PasswordHash
            var passwordNTHash = HashUtils.PasswordNTHash(strPassword);
            return challengeMessage.CreateType3(strUserName, passwordNTHash);
        }
        static public AuthenticateMessage CreateType3(this ChallengeMessage challengeMessage, string strUserName, byte[] passwordNTHash)
        {
            return AuthenticateMessage.CreateAuthenticateMessage(challengeMessage, strUserName, passwordNTHash);
        }
    }
}
