using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    class Bind : SpecialPDU
    {
        ushort _MaxXmitFrag;
        ushort _MaxRecvFrag;
        uint _AssocGroupId;
        byte _ContextCount;
        byte _Reserved;
        ushort _Reserved2;
        List<Context> _Contexts;

        public ushort MaxXmitFrag { get => _MaxXmitFrag; private set => _MaxXmitFrag = value; }
        public ushort MaxRecvFrag { get => _MaxRecvFrag; private set => _MaxRecvFrag = value; }
        public uint AssocGroupId { get => _AssocGroupId; private set => _AssocGroupId = value; }
        public byte ContextCount { get => _ContextCount; private set => _ContextCount = value; }
        public byte Reserved { get => _Reserved; private set => _Reserved = value; }
        public ushort Reserved2 { get => _Reserved2; private set => _Reserved2 = value; }
        public List<Context> Contexts { get => _Contexts; private set => _Contexts = value; }
        List<byte> Datas;

        public Bind(List<Context> contexts)
        {
            MaxXmitFrag = 4096;
            MaxRecvFrag = 4096;
            AssocGroupId = 0;
            ContextCount = (byte)contexts.Count;
            Reserved = 0;
            Reserved2 = 0;
            Contexts = contexts;

            Datas = new List<byte>();
            Contexts.ForEach(item =>
            {
                Datas.AddRange(item.DumpBinary());
            });
            Length = 12 + Datas.Count;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(MaxXmitFrag.GetBytes());
            vs.AddRange(MaxRecvFrag.GetBytes());
            vs.AddRange(AssocGroupId.GetBytes());
            vs.Add(ContextCount);
            vs.Add(Reserved);
            vs.AddRange(Reserved2.GetBytes());
            vs.AddRange(Datas);
            return vs;
        }
    }
}
