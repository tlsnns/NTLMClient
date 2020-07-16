using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Auth
{
    class Auth : SpecialPDU
    {
        byte[] _Pad;
        public byte[] Pad { get => _Pad; set => _Pad = value; }


        public Auth()
        {
            Pad = new byte[4];
            Length = 4;
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(Pad);
            return vs;
        }
    }
}
