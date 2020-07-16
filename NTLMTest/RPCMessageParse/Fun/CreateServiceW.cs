using Common;
using SMBMessageParser;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse
{
    public class CreateServiceW : IDL
    {
        static ushort OPNum = 12;
        public CreateServiceW(RpcBind rpcBind) : base(rpcBind)
        {

        }
        public int EXEC(byte[] scHandle, string strServerName, string strBinPath, out byte[] handle)
        {
            var tmp = RPCBind.Request(OPNum, CreateStubData(scHandle, strServerName, strBinPath));
            int retCode = ParseReturnStubData(tmp, out handle);
            return retCode;
        }
        static byte[] CreateStubData(
           byte[] handle, string strServerName, string strBinPath)
        {
            string sn;
            if (strServerName.Length % 2 == 0)//4 byte justified
            {
                sn = strServerName + "\0\0";
            }
            else
            {
                sn = strServerName + "\0";
            }
            string bp;
            if (strBinPath.Length % 2 == 0)
            {
                bp = strBinPath + "\0\0";
            }
            else
            {
                bp = strBinPath + "\0";
            }

            List<byte> vs = new List<byte>();
            vs.AddRange(handle);

            vs.AddRange(sn.Length.GetBytes());//service name
            vs.AddRange(0.GetBytes());
            vs.AddRange(sn.Length.GetBytes());
            vs.AddRange(Encoding.Unicode.GetBytes(sn));

            vs.AddRange(0.GetBytes());//display name

            vs.AddRange(0x000f01ff.GetBytes());//access mask
            vs.AddRange(0x00000010u.GetBytes());//service type
            vs.AddRange(3u.GetBytes());//service start type
            vs.AddRange(1u.GetBytes());//error type

            vs.AddRange(bp.Length.GetBytes());//binpath
            vs.AddRange(0.GetBytes());
            vs.AddRange(bp.Length.GetBytes());
            vs.AddRange(Encoding.Unicode.GetBytes(bp));

            vs.AddRange(0.GetBytes());//LoadOrderGroup
            vs.AddRange(0.GetBytes());//TagId

            vs.AddRange(0L.GetBytes());//Dependencies
            vs.AddRange(0.GetBytes());//account
            vs.AddRange(0L.GetBytes());//password
            return vs.ToArray();
        }
        static int ParseReturnStubData(byte[] StubData, out byte[] handle)
        {
            try
            {
                handle = new byte[20];
                Array.Copy(StubData, 4, handle, 0, 20);
                return BitConverter.ToInt32(StubData, 24);
            }
            catch
            {
                throw new Exception("not't CreateServiceWReturn StubData");
            }
        }
    }
}
