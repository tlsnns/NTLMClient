using RPCMessageParse.Models.NetworkStruct.ReqPes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    public class RpcBind
    {
        RPCWorker RPCWorker;
        ushort ContextId;
        internal RpcBind(RPCWorker rpcWorker, ushort contextId)
        {
            RPCWorker = rpcWorker;
            ContextId = contextId;
        }
        public byte[] Request(ushort opnum, byte[] subdatas)
        {
            var gp = RPCWorker.Request(ContextId, opnum, subdatas);
            var r = gp.SpecialPDU as Response;
            return r.StubData;
        }
    }
}
