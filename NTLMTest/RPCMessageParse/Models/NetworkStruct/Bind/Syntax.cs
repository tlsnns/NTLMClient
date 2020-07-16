using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    class Syntax : IBinary
    {
        Guid _UUID;
        ushort _MajorVersion;
        ushort _MinorVersion;

        public Guid UUID { get => _UUID; private set => _UUID = value; }
        public ushort MajorVersion { get => _MajorVersion; private set => _MajorVersion = value; }
        public ushort MinorVersion { get => _MinorVersion; private set => _MinorVersion = value; }

        public Syntax(Guid uuid, ushort majorVersion, ushort minorVersion)
        {
            UUID = uuid;
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(UUID.ToByteArray());
            vs.AddRange(MajorVersion.GetBytes());
            vs.AddRange(MinorVersion.GetBytes());
            return vs;
        }
        public static Syntax Parser(byte[] vs, int offset)
        {
            return new Syntax(vs, offset);
        }
        Syntax(byte[] vs, int offset)
        {
            byte[] tmp = new byte[16];
            Array.Copy(vs, offset, tmp, 0, 16);
            UUID = new Guid(tmp);

            MajorVersion = BitConverter.ToUInt16(vs, offset + 16);//16-2
            MinorVersion = BitConverter.ToUInt16(vs, offset + 18);//18-2

        }
        static public Syntax Create32BitNDR()
        {
            return new Syntax(Guid.Parse("8a885d04-1ceb-11c9-9fe8-08002b104860"), 2, 0);
        }
        static public Syntax Create64BitNDR()
        {
            return new Syntax(Guid.Parse("71710533-beba-4937-8319-b5dbef9ccc36"), 1, 0);
        }
        static public Syntax CreateBindTimeFeatureNegotiation(BindTimeFeatureNegotiationBitmaskFlags bindTimeFeatureNegotiationBitmaskFlags)
        {
            return new Syntax(new Guid(0x6cb71c2c, -26606, 0x4540, bindTimeFeatureNegotiationBitmaskFlags.GetByte()), 1, 0);
        }

    }
}
