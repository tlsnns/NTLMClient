using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Auth
{
    public enum SecurityProviderType : byte
    {
        NONE = 0x00,
        GSS_NEGOTIATE = 0x09,
        WINNT = 0x0A,
        GSS_SCHANNEL = 0x0E,
        GSS_KERBEROS = 0x10,
        NETLOGON = 0x44,
        DEFAULT = 0xFF,
    }
    static class SecurityProviderTypeExtend
    {
        public static byte GetByte(this SecurityProviderType t)
        {
            return (byte)t;
        }
    }
}
