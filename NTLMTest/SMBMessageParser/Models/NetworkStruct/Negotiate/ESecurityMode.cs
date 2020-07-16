using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.Negotiate
{
    enum ESecurityMode : ushort
    {
        SMB2_NEGOTIATE_SIGNING_ENABLED = 1,
        SMB2_NEGOTIATE_SIGNING_REQUIRED = 2
    }
    static class ESecurityModeExtend
    {
        public static byte[] GetBytes(this ESecurityMode t)
        {
            return BitConverter.GetBytes((ushort)t);
        }
    }
}
