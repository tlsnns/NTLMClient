using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    enum ContextResultType : short
    {
        Acceptance = 0,
        UserRejection = 1,
        ProviderRejection = 2,
        NegotiateACK = 3
    }
}
