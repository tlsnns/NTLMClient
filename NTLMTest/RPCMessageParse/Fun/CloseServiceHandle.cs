using SMBMessageParser;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse
{
    public class CloseServiceHandle : IDL
    {
        static ushort OPNum = 0;

        public CloseServiceHandle(RpcBind rpcBind) : base(rpcBind)
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
            return vs.ToArray();
        }
        static int ParseReturnStubData(byte[] StubData)
        {
            try
            {
                return BitConverter.ToInt32(StubData, 20);
            }
            catch
            {
                throw new Exception("not't CloseServiceHandle StubData");
            }
        }
    }
}
