using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace NTLMMessageParse.Models
{
    class MetaDataPayloadString : MetaDataPayload
    {
        string _StringValue;

        public string StringValue { get => _StringValue; private set => _StringValue = value; }

        public MetaDataPayloadString()
        {
            Len = 0;
            MaxLen = 0;
            BufferOffset = 0;
            Buffer = new byte[0];
        }
        public MetaDataPayloadString(uint bufferOffset, byte[] buffer)
        {
            ushort len = (ushort)buffer.Length;
            Len = len;
            MaxLen = len;
            BufferOffset = bufferOffset;
            Buffer = buffer;
        }
        public MetaDataPayloadString(byte[] v, int startIndex)
        {
            Len = BitConverter.ToUInt16(v, startIndex);
            MaxLen = BitConverter.ToUInt16(v, startIndex + 2);
            BufferOffset = BitConverter.ToUInt32(v, startIndex + 4);

            Buffer = new byte[Len];
            Array.Copy(v, BufferOffset, Buffer, 0, Len);
        }
        public override List<byte> DumpMetaData()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(Len.GetBytes());
            vs.AddRange(MaxLen.GetBytes());
            vs.AddRange(BufferOffset.GetBytes());
            return vs;
        }

        public override List<byte> DumpBuffer()
        {
            List<byte> vs = new List<byte>(Buffer);
            return vs;
        }

        public string ToString(Encoding encoding)
        {
            if (StringValue == null)
            {
                StringValue = encoding.GetString(Buffer);
            }
            return StringValue;
        }
    }
}
