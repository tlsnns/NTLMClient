using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    class ContextResult
    {
        ContextResultType _AckResult;
        ProviderReasonType _ProviderReason;
        Syntax _TransferSyntaxes;

        public ContextResultType AckResult { get => _AckResult; set => _AckResult = value; }
        public ProviderReasonType ProviderReason { get => _ProviderReason; set => _ProviderReason = value; }
        public Syntax TransferSyntaxes { get => _TransferSyntaxes; set => _TransferSyntaxes = value; }

        public static ContextResult Parser(byte[] vs, int offset)
        {
            return new ContextResult(vs, offset);
        }
        ContextResult(byte[] vs, int offset)
        {
            AckResult = (ContextResultType)BitConverter.ToInt16(vs, offset);//0-2
            ProviderReason = (ProviderReasonType)BitConverter.ToInt16(vs, offset + 2);//2-2
            TransferSyntaxes = Syntax.Parser(vs, offset + 4);
        }
    }
}
