using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBMessageParser.Models.NetworkStruct
{
    class LogoffAndTreeDisconnect : SMB2Body
    {
        readonly ushort _StructureSize = 4;
        ushort _Reserved;

        public ushort StructureSize { get => _StructureSize; }
        public ushort Reserved { get => _Reserved; private set => _Reserved = value; }
        public LogoffAndTreeDisconnect()
        {
        }
        public override List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(StructureSize.GetBytes());
            vs.AddRange(Reserved.GetBytes());
            return vs;
        }
        public static LogoffAndTreeDisconnect Parser(byte[] vs, int offset)
        {
            return new LogoffAndTreeDisconnect(vs, offset);
        }
        LogoffAndTreeDisconnect(byte[] vs, int offset)
        {
            if (StructureSize != BitConverter.ToUInt16(vs, offset))//0-2
            {
                throw new Exception("StructureSize is not 4");
            }
            Reserved = BitConverter.ToUInt16(vs, offset + 2);//0-2
        }
    }
}
