using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse.Models.NetworkStruct
{
    [Flags]
    enum PDUFlags : byte
    {
        FIRST_FRAG = 0x01,
        LAST_FRAG = 0x02,
        PENDING_CANCEL = 0x04,
        RESERVED = 0x08,
        CONC_MPX = 0x10,
        DID_NOT_EXECUTE = 0x20,
        MAYBE = 0x40,
        OBJECT_UUID = 0x80,
    }
    static class PDUFlagsExtend
    {
        public static bool HasFlag(this PDUFlags t, PDUFlags sMB2HeaderFlags)
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
        public static byte GetByte(this PDUFlags t)
        {
            return (byte)t;
        }
    }
}
