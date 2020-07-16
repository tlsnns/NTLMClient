using RPCMessageParse.Models.NetworkStruct.NDR;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using RPCMessageParse.Models.NetworkStruct.Auth;

namespace RPCMessageParse.Models.NetworkStruct
{
    class GenericPDU : IBinary
    {
        byte _MajorVersion;
        byte _MinorVersion;
        PDUType _PacketType;
        PDUFlags _PacketFlags;
        DataRepresentationFormatLabel _DataRepresentation;
        ushort _FragLength;
        ushort _AuthLength;
        uint _CallId;
        SpecialPDU _SpecialPDU;
        SecTrailer _AuthInfo;
        byte[] _AuthDatas;
        public byte MajorVersion { get => _MajorVersion; private set => _MajorVersion = value; }
        public byte MinorVersion { get => _MinorVersion; private set => _MinorVersion = value; }
        public PDUType PacketType { get => _PacketType; private set => _PacketType = value; }
        public PDUFlags PacketFlags { get => _PacketFlags; private set => _PacketFlags = value; }
        public DataRepresentationFormatLabel DataRepresentation { get => _DataRepresentation; private set => _DataRepresentation = value; }
        public ushort FragLength { get => _FragLength; private set => _FragLength = value; }
        public ushort AuthLength { get => _AuthLength; private set => _AuthLength = value; }
        public uint CallId { get => _CallId; private set => _CallId = value; }
        public SpecialPDU SpecialPDU { get => _SpecialPDU; private set => _SpecialPDU = value; }
        public SecTrailer AuthInfo { get => _AuthInfo; private set => _AuthInfo = value; }
        public byte[] AuthDatas { get => _AuthDatas; private set => _AuthDatas = value; }


        public GenericPDU(PDUType pduType, PDUFlags pduFlags, DataRepresentationFormatLabel dataRepresentation, uint callId, SpecialPDU body)
        {
            MajorVersion = 5;
            MinorVersion = 0;
            PacketType = pduType;
            PacketFlags = pduFlags;
            DataRepresentation = dataRepresentation;
            FragLength = (ushort)(16 + body.Length);
            AuthLength = 0;
            CallId = callId;
            SpecialPDU = body;
        }
        public GenericPDU(PDUType pduType, uint callId, SpecialPDU body) :
            this(pduType, PDUFlags.FIRST_FRAG | PDUFlags.LAST_FRAG, new DataRepresentationFormatLabel(), callId, body)
        {

        }
        public GenericPDU(PDUType pduType, PDUFlags pduFlags, DataRepresentationFormatLabel dataRepresentation, uint callId, SpecialPDU body,
    SecurityProviderType authType, AuthenticationLevelType authLevel, byte authPadLength, uint authContextId, byte[] authDatas)
    : this(pduType, pduFlags, dataRepresentation, callId, body)
        {
            FragLength = (ushort)(FragLength + 8 + authDatas.Length);
            AuthLength = (ushort)authDatas.Length;

            AuthInfo = new SecTrailer(authType, authLevel, authPadLength, authContextId);
            AuthDatas = authDatas;
        }
        public GenericPDU(PDUType pduType, uint callId, SpecialPDU body,
   SecurityProviderType authType, AuthenticationLevelType authLevel, byte authPadLength, uint authContextId, byte[] authDatas)
   : this(pduType, PDUFlags.FIRST_FRAG | PDUFlags.LAST_FRAG, new DataRepresentationFormatLabel(), callId, body,
         authType, authLevel, authPadLength, authContextId, authDatas)
        {

        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.Add(MajorVersion);
            vs.Add(MinorVersion);
            vs.Add(PacketType.GetByte());
            vs.Add(PacketFlags.GetByte());
            vs.AddRange(DataRepresentation.DumpBinary());
            vs.AddRange(FragLength.GetBytes());
            vs.AddRange(AuthLength.GetBytes());
            vs.AddRange(CallId.GetBytes());
            vs.AddRange(SpecialPDU.DumpBinary());
            if (AuthLength > 0)
            {
                vs.AddRange(AuthInfo.DumpBinary());
                vs.AddRange(AuthDatas);
            }
            return vs;
        }
        public static GenericPDU Parser(byte[] vs)
        {
            return new GenericPDU(vs);
        }
        GenericPDU(byte[] vs)
        {
            MajorVersion = vs[0];//0-1
            MinorVersion = vs[1];//1-1
            PacketType = (PDUType)vs[2];//2-1
            PacketFlags = (PDUFlags)vs[3];//3-1
            DataRepresentation = DataRepresentationFormatLabel.Parser(vs, +4);//4-4
            FragLength = BitConverter.ToUInt16(vs, 8);//8-2
            AuthLength = BitConverter.ToUInt16(vs, 10);//10-2
            CallId = BitConverter.ToUInt32(vs, 12);//12-4

            var specialPDULength = FragLength - 16;
            ;
            if (AuthLength > 0)
            {
                specialPDULength = specialPDULength - AuthLength - 8;
                var authDatasIndex = FragLength - AuthLength;
                var authInfoIndex = authDatasIndex - 8;

                AuthInfo = SecTrailer.Parser(vs, authInfoIndex);
                AuthDatas = new byte[AuthLength];
                Array.Copy(vs, authDatasIndex, AuthDatas, 0, AuthLength);
            }
            SpecialPDU = PDUBodyFactory.CreatePDUBody(vs, 16, PacketType, specialPDULength);//16-?
        }
    }
}
