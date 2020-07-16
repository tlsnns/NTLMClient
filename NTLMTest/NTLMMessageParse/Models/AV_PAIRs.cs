using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace NTLMMessageParse.Models
{
    class AV_PAIRs : IBinary
    {
        List<AV_PAIR> TargetInfos;
        public ushort Length { get; private set; }
        public AV_PAIRs(byte[] v, uint startIndex, ushort len)
        {
            TargetInfos = new List<AV_PAIR>();
            Length = len;
            uint i = startIndex;
            uint l = len + i;
            while (i < l)
            {
                var t = new AV_PAIR(v, i);
                TargetInfos.Add(t);
                i = i + 4 + t.AvLen;
            }
        }
        public void AddOrUpdate(AvType avId, byte[] value)
        {
            var item = TargetInfos.Find(c =>
            {
                return c.AvId == avId ? true : false;

            });
            if (item == null)
            {
                TargetInfos.Insert(TargetInfos.Count - 1, new AV_PAIR(avId, value));
            }
            else
            {
                item.SetValue(value);
            }
            Length += (ushort)value.Length;
        }
        public bool TryGetTimestamp(out long timestamp)
        {
            var item = TargetInfos.Find(c =>
            {
                return c.AvId == AvType.MsvAvTimestamp ? true : false;

            });
            if (item == null)
            {
                timestamp = 0;
                return false;
            }
            else
            {
                timestamp = BitConverter.ToInt64(item.Value, 0);
                return true;
            }
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            TargetInfos.ForEach(i =>
            {
                vs.AddRange(i.DumpBinary());
            });
            return vs;
        }

        public T Parser<T>(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
}
