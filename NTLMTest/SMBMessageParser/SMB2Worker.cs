using System;
using SMBMessageParser.Models;
using SMBMessageParser.Models.Negotiate;
using SMBMessageParser.Models.SessionSetup;
using NTLMMessageParse;
using SMBMessageParser.Models.TreeConnect;
using Common;
using SMBMessageParser.Models.NetworkStruct.Create;
using SMBMessageParser.Models.NetworkStruct.Read;
using SMBMessageParser.Models.NetworkStruct.Write;
using SMBMessageParser.Models.NetworkStruct.Close;
using System.Threading;
using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using SMBMessageParser.Models.NetworkStruct;
using SMBMessageParser.Pipe;

namespace SMBMessageParser
{
    class SMB2Worker
    {

        string IpAddress;
        SMBTransport SMBTransport;

        ConcurrentDictionary<ulong, SMB2Message> ResponseMessages;

        volatile bool IsRun;

        bool IsConnectd;
        bool IsBuildSession;

        ulong SessionId;
        long MessageId;
        public SMB2Worker(string strIpAddress, int destinationPort)
        {
            IpAddress = strIpAddress;
            SMBTransport = new SMBTransportOnTCP(strIpAddress, destinationPort);
            ResponseMessages = new ConcurrentDictionary<ulong, SMB2Message>();

            IsRun = true;
            Thread Thread = new Thread(ReceiveDatas);
            Thread.IsBackground = true;
            Thread.Start();

            MessageId = -1;
        }
        public void Close()
        {
            IsRun = false;
            SMBTransport.Close();
        }


