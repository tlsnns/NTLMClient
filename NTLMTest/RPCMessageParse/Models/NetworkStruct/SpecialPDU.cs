using System;
using System.Collections.Generic;
using System.Text;
using Common;
using RPCMessageParse.Models.NetworkStruct.Bind;
using RPCMessageParse.Models.NetworkStruct.ReqPes;

namespace RPCMessageParse.Models.NetworkStruct
{
    abstract class SpecialPDU : IBinary
    {
        public int Length { get; protected set; }

        public abstract List<byte> DumpBinary();
    }
    class PDUBodyFactory
    {
        public static SpecialPDU CreatePDUBody(byte[] vs, int offset, PDUType pduType, int specialPDULength)
        {
            SpecialPDU pduBody = null;
            switch (pduType)
            {
                case PDUType.response: pduBody = Response.Parser(vs, offset, specialPDULength); break;
                case PDUType.bind_ack: pduBody = BindACK.Parser(vs, offset, specialPDULength); break;
                default: throw new NotImplementedException("unknow pdu type");
            }

            return pduBody;
        }
    }
}
