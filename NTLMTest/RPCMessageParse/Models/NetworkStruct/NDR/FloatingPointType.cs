using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse.Models.NetworkStruct.NDR
{
    enum FloatingPointType : byte
    {
        IEEE = 0,
        VAX = 1,
        Cray = 2,
        IBM = 3
    }
    static class FloatingPointTypeExtend
    {
        public static byte GetByte(this FloatingPointType t)
        {
            return (byte)t;
        }
    }
}
