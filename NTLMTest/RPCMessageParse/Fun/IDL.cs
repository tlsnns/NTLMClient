using SMBMessageParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    public abstract class IDL
    {
        protected RpcBind RPCBind { get; set; }
        public IDL(RpcBind rpcBind)
        {
            RPCBind = rpcBind;
        }
    }
}
