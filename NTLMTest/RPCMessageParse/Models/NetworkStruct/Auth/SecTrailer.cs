using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Auth
{
    class SecTrailer : IBinary
    {
        SecurityProviderType _AuthType;
        AuthenticationLevelType _AuthLevel;
        byte _AuthPadLength;
        byte _AuthReserved;
        uint _AuthContextId;

        public SecurityProviderType AuthType { get => _AuthType; private set => _AuthType = value; }
        public AuthenticationLevelType AuthLevel { get => _AuthLevel; private set => _AuthLevel = value; }
        public byte AuthPadLength { get => _AuthPadLength; private set => _AuthPadLength = value; }
        public byte AuthReserved { get => _AuthReserved; private set => _AuthReserved = value; }
        public uint AuthContextId { get => _AuthContextId; private set => _AuthContextId = value; }

        public SecTrailer(SecurityProviderType authType, AuthenticationLevelType authLevel, byte authPadLength, uint authContextId)
        {
            AuthType = authType;
            AuthLevel = authLevel;
            AuthPadLength = authPadLength;
            AuthReserved = 0;
            AuthContextId = authContextId;
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.Add(AuthType.GetByte());
            vs.Add(AuthLevel.GetByte());
            vs.Add(AuthPadLength);
            vs.Add(AuthReserved);
            vs.AddRange(AuthContextId.GetBytes());
            return vs;
        }
        public static SecTrailer Parser(byte[] vs, int offset)
        {
            return new SecTrailer(vs, offset);
        }
        SecTrailer(byte[] vs, int offset)
        {
            AuthType = (SecurityProviderType)vs[offset];//0-1
            AuthLevel = (AuthenticationLevelType)vs[offset + 1];//1-1
            AuthPadLength = vs[offset + 2];//2-1
            AuthReserved = vs[offset + 3];//3-1
            AuthContextId = BitConverter.ToUInt32(vs, offset + 4);//4-4
        }
    }
}
