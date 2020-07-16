using RPCMessageParse;
using RPCMessageParse.Models.NetworkStruct.Auth;
using SMBMessageParser;
using SMBMessageParser.Models;
using SMBMessageParser.Models.NetworkStruct.Create;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTLMTest
{
    class SC
    {
        string IpAddress;
        RPCClient RPCClient;
        RpcBind RPCBind;
        byte[] SCHandle;
        public SC(string ipAddress, string userName, string password)
        {
            IpAddress = ipAddress;
            RPCClient = new RPCClient(ipAddress, "svcctl", ProtocolSequenceType.ncacn_np,
              AuthenticationLevelType.DEFAULT, SecurityProviderType.NONE, userName, password);
        }
        void Close(byte[] serHandle)
        {
            var retCode = new CloseServiceHandle(RPCBind).EXEC(serHandle);
            retCode = new CloseServiceHandle(RPCBind).EXEC(SCHandle);
            RPCClient.Close();
        }
        void ServiceInit()
        {
            RPCBind = RPCClient.CreateBind(Guid.Parse("367abb81-9844-35f1-ad32-98f038001003"), 2, 0, false, true);
            //OpenSCManagerW
            int retCode = new OpenSCManagerW(RPCBind).EXEC(IpAddress, out byte[] scHandle);
            if (retCode == 0)
            {
                SCHandle = scHandle;
            }
            else
            {
                throw new Exception("OpenSCManagerW error " + retCode);
            }
        }

        public void CreateService(string serverName, string binPath)
        {
            ServiceInit();
            var retCode = new CreateServiceW(RPCBind).EXEC(SCHandle, serverName, binPath, out byte[] serHandle);
            if (retCode != 0)
            {
                throw new Exception("CreateServiceW error " + retCode);
            }
            Close(serHandle);
        }
        public void StartService(string serverName)
        {
            ServiceInit();
            var retCode = new OpenServiceW(RPCBind).EXEC(SCHandle, serverName, out byte[] serHandle);
            if (retCode != 0)
            {
                throw new Exception("OpenServiceW error " + retCode);
            }
            retCode = new StartServiceW(RPCBind).EXEC(serHandle);
            if (retCode != 0)
            {
                throw new Exception("StartServiceW error " + retCode);
            }
            Close(serHandle);
        }

    }
}