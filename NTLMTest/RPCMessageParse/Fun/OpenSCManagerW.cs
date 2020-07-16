using Common;
using SMBMessageParser;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCMessageParse
{
    public class OpenSCManagerW : IDL
    {
        public static ushort OPNum = 15;
        static uint ReferentID = 0x00020000;

        public OpenSCManagerW(RpcBind rpcBind) : base(rpcBind)
        {


        }
        public int EXEC(string strMachineName, out byte[] handle)
        {
            var tmp = RPCBind.Request(OPNum, CreateStubData(strMachineName));
            int retCode = ParseReturnStubData(tmp, out handle);
            return retCode;
        }
        public static byte[] CreateStubData(string strMachineName)
        {
            string t = @"\\" + strMachineName + "\0";
            List<byte> vs = new List<byte>();
            vs.AddRange(ReferentID.GetBytes());
            vs.AddRange(t.Length.GetBytes());
            vs.AddRange(0.GetBytes());
            vs.AddRange(t.Length.GetBytes());
            vs.AddRange(Encoding.Unicode.GetBytes(t));
            vs.AddRange(0.GetBytes());
            vs.AddRange(0x3.GetBytes());
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
                throw new Exception("not't OpenSCManagerWReturn StubData");
            }
        }
    }
}
