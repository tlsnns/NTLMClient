using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Auth
{
    public enum AuthenticationLevelType : byte
    {
        DEFAULT = 0x00,
        NONE = 0x01,
        CONNECT = 0x02,
        CALL = 0x03,
        PKT = 0x04,
        PKT_INTEGRITY = 0x05,
        PKT_PRIVACY = 0x06,
    }
    static class AuthenticationLevelTypeExtend
    {
        public static byte GetByte(this AuthenticationLevelType t)
        {
            return (byte)t;
        }
    }
}
