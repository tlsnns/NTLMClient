using NTLMMessageParse;
using RPCMessageParse.Models.NetworkStruct;
using RPCMessageParse.Models.NetworkStruct.Auth;
using RPCMessageParse.Models.NetworkStruct.Bind;
using RPCMessageParse.Models.NetworkStruct.ReqPes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    class RPCWorker
    {
        RpcTransport RpcTransport;
        uint CallId = 2;
        SecurityProviderType SecurityProviderType;
        AuthenticationLevelType AuthenticationLevelType;

        NTLMClient NTLMClient;
        string UserName;
        string Password;

        public RPCWorker(string strIpAddress, int destinationPort, string userName, string password, AuthenticationLevelType authenticationLevelType,
            SecurityProviderType securityProviderType)
        {
            RpcTransport = new RpcTransportOnTcp(strIpAddress, destinationPort);
            SecurityProviderType = securityProviderType;
            AuthenticationLevelType = authenticationLevelType;

            if (SecurityProviderType == SecurityProviderType.WINNT)
            {
                NTLMClient = new NTLMClient();
            }
            UserName = userName;
            Password = password;
        }
        public RPCWorker(string ipAddress, string strNamePipe, string userName, string password)
        {
            RpcTransport = new RpcTransportOnPipe(ipAddress, strNamePipe, userName, password);
            SecurityProviderType = SecurityProviderType.NONE;
            AuthenticationLevelType = AuthenticationLevelType.NONE;
        }
        public GenericPDU Bind(Syntax abstractSyntax, bool has64NDR, bool supportBindTimeFeatureNegotiation)
        {
            ushort contextid = 0;
            List<Context> contexts = new List<Context>();
            contexts.Add(new Context(contextid, abstractSyntax, Syntax.Create32BitNDR()));
            if (has64NDR)
            {
                contextid++;
                contexts.Add(new Context(contextid, abstractSyntax, Syntax.Create64BitNDR()));

            }
            if (supportBindTimeFeatureNegotiation)
            {
                contextid++;
                contexts.Add(
                    new Context(contextid,
                    abstractSyntax,
                    Syntax.CreateBindTimeFeatureNegotiation(BindTimeFeatureNegotiationBitmaskFlags.KeepConnectionOnOrphanSupported | BindTimeFeatureNegotiationBitmaskFlags.SecurityContextMultiplexingSupported)
                    )
                );
            }
            GenericPDU genericPDU = null;
            if (AuthenticationLevelType == AuthenticationLevelType.NONE)
            {
                genericPDU = new GenericPDU(PDUType.bind, CallId, new Bind(contexts));
            }
            else
            {
                genericPDU = new GenericPDU(PDUType.bind, CallId, new Bind(contexts), SecurityProviderType, AuthenticationLevelType, 0, 0, NTLMClient.CreateType1().ToArray());
            }
            RpcTransport.SendDatas(genericPDU.DumpBinary());
            var datas = RpcTransport.ReceiveDatas();
            var gp = GenericPDU.Parser(datas);
            if (gp.PacketType == PDUType.bind_ack)
            {
                if (AuthenticationLevelType != AuthenticationLevelType.NONE)
                {
                    var type3 = NTLMClient.ParseType2AndCreateType3(gp.AuthDatas, UserName, Password);
                    genericPDU = new GenericPDU(PDUType.Auth3, CallId, new Auth(), SecurityProviderType, AuthenticationLevelType, 0, 0, type3.ToArray());
                    RpcTransport.SendDatas(genericPDU.DumpBinary());
                }
            }
            else
            {

            }
            return gp;
        }
        public GenericPDU Request(ushort contextId, ushort opnum, byte[] subdatas)
        {
            CallId++;
            var body = new Request(0, opnum, subdatas);
            GenericPDU genericPDU = new GenericPDU(PDUType.request, CallId, body);
            RpcTransport.SendDatas(genericPDU.DumpBinary());
            var datas = RpcTransport.ReceiveDatas();
            var gp = GenericPDU.Parser(datas);
            return gp;
        }
        public void Close()
        {
            RpcTransport.Close();
        }
    }
}
