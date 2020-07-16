using NTLMMessageParse.Models.Challenge;
using Common;
using System.Collections.Generic;

namespace NTLMMessageParse.Models.Authenticate
{
    class NtChallengeResponseFields
    {
        public ushort NtChallengeResponseLen;
        public ushort NtChallengeResponseMaxLen;
        public uint NtChallengeResponseBufferOffset;
        public NTLMV2Response VariableNTLMv2Response;

        public NtChallengeResponseFields(uint payloadPointer, NTLMV2Response variableNTLMV2Response)
        {
            NtChallengeResponseBufferOffset = payloadPointer;
            VariableNTLMv2Response = variableNTLMV2Response;
            var a = (ushort)(variableNTLMV2Response.Length);
            NtChallengeResponseLen = a;
            NtChallengeResponseMaxLen = a;
        }
        public static NtChallengeResponseFields CreateNtChallengeResponseFields(ChallengeMessage challengeMessage, string strUserName, byte[] passwordNTHash, ref uint payloadPointer)
        {
            var np = NTLMV2Response.CreateNTLMV2Response(challengeMessage, strUserName, passwordNTHash);
            var t = new NtChallengeResponseFields(payloadPointer, np);
            payloadPointer += t.NtChallengeResponseLen;
            return t;
        }

        internal IEnumerable<byte> DumpMetaData()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(NtChallengeResponseLen.GetBytes());
            vs.AddRange(NtChallengeResponseMaxLen.GetBytes());
            vs.AddRange(NtChallengeResponseBufferOffset.GetBytes());
            return vs;
        }
        internal IEnumerable<byte> DumpBuffer()
        {
            return VariableNTLMv2Response.DumpBinary();
        }
    }
}
