using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse.Models.NetworkStruct.NDR
{
    [Flags]
    enum OrderAndCharsetFlags : byte
    {
        EBCDIC = 0x01,//no set is ascii
        LittleEndian = 0x10,//no set is Bigendian
    }
    static class OrderAndCharsetFlagsExtend
    {
        public static bool HasFlag(this OrderAndCharsetFlags t, OrderAndCharsetFlags sMB2HeaderFlags)
        {
            if (sMB2HeaderFlags == 0)
            {
                throw new ArgumentException("Can't be Zero");
            }
            if ((t & sMB2HeaderFlags) == sMB2HeaderFlags)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte GetByte(this OrderAndCharsetFlags t)
        {
            return (byte)t;
        }
    }
}
