using Common;
using SMBMessageParser;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    public class OpenServiceW : IDL
    {
        static ushort OPNum = 16;
        public OpenServiceW(RpcBind rpcBind) : base(rpcBind)
        {

        }
        public int EXEC(byte[] scHandle, string strServerName, out byte[] handle)
        {
            var tmp = RPCBind.Request(OPNum, CreateStubData(scHandle, strServerName));
            int retCode = ParseReturnStubData(tmp, out handle);
            return retCode;
        }
        static byte[] CreateStubData(byte[] handle, string strServerName)
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
            List<byte> vs = new List<byte>();
            vs.AddRange(handle);

            vs.AddRange(sn.Length.GetBytes());//service name
            vs.AddRange(0.GetBytes());
            vs.AddRange(sn.Length.GetBytes());
            vs.AddRange(Encoding.Unicode.GetBytes(sn));

            vs.AddRange(0x000f01ff.GetBytes());//access mask
            return vs.ToArray();
        }
        static int ParseReturnStubData(byte[] StubData, out byte[] handle)
        {
            try
            {
                handle = new byte[20];
                Array.Copy(StubData, 0, handle, 0, 20);
                return BitConverter.ToInt32(StubData, 20);
            }
            catch
            {
                throw new Exception("not't OpenServiceWReturn StubData");
            }
        }
    }
}
