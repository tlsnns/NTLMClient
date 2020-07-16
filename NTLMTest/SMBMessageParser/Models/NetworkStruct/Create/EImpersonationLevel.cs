using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    enum EImpersonationLevel : uint
    {
        Anonymous = 0,
        Identification = 1,
        Impersonation = 2,
        Delegate = 3
    }
    static class EImpersonationLevelExtend
    {
        public static byte[] GetBytes(this EImpersonationLevel t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
