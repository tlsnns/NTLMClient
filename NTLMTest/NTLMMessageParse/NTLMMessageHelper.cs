using NTLMMessageParse.Models.Challenge;
using NTLMMessageParse.Models.Negotiate;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTLMMessageParse
{
    public class NTLMMessageHelper
    {
        static public List<byte> CreateType1()
        {
            NegotiateMessage negotiateMessage = NegotiateMessage.CreateDefaultNegotiateMessage();
            return negotiateMessage.DumpBinary();
        }

        static public List<byte> CreateType3(byte[] Type2data, string userName, string password)
        {
            ChallengeMessage type2 = new ChallengeMessage(Type2data);
            return type2.CreateType3(userName, password).DumpBinary();
        }
    }
}
