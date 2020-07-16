using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    [Flags]
    public enum ShareAccessFlags : uint
    {
        None = 0,
        ShareRead = 0x00000001,
        ShareWrite = 0x00000002,
        ShareDelete = 0x00000004,
    }
    static class ShareAccessFlagsExtend
    {
        public static bool HasFlag(this ShareAccessFlags t, ShareAccessFlags sMB2HeaderFlags)
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
        public static byte[] GetBytes(this ShareAccessFlags t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
