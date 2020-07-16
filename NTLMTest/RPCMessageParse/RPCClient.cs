using RPCMessageParse.Models.NetworkStruct.Auth;
using RPCMessageParse.Models.NetworkStruct.Bind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    public class RPCClient
    {
        RPCWorker RPCWorker;
        public RPCClient(string ipAddress, string strEndPoint, ProtocolSequenceType protocolSequence,
            AuthenticationLevelType authenticationLevelType,
            SecurityProviderType securityProviderType,
            string userName, string password)
        {
            if (protocolSequence == ProtocolSequenceType.ncacn_np)
            {
                RPCWorker = new RPCWorker(ipAddress, strEndPoint, userName, password);
            }
            else if (protocolSequence == ProtocolSequenceType.ncacn_ip_tcp)
            {
                RPCWorker = new RPCWorker(ipAddress, int.Parse(strEndPoint), userName, password, authenticationLevelType, securityProviderType);
            }
            else
            {
                throw new Exception("no Support ProtocolSequenceType");
            }
            if (authenticationLevelType != AuthenticationLevelType.NONE && authenticationLevelType != AuthenticationLevelType.CONNECT && authenticationLevelType != AuthenticationLevelType.PKT)
            {
                throw new Exception("no Support authenticationLevelType");
            }
            if (securityProviderType != SecurityProviderType.NONE && securityProviderType != SecurityProviderType.WINNT)
            {
                throw new Exception("no Support SecurityProviderType");
            }
        }

        public RpcBind CreateBind(Guid uuid, ushort majorVersion, ushort minorVersion,
            bool has64NDR, bool supportBindTimeFeatureNegotiation)
        {

            var a = RPCWorker.Bind(
                  new Syntax(uuid, majorVersion, minorVersion),
                  has64NDR,
                  supportBindTimeFeatureNegotiation
             );
            var b = a.SpecialPDU as BindACK;
            var c = b.ContextResults.FindIndex(item => item.AckResult == ContextResultType.Acceptance);
            return new RpcBind(RPCWorker, (ushort)c);
        }
        public void Close()
        {
            RPCWorker.Close();
        }
    }
}
