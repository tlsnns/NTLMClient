using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    enum ProviderReasonType : short
    {
        ReasonNotSpecified = 0,
        AbstractSyntaxNotSupported = 1,
        ProposedTransferSyntaxesNotSupported = 2,
        LocalLimitExceeded = 3
    }
}
