using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models
{
    [Flags]
    public enum SMB2HeaderFlags : uint
    {
        None = 0,
        ServerToRedir = 0x00000001,
        AsyncCommand = 0x00000002,
        RelatedOrerations = 0x00000004,
        Signed = 0x00000008,
        PriorityMask = 0x00000070,
        DFSOperations = 0x10000000,
        ReplayOperation = 0x20000000
    }
    static class SMB2HeaderFlagsExtend
    {
        public static bool HasFlag(this SMB2HeaderFlags t, SMB2HeaderFlags sMB2HeaderFlags)
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
        public static byte[] GetBytes(this SMB2HeaderFlags t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
