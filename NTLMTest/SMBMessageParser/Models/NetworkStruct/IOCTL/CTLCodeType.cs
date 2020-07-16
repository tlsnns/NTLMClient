using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.IOCTL
{
    public enum CTLCodeType : uint
    {
        FSCTL_PIPE_TRANSCEIVE = 0x0011C017,
    }
    static class CTLCodeTypeExtend
    {
        public static byte[] GetBytes(this CTLCodeType t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