        void ReceiveDatas()
        {
            while (IsRun)
            {
                var data = SMBTransport.ReceiveDatas();
                if (data != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        var sm = SMB2Message.Parser(data);
                        if (sm.SMB2Header.Status != NTStateType.Pending)
                        {
                            ResponseMessages.TryAdd(sm.SMB2Header.MessageId, sm);
                        }
                    });
                }
            }
        }
        SMB2Message GetMessage(ulong mid)
        {
            SMB2Message smb2Message;
            while (true)
            {
                if (ResponseMessages.TryRemove(mid, out smb2Message))
                {
                    break;
                }
            }
            return smb2Message;
        }
        public void Negotiate()
        {
            if (IsConnectd)
            {
                throw new Exception("Repeat Negotiate");
            }
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header smb2Header = new SMB2Header(ESMB2Command.NEGOTIATE, SMB2HeaderFlags.None, mid, 0, 0);
            SMB2Body smb2Body = new NegotiateRequest(ESecurityMode.SMB2_NEGOTIATE_SIGNING_ENABLED, Guid.NewGuid());
            SMB2Message smb2Message = new SMB2Message(smb2Header, smb2Body);
            SMBTransport.SendDatas(smb2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status == 0)
            {
                IsConnectd = true;
            }
            else
            {
                throw new Exception("Negotiate Status error:" + sm.SMB2Header.Status);
            }
        }

        public void SessionSetup(string userName, string password)
        {
            if (!IsConnectd)
            {
                throw new Exception("no Negotiate");
            }
            if (IsBuildSession)
            {
                throw new Exception("Repeat build session");
            }

            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            var type1Data = NTLMMessageHelper.CreateType1().ToArray();
            var sm = SendSessionSetupRequest(mid, 0, type1Data);
            var ssr = sm.SMB2Body as SessionSetupResponse;
            SessionId = sm.SMB2Header.SessionId;
            byte[] securityBuffer = ssr.SecurityBuffer;

            mid = (ulong)Interlocked.Increment(ref MessageId);
            var type3Data = NTLMMessageHelper.CreateType3(securityBuffer, userName, password).ToArray();
            sm = SendSessionSetupRequest(mid, SessionId, type3Data);

            IsBuildSession = true;
        }
        SMB2Message SendSessionSetupRequest(ulong mid, ulong sessionId, byte[] secData)
        {
            SMB2Header smb2Header = new SMB2Header(ESMB2Command.SESSION_SETUP, SMB2HeaderFlags.None, mid, 0, sessionId);
            SMB2Body smb2Body = new SessionSetupRequest(secData);
            SMB2Message smb2Message = new SMB2Message(smb2Header, smb2Body);
            SMBTransport.SendDatas(smb2Message.DumpBinary());
            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status != NTStateType.Success && sm.SMB2Header.Status != NTStateType.MoreProcessingRequired)
            {
                throw new Exception("SessionSetup Status error:" + sm.SMB2Header.Status);
            }
            return sm;
        }
        public void SessionLogoff()
        {
            if (IsBuildSession)
            {
                ulong mid = (ulong)Interlocked.Increment(ref MessageId);
                SMB2Header smb2Header = new SMB2Header(ESMB2Command.LOGOFF, SMB2HeaderFlags.None, mid, 0, SessionId);
                SMB2Body smb2Body = new LogoffAndTreeDisconnect();
                SMB2Message smb2Message = new SMB2Message(smb2Header, smb2Body);
                SMBTransport.SendDatas(smb2Message.DumpBinary());
                var sm = GetMessage(mid);
                if (sm.SMB2Header.Status != NTStateType.Success)
                {
                    throw new Exception("SessionLogoff Status error:" + sm.SMB2Header.Status);
                }
                IsBuildSession = false;
            }
        }
        public uint TreeConnect(string strShareName)
        {
            if (!IsBuildSession)
            {
                throw new Exception("no build session");
            }
            string strPath = @"\\" + IpAddress + @"\" + strShareName;
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header sMB2Header = new SMB2Header(ESMB2Command.TREE_CONNECT, SMB2HeaderFlags.None, mid, 0, SessionId);
            SMB2Body sMB2Body = new TreeConnectRequest(strPath.GetUnicodeBytes());
            SMB2Message sMB2Message = new SMB2Message(sMB2Header, sMB2Body);
            SMBTransport.SendDatas(sMB2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status == 0)
            {
                return sm.SMB2Header.TreeId;
            }
            else
            {
                throw new Exception("TreeConnect Status error:" + sm.SMB2Header.Status);
            }
        }
        public void TreeDisconnect(uint treeId)
        {
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header smb2Header = new SMB2Header(ESMB2Command.TREE_DISCONNECT, SMB2HeaderFlags.None, mid, treeId, SessionId);
            SMB2Body smb2Body = new LogoffAndTreeDisconnect();
            SMB2Message smb2Message = new SMB2Message(smb2Header, smb2Body);
            SMBTransport.SendDatas(smb2Message.DumpBinary());
            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status != NTStateType.Success)
            {
                throw new Exception("TreeDisconnect Status error:" + sm.SMB2Header.Status);
            }
        }
        public Guid CreateFile(uint treeId,
            FilePipePrinterAccessMaskFlags maskFlags,
            ShareAccessFlags shareAccess,
            ECreateDisposition createDisposition,
            uint createOptions,
            string strFileName, out ulong length)
        {
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header sMB2Header = new SMB2Header(ESMB2Command.CREATE, SMB2HeaderFlags.None, mid, treeId, SessionId);
            SMB2Body sMB2Body = new CreateRequest(maskFlags, shareAccess, createDisposition, createOptions, strFileName.GetUnicodeBytes());
            SMB2Message sMB2Message = new SMB2Message(sMB2Header, sMB2Body);
            SMBTransport.SendDatas(sMB2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status == 0)
            {
                var cr = sm.SMB2Body as CreateResponse;

                length = cr.EndofFile;
                return cr.FileId;

            }
            else
            {
                throw new Exception("CreateFile Status error:" + sm.SMB2Header.Status);
            }
        }
        public void CloseFile(uint treeId, Guid fileId)
        {
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header sMB2Header = new SMB2Header(ESMB2Command.CLOSE, SMB2HeaderFlags.None, mid, treeId, SessionId);
            SMB2Body sMB2Body = new CloseRequest(fileId);
            SMB2Message sMB2Message = new SMB2Message(sMB2Header, sMB2Body);
            SMBTransport.SendDatas(sMB2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status != 0)
            {
                throw new Exception("CloseFile Status error:" + sm.SMB2Header.Status);
            }
        }
        public uint Write(uint treeId, ulong offset, Guid fileId, byte[] datas)
        {
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header sMB2Header = new SMB2Header(ESMB2Command.WRITE, SMB2HeaderFlags.None, mid, treeId, SessionId);
            SMB2Body sMB2Body = new WriteRequest(offset, fileId, datas);
            SMB2Message sMB2Message = new SMB2Message(sMB2Header, sMB2Body);
            SMBTransport.SendDatas(sMB2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status == 0)
            {
                var s = sm.SMB2Body as WriteResponse;
                return s.Count;
            }
            else
            {
                throw new Exception("Write Status error:" + sm.SMB2Header.Status);
            }
        }
        public byte[] Read(uint treeId, uint fileLength, Guid fileId)
        {
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header sMB2Header = new SMB2Header(ESMB2Command.READ, SMB2HeaderFlags.None, mid, treeId, SessionId);
            SMB2Body sMB2Body = new ReadRequest(fileLength, fileId);
            SMB2Message sMB2Message = new SMB2Message(sMB2Header, sMB2Body);
            SMBTransport.SendDatas(sMB2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status == 0)
            {
                var s = sm.SMB2Body as ReadResponse;
                return s.Buffer;
            }
            else if (sm.SMB2Header.Status == NTStateType.PIPE_DISCONNECTED)
            {
                throw new PipeDisConnectException();
            }
            else
            {
                throw new Exception("Read Status error:" + sm.SMB2Header.Status);
            }
        }
        public byte[] IOCTL(uint treeId, CTLCodeType ctlCodeType, Guid fileId, CTLType ctlType, byte[] buffer)
        {
            ulong mid = (ulong)Interlocked.Increment(ref MessageId);
            SMB2Header sMB2Header = new SMB2Header(ESMB2Command.IOCTL, SMB2HeaderFlags.None, mid, treeId, SessionId);
            SMB2Body sMB2Body = new IOCTLRequest(ctlCodeType, fileId, ctlType, buffer);
            SMB2Message sMB2Message = new SMB2Message(sMB2Header, sMB2Body);
            SMBTransport.SendDatas(sMB2Message.DumpBinary());

            var sm = GetMessage(mid);
            if (sm.SMB2Header.Status == 0)
            {
                var s = sm.SMB2Body as IOCTLResponse;
                return s.Buffer;
            }
            else
            {
                throw new Exception("IOCTL Status error:" + sm.SMB2Header.Status);
            }
        }
    }
}