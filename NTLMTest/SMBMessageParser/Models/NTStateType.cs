using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models
{
    enum NTStateType : uint
    {
        Success = 0,
        MoreProcessingRequired = 0xc0000016,
        Pending = 0x00000103,
        LogonFailure = 0xc000006d,
        PIPE_DISCONNECTED = 0xc00000b0
    }
    static class NTStateTypeExtend
    {
        public static byte[] GetBytes(this NTStateType t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
