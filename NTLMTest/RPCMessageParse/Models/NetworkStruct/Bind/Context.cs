using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    class Context : IBinary
    {
        ushort _ContextId;
        byte _TransferCount;     /* number of items */
        byte _Reserved;           /* alignment pad, m.b.z. */
        Syntax _AbstractSyntax;    /* transfer syntax list */
        List<Syntax> _TransferSyntaxes;

        public ushort ContextId { get => _ContextId; private set => _ContextId = value; }
        public byte TransferCount { get => _TransferCount; private set => _TransferCount = value; }
        public byte Reserved { get => _Reserved; private set => _Reserved = value; }
        public Syntax AbstractSyntax { get => _AbstractSyntax; private set => _AbstractSyntax = value; }
        public List<Syntax> TransferSyntaxes { get => _TransferSyntaxes; private set => _TransferSyntaxes = value; }
        List<byte> Datas;
        public Context(ushort contextId, Syntax abstractSyntax, params Syntax[] args)
        {
            ContextId = contextId;
            TransferCount = (byte)args.Length;
            Reserved = 0;
            AbstractSyntax = abstractSyntax;
            TransferSyntaxes = new List<Syntax>();
            TransferSyntaxes.AddRange(args);

            Datas = new List<byte>();
            TransferSyntaxes.ForEach(item =>
            {
                Datas.AddRange(item.DumpBinary());
            });
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(ContextId.GetBytes());
            vs.Add(TransferCount);
            vs.Add(Reserved);
            vs.AddRange(AbstractSyntax.DumpBinary());
            vs.AddRange(Datas);
            return vs;
        }
    }
}
