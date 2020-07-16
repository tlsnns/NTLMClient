using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace RPCMessageParse.Models.NetworkStruct.NDR
{
    class DataRepresentationFormatLabel : IBinary
    {
        OrderAndCharsetFlags _Flags;
        FloatingPointType _Type;
        byte _Reserved;
        byte _Reserved2;
        public OrderAndCharsetFlags Flags { get => _Flags; private set => _Flags = value; }
        public FloatingPointType Type { get => _Type; private set => _Type = value; }
        public byte Reserved { get => _Reserved; private set => _Reserved = value; }
        public byte Reserved2 { get => _Reserved2; private set => _Reserved2 = value; }

        public DataRepresentationFormatLabel(OrderAndCharsetFlags flags, FloatingPointType type)
        {
            Flags = flags;
            Type = type;
            Reserved = 0;
            Reserved2 = 0;
        }
        public DataRepresentationFormatLabel() : this(OrderAndCharsetFlags.LittleEndian, FloatingPointType.IEEE)
        {

        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.Add(Flags.GetByte());
            vs.Add(Type.GetByte());
            vs.Add(Reserved);
            vs.Add(Reserved2);
            return vs;
        }
        public static DataRepresentationFormatLabel Parser(byte[] vs, int offset)
        {
            return new DataRepresentationFormatLabel(vs, offset);
        }
        DataRepresentationFormatLabel(byte[] vs, int offset)
        {
            Flags = (OrderAndCharsetFlags)vs[offset];//0-1
            Type = (FloatingPointType)vs[offset + 1];//1-1
            Reserved = vs[offset + 2];//2-1
            Reserved2 = vs[offset + 3];//3-1
        }
    }
}
