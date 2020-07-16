using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.IOCTL
{
    public enum CTLType : uint
    {
        IOCTL = 0,
        FSCTL = 1
    }
    static class CTLTypeExtend
    {
        public static byte[] GetBytes(this CTLType t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
