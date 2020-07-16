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
    public class StartServiceW : IDL
    {
        static ushort OPNum = 19;

        public StartServiceW(RpcBind rpcBind) : base(rpcBind)
        {
        }
        public int EXEC(byte[] handle)
        {
            var tmp = RPCBind.Request(OPNum, CreateStubData(handle));
            int retCode = ParseReturnStubData(tmp);
            return retCode;
        }
        static byte[] CreateStubData(byte[] handle)
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(handle);
            vs.AddRange(0.GetBytes());
            vs.AddRange(0.GetBytes());
            return vs.ToArray();
        }
        static int ParseReturnStubData(byte[] StubData)
        {
            try
            {
                return BitConverter.ToInt32(StubData, 0);
            }
            catch
            {
                throw new Exception("not't StartServiceW StubData");
            }
        }
    }
}
