using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse.Models.NetworkStruct
{
    enum PDUType : byte
    {
        request = 0,
        ping = 1,
        response = 2,
        fault = 3,
        working = 4,
        nocall = 5,
        reject = 6,
        ack = 7,
        cl_cancel = 8,
        fack = 9,
        cancel_ack = 10,
        bind = 11,
        bind_ack = 12,
        bind_nak = 13,
        alter_context = 14,
        alter_context_resp = 15,
        Auth3 = 16,
        shutdown = 17,
        co_cancel = 18,
        orphaned = 19,
    }
    static class PDUTypeExtend
    {
        public static byte GetByte(this PDUType t)
        {
            return (byte)t;
        }
    }
}
