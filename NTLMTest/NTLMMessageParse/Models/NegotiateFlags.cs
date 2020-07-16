using System;
namespace NTLMMessageParse.Models
{
    [Flags]
    public enum NegotiateFlags : uint
    {
        NEGOTIATE_UNICODE = 0x1,
        NEGOTIATE_OEM = 0x2,
        REQUEST_TARGET = 0x4,
        R10 = 0x8,
        NEGOTIATE_SIGN = 0x10,
        NEGOTIATE_SEAL = 0x20,
        NEGOTIATE_DATAGRAM = 0x40,
        NEGOTIATE_LM_KEY = 0x80,
        R9 = 0x100,
        NEGOTIATE_NTLM = 0x200,
        R8 = 0x400,
        NEGOTIATE_ANONYMOUS = 0x800,
        NEGOTIATE_OEM_DOMAIN_SUPPLIED = 0x1000,
        NEGOTIATE_OEM_WORKSTATION_SUPPLIED = 0x2000,
        R7 = 0x4000,
        NEGOTIATE_ALWAYS_SIGN = 0x8000,
        NEGOTIATE_TARGET_TYPE_DOMAIN = 0x10000,
        NEGOTIATE_TARGET_TYPE_SERVER = 0x20000,
        R6 = 0x40000,
        NEGOTIATE_EXTENDED_SESSIONSECURITY = 0x80000,
        NEGOTIATE_IDENTIFY = 0x100000,
        R5 = 0x200000,
        REQUEST_NON_NT_SESSION_KEY = 0x400000,
        NEGOTIATE_TARGET_INFO = 0x800000,
        R4 = 0x1000000,
        NEGOTIATE_VERSION = 0x2000000,
        R3 = 0x4000000,
        R2 = 0x8000000,
        R1 = 0x10000000,
        NEGOTIATE_128 = 0x20000000,
        NEGOTIATE_KEY_EXCH = 0x40000000,
        NEGOTIATE_56 = 0x80000000,
    }
    static class NegotiateFlagsExtend
    {
        public static bool HasFlag(this NegotiateFlags t, NegotiateFlags negotiateFlags)
        {
            if (negotiateFlags == 0)
            {
                throw new ArgumentException("Can't be Zero");
            }
            if ((t & negotiateFlags) == negotiateFlags)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte[] GetBytes(this NegotiateFlags t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
