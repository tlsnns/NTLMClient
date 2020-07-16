using NTLMMessageParse.Models.Challenge;
using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace NTLMMessageParse.Models.Authenticate
{
    class NTLMV2Response : IBinary
    {
        public byte[] NTProofStr;
        public Blob Blob;
        //
        int _Length;
        public int Length
        {
            get => _Length;
        }
        public byte[] UserSessionKey { get; private set; }
        public byte[] RandomSessionKey { get; private set; }
        public NTLMV2Response(byte[] ntProofStr, Blob blob, byte[] userSessionKey)
        {
            if (ntProofStr.Length != 16)
            {
                throw new ArgumentOutOfRangeException("ntProofStr lenght must 16");
            }
            NTProofStr = ntProofStr;
            Blob = blob;
            _Length = ntProofStr.Length + blob.Length;
            UserSessionKey = userSessionKey;

            RandomSessionKey = new byte[16];
            HashUtils.WriteRandomBytes(RandomSessionKey);
        }
        public static NTLMV2Response CreateNTLMV2Response(ChallengeMessage challengeMessage, string strUserName, byte[] passwordNTHash)
        {
            //get ntlmv2Hash
            var ntlmv2Hash = HashUtils.NTLMV2Hash(passwordNTHash, strUserName, challengeMessage.TargetNameField.Buffer);
            //buid Blob
            var tmpinfo = new AV_PAIRs(challengeMessage.TargetInfoField.Buffer, 0, challengeMessage.TargetInfoField.Len);

            //msdn must
            // tmpinfo.AddOrUpdate(AvType.MsvAvFlags, 2u.GetBytes());
            // tmpinfo.AddOrUpdate(AvType.MsvAvChannelBindings, new byte[16]);

            var blob = new Blob(tmpinfo);
            var bBlob = blob.DumpBinary().ToArray();
            //get ntProofStr
            var ntProofStr = HashUtils.GetNTProofStr(challengeMessage.ServerChallenge, bBlob, ntlmv2Hash);

            var userSessionKey = HashUtils.HMACMD5Compute(ntProofStr, ntlmv2Hash);

            return new NTLMV2Response(ntProofStr, blob, userSessionKey);
        }

        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(NTProofStr);
            vs.AddRange(Blob.DumpBinary());
            return vs;
        }

        public T Parser<T>(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
}
