using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    enum BindTimeFeatureNegotiationBitmaskFlags : ulong
    {
        SecurityContextMultiplexingSupported = 1,
        KeepConnectionOnOrphanSupported = 2,
    }
    static class BindTimeFeatureNegotiationBitmaskFlagsExtend
    {
        public static bool HasFlag1(this BindTimeFeatureNegotiationBitmaskFlags t, BindTimeFeatureNegotiationBitmaskFlags sMB2HeaderFlags)
        {
            if (sMB2HeaderFlags == 0)
            {
                throw new ArgumentException("Can't be Zero");
            }
            if ((t & sMB2HeaderFlags) == sMB2HeaderFlags)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte[] GetByte(this BindTimeFeatureNegotiationBitmaskFlags t)
        {
            return BitConverter.GetBytes((ulong)t);
        }
    }
}
