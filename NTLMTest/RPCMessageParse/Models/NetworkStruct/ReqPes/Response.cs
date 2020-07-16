using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse.Models.NetworkStruct.ReqPes
{
    class Response : SpecialPDU
    {
        uint _AllocHint;
        ushort _ContextID;
        byte _CancelCount;
        byte _Reserved;

        byte[] _Datas;

        public uint AllocHint { get => _AllocHint; private set => _AllocHint = value; }
        public ushort ContextID { get => _ContextID; private set => _ContextID = value; }
        public byte CancelCount { get => _CancelCount; private set => _CancelCount = value; }
        public byte Reserved { get => _Reserved; private set => _Reserved = value; }
        public byte[] StubData { get => _Datas; private set => _Datas = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static Response Parser(byte[] vs, int offset, int specialPDULength)
        {
            return new Response(vs, offset, specialPDULength);
        }
         Response(byte[] vs, int offset, int specialPDULength)
        {
            AllocHint = BitConverter.ToUInt32(vs, offset);//0-4
            ContextID = BitConverter.ToUInt16(vs, offset + 4);//4-2
            CancelCount = vs[offset + 6];//6-1
            Reserved = vs[offset + 7];//7-1
            StubData = new byte[AllocHint];
            Array.Copy(vs, offset + 8, StubData, 0, specialPDULength - 8);
        }
    }
}
