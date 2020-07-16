using System;
using System.Collections.Generic;

namespace NTLMMessageParse.Models
{
    class MetaDataVariableTargetInfos : MetaDataPayload
    {
        public MetaDataVariableTargetInfos(byte[] v, int startIndex)
        {
            Len = BitConverter.ToUInt16(v, startIndex);
            MaxLen = BitConverter.ToUInt16(v, startIndex + 2);
            BufferOffset = BitConverter.ToUInt32(v, startIndex + 4);

            Buffer = new byte[Len];
            Array.Copy(v, BufferOffset, Buffer, 0, Len);
            TargetInfos = new AV_PAIRs(v, BufferOffset, Len);

        }
        public AV_PAIRs TargetInfos;

        public override List<byte> DumpMetaData()
        {
            throw new NotImplementedException();
        }

        public override List<byte> DumpBuffer()
        {
            throw new NotImplementedException();
        }
    }
}
