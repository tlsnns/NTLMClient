using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace NTLMMessageParse.Models
{
    class Versions : IBinary
    {
        byte _ProductMajorVersion;
        byte _ProductMinorVersion;
        ushort _ProductBuild;
        byte[] _Reserved;
        byte _NTLMRevisionCurrent;

        public byte ProductMajorVersion { get => _ProductMajorVersion; private set => _ProductMajorVersion = value; }
        public byte ProductMinorVersion { get => _ProductMinorVersion; private set => _ProductMinorVersion = value; }
        public ushort ProductBuild { get => _ProductBuild; private set => _ProductBuild = value; }
        public byte[] Reserved { get => _Reserved; private set => _Reserved = value; }
        public byte NTLMRevisionCurrent { get => _NTLMRevisionCurrent; private set => _NTLMRevisionCurrent = value; }
        public Versions()
        {
            ProductMajorVersion = 5;
            ProductMinorVersion = 1;
            ProductBuild = 2600;
            Reserved = new byte[3];
            NTLMRevisionCurrent = 15;
        }
        public Versions(byte[] v, int startIndex)
        {
            ProductMajorVersion = v[startIndex];
            ProductMinorVersion = v[startIndex + 1];
            ProductBuild = BitConverter.ToUInt16(v, startIndex + 2);
            Reserved = new byte[3];
            Array.Copy(v, startIndex + 4, Reserved, 0, 3);
            NTLMRevisionCurrent = v[startIndex + 7];
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.Add(ProductMajorVersion);
            vs.Add(ProductMinorVersion);
            vs.AddRange(ProductBuild.GetBytes());
            vs.AddRange(Reserved);
            vs.Add(NTLMRevisionCurrent);
            return vs;
        }

        public T Parser<T>(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
}
