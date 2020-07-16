using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    public enum ECreateAction : uint
    {
        FILE_SUPERSEDED = 0,
        FILE_OPENED = 1,
        FILE_CREATED = 2,
        FILE_OVERWRITTEN = 3
    }
    static class ECreateActionExtend
    {
        public static byte[] GetBytes(this ECreateAction t)
        {
            return BitConverter.GetBytes((uint)t);
        }
    }
}
