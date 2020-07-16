using System;
using System.IO;
using RPCMessageParse;
using RPCMessageParse.Models.NetworkStruct.Auth;
using SMBMessageParser;
using SMBMessageParser.Models.NetworkStruct.Create;

namespace NTLMTest
{
    class Program
    {
        static void Main(string[] args)
        {

            string strIpaddress = args[0];
            string strUserName = args[1];
            string strPassword = args[2];
            string command = args[3].ToLower();
            string strServiceName;
            string strRemotePath;
            string strLocalPath;

            RPCClient RPCClient = new RPCClient(strIpaddress, "13521", ProtocolSequenceType.ncacn_ip_tcp,
            AuthenticationLevelType.CONNECT, SecurityProviderType.WINNT, strUserName, strPassword);
            var t = RPCClient.CreateBind(Guid.Parse("551d88b0-b831-4283-a1cd-276559e49f28"), 1, 0, true, true);
            var d = t.Request(1, new byte[] { 2, 0, 0, 0, 1, 0, 0, 0 });
            if (BitConverter.ToInt32(d, 4) == 0)
            {
                Console.WriteLine("结果：" + BitConverter.ToInt32(d, 0));
            }
            else
            {
                Console.WriteLine("失败");
            }
            RPCClient.Close();

            //SC sc;
            //switch (command)
            //{

            //    case "help":
            //        string strHelp =
            //            "[ipaddress] [username] [password] upload [remotePath] strLocalPath" +
            //            "[ipaddress] [username] [password] createservice [servername] [binPath]" +
            //            "[ipaddress] [username] [password] startservice [servername] " +
            //            "[ipaddress] [username] [password] exec";
            //        Console.WriteLine(strHelp);
            //        break;

            //    case "upload":
            //        strRemotePath = args[4];
            //        strLocalPath = args[5];
            //        SMB2Client smb2Client = new SMB2Client(strIpaddress, strUserName, strPassword);
            //        smb2Client.Login();
            //        var dt = smb2Client.CreateTree(strRemotePath[0] + "$");
            //        var dF = dt.CreateFile(FilePipePrinterAccessMaskFlags.GENERIC_READ | FilePipePrinterAccessMaskFlags.GENERIC_WRITE, ShareAccessFlags.None,
            //                 ECreateDisposition.FILE_OVERWRITE_IF, 0x44, strRemotePath.Substring(3));
            //        dF.Write(0, File.ReadAllBytes(strLocalPath));
            //        dF.Close();
            //        dt.Disconnect();
            //        smb2Client.Logout();
            //        break;
            //    case "create":
            //        sc = new SC(strIpaddress, strUserName, strPassword);
            //        strServiceName = args[4];
            //        strRemotePath = args[5];
            //        sc.CreateService(strServiceName, strRemotePath);
            //        break;
            //    case "start":
            //        sc = new SC(strIpaddress, strUserName, strPassword);
            //        strServiceName = args[4];
            //        sc.StartService(strServiceName);
            //        break;
            //    case "exec":
            //        new Exec(strIpaddress, strUserName, strPassword).Start();
            //        break;
            //    default: Console.WriteLine("unknow command"); break;
            //}
            Console.Read();
        }
    }
}
