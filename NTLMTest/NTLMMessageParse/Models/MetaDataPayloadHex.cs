using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace NTLMMessageParse.Models
{
    class MetaDataPayloadHex : MetaDataPayload
    {
        string _HexValue;

        public string HexValue { get => _HexValue; private set => _HexValue = value; }

        public MetaDataPayloadHex(byte[] v, int startIndex)
        {
            Len = BitConverter.ToUInt16(v, startIndex);
            MaxLen = BitConverter.ToUInt16(v, startIndex + 2);
            BufferOffset = BitConverter.ToUInt32(v, startIndex + 4);

            Buffer = new byte[Len];
            Array.Copy(v, BufferOffset, Buffer, 0, Len);

            SetHexValue();
        }
        public MetaDataPayloadHex(uint bufferOffset, byte[] buffer)
        {
            ushort len = (ushort)buffer.Length;
            Len = len;
            MaxLen = len;
            BufferOffset = bufferOffset;
            Buffer = buffer;
            SetHexValue();
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

        void SetHexValue()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Buffer.Length; i++)
            {
                sb.Append(Buffer[i].ToString("X2"));
            }
            HexValue = sb.ToString();
        }
    }
}
