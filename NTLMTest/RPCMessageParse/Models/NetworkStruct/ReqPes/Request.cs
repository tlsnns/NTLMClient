using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse.Models.NetworkStruct.ReqPes
{
    class Request : SpecialPDU
    {
        uint _AllocHint;
        ushort _ContextID;
        ushort _OPnum;
        byte[] _Datas;

        public uint AllocHint { get => _AllocHint; private set => _AllocHint = value; }
        public ushort ContextID { get => _ContextID; private set => _ContextID = value; }
        public ushort OPnum { get => _OPnum; private set => _OPnum = value; }
        public byte[] Datas { get => _Datas; private set => _Datas = value; }

        public Request(ushort contextID, ushort opnum, byte[] datas)
        {
            AllocHint = (uint)datas.Length;
            ContextID = contextID;
            OPnum = opnum;
            Datas = datas;
            Length = 8 + datas.Length;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(AllocHint.GetBytes());
            vs.AddRange(ContextID.GetBytes());
            vs.AddRange(OPnum.GetBytes());
            vs.AddRange(Datas);
            return vs;
        }
    }
}
