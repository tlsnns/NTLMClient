using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace NTLMMessageParse.Models
{
    class AV_PAIR : IBinary
    {
        AvType _AvId;
        ushort _AvLen;
        byte[] _Value;

        public AvType AvId { get => _AvId; }
        public ushort AvLen { get => _AvLen; }
        public byte[] Value { get => _Value; }

        public AV_PAIR(byte[] v, uint uStartIndex)
        {
            int startIndex = (int)uStartIndex;
            _AvId = (AvType)BitConverter.ToUInt16(v, startIndex);

            _AvLen = BitConverter.ToUInt16(v, startIndex + 2);

            _Value = new byte[AvLen];
            Array.Copy(v, startIndex + 4, Value, 0, AvLen);
        }
        public AV_PAIR(AvType avType, byte[] value)
        {
            _AvId = avType;
            _Value = value;
            _AvLen = (ushort)value.Length;
        }
        public void SetValue(byte[] value)
        {
            _Value = value;
            _AvLen = (ushort)value.Length;
        }

        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(((ushort)AvId).GetBytes());
            vs.AddRange(AvLen.GetBytes());
            vs.AddRange(_Value);
            return vs;
        }

        public T Parser<T>(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
    enum AvType : ushort
    {
        MsvAvEOL = 0x0000,
        MsvAvNbComputerName = 0x0001,
        MsvAvNbDomainName = 0x0002,
        MsvAvDnsComputerName = 0x0003,
        MsvAvDnsDomainName = 0x0004,
        MsvAvDnsTreeName = 0x0005,
        MsvAvFlags = 0x0006,
        MsvAvTimestamp = 0x0007,
        MsvAvSingleHost = 0x0008,
        MsvAvTargetName = 0x0009,
        MsvAvChannelBindings = 0x000a
    }
}
