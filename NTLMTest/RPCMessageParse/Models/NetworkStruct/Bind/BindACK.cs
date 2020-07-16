using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCMessageParse.Models.NetworkStruct.Bind
{
    class BindACK : SpecialPDU
    {
        ushort _MaxXmitFrag;
        ushort _MaxRecvFrag;
        uint _AssocGroupId;
        ushort _SecondaryEndpointAddressesLength;
        string _SecondaryEndpointAddresses;
        //pad 4 byte rpc header
        byte _ContextResultCount;
        byte _Reserved;
        ushort _Reserved2;
        List<ContextResult> _ContextResults;

        public ushort MaxXmitFrag { get => _MaxXmitFrag; set => _MaxXmitFrag = value; }
        public ushort MaxRecvFrag { get => _MaxRecvFrag; set => _MaxRecvFrag = value; }
        public uint AssocGroupId { get => _AssocGroupId; set => _AssocGroupId = value; }
        public ushort SecondaryEndpointAddressesLength { get => _SecondaryEndpointAddressesLength; set => _SecondaryEndpointAddressesLength = value; }
        public string SecondaryEndpointAddresses { get => _SecondaryEndpointAddresses; set => _SecondaryEndpointAddresses = value; }
        public byte ContextResultCount { get => _ContextResultCount; set => _ContextResultCount = value; }
        public byte Reserved { get => _Reserved; set => _Reserved = value; }
        public ushort Reserved2 { get => _Reserved2; set => _Reserved2 = value; }
        public List<ContextResult> ContextResults { get => _ContextResults; set => _ContextResults = value; }

        public override List<byte> DumpBinary()
        {
            throw new NotImplementedException();
        }
        public static BindACK Parser(byte[] vs, int offset, int specialPDULength)
        {
            return new BindACK(vs, offset, specialPDULength);
        }

        public BindACK(byte[] vs, int offset, int specialPDULength)
        {
            MaxXmitFrag = BitConverter.ToUInt16(vs, offset);//0-2
            MaxRecvFrag = BitConverter.ToUInt16(vs, offset + 2);//2-2
            AssocGroupId = BitConverter.ToUInt32(vs, offset + 4);//4-4
            SecondaryEndpointAddressesLength = BitConverter.ToUInt16(vs, offset + 8);//8-2
            SecondaryEndpointAddresses = Encoding.ASCII.GetString(vs, offset + 10, SecondaryEndpointAddressesLength - 1);//10-SecondaryEndpointAddressesLength

            var padLenth = 4 - (2 + SecondaryEndpointAddressesLength) % 4;
            var newOffset = offset + 10 + SecondaryEndpointAddressesLength + padLenth;

            ContextResultCount = vs[newOffset];//0-1
            Reserved = vs[newOffset + 1];//1-1
            Reserved2 = BitConverter.ToUInt16(vs, newOffset + 2);//2-2

            ContextResults = new List<ContextResult>();
            for (int i = 0; i < ContextResultCount; i++)
            {
                ContextResults.Add(ContextResult.Parser(vs, newOffset + 4 + i * 24));
            }
        }
    }
}
