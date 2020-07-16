using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    public enum ECreateDisposition : uint
    {
        FILE_SUPERSEDE = 0,
        FILE_OPEN = 1,
        FILE_CREATE = 2,
        FILE_OPEN_IF = 3,
        FILE_OVERWRITE = 4,
        FILE_OVERWRITE_IF = 5
    }
    static class ECreateDispositionExtend
    {
        public static byte[] GetBytes(this ECreateDisposition t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
