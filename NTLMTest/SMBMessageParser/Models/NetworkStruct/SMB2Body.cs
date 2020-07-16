using Common;
using SMBMessageParser.Models.Negotiate;
using SMBMessageParser.Models.NetworkStruct;
using SMBMessageParser.Models.NetworkStruct.Close;
using SMBMessageParser.Models.NetworkStruct.Create;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using SMBMessageParser.Models.NetworkStruct.Read;
using SMBMessageParser.Models.NetworkStruct.TreeConnect;
using SMBMessageParser.Models.NetworkStruct.Write;
using SMBMessageParser.Models.SessionSetup;
using System;
using System.Collections.Generic;

namespace SMBMessageParser.Models
{
    public abstract class SMB2Body : IBinary
    {
        public abstract List<byte> DumpBinary();
    }
    class SMB2BodyFactory
    {
        public static SMB2Body CreateSMB2Body(byte[] vs, int offset, ESMB2Command eSMB2Command, SMB2HeaderFlags smb2HeaderFlags, NTStateType ntState)
        {
            SMB2Body sMB2Body = null;
            if (smb2HeaderFlags.HasFlag(SMB2HeaderFlags.ServerToRedir))
            {
                if (ntState == NTStateType.Success || ntState == NTStateType.MoreProcessingRequired || ntState == NTStateType.LogonFailure)
                {
                    switch (eSMB2Command)
                    {
                        case ESMB2Command.NEGOTIATE: sMB2Body = NegotiateResponse.Parser(vs, offset); break;
                        case ESMB2Command.SESSION_SETUP: sMB2Body = SessionSetupResponse.Parser(vs, offset); break;
                        case ESMB2Command.TREE_CONNECT: sMB2Body = TreeConnectResponse.Parser(vs, offset); break;
                        case ESMB2Command.LOGOFF:
                        case ESMB2Command.TREE_DISCONNECT: sMB2Body = LogoffAndTreeDisconnect.Parser(vs, offset); break;
                        case ESMB2Command.CREATE: sMB2Body = CreateResponse.Parser(vs, offset); break;
                        case ESMB2Command.CLOSE: sMB2Body = CloseResponse.Parser(vs, offset); break;
                        case ESMB2Command.WRITE: sMB2Body = WriteResponse.Parser(vs, offset); break;
                        case ESMB2Command.READ: sMB2Body = ReadResponse.Parser(vs, offset); break;
                        case ESMB2Command.IOCTL: sMB2Body = IOCTLResponse.Parser(vs, offset); break;
                        default: throw new Exception("UnKnow SMB2 Command");
                    }
                }
                else
                {
                    sMB2Body = ErrorResponse.Parser(vs, offset);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return sMB2Body;
        }
    }
}
