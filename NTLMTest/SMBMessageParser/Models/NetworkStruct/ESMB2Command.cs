using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models
{
    enum ESMB2Command : ushort
    {
        NEGOTIATE = 0,
        SESSION_SETUP = 1,
        LOGOFF = 2,
        TREE_CONNECT = 3,
        TREE_DISCONNECT = 4,
        CREATE = 5,
        CLOSE = 6,
        FLUSH = 7,
        READ = 8,
        WRITE = 9,
        LOCK = 10,
        IOCTL = 11,
        CANCEL = 12,
        ECHO = 13,
        QUERY_DIRECTORY = 14,
        CHANGE_NOTIFY = 15,
        QUERY_INFO = 16,
        SET_INFO = 17,
        OPLOCK_BREAK = 18
    }
    static class ESMB2CommandExtend
    {
        public static byte[] GetBytes(this ESMB2Command t)
        {
            return BitConverter.GetBytes((ushort)t);
        }
    }
}
