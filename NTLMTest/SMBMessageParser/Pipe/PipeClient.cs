using SMBMessageParser.Models.NetworkStruct.Create;
using System;
namespace SMBMessageParser.Pipe
{
    public class PipeClient
    {
        bool IsConnectd;
        SMB2Client SMB2Client;
        SMB2Tree SMB2Tree;
        SMB2File SMB2File;
        string PipeName;
        FilePipePrinterAccessMaskFlags FilePipePrinterAccessMaskFlags;

        public PipeClient(string serverName, string pipeName, PipeDirection direction, string strUserName, string strPassword)
        {
            SMB2Client = new SMB2Client(serverName, strUserName, strPassword);
            PipeName = pipeName;
            if (direction == PipeDirection.Read)
            {
                FilePipePrinterAccessMaskFlags = FilePipePrinterAccessMaskFlags.GENERIC_READ;
            }
            else if (direction == PipeDirection.Write)
            {
                FilePipePrinterAccessMaskFlags = FilePipePrinterAccessMaskFlags.GENERIC_WRITE;
            }
            else 
            {
                FilePipePrinterAccessMaskFlags = FilePipePrinterAccessMaskFlags.GENERIC_READ | FilePipePrinterAccessMaskFlags.GENERIC_WRITE;
            }
        }
        public void Connect()
        {
            SMB2Client.Login();
            SMB2Tree = SMB2Client.CreateTree("ipc$");
            SMB2File = SMB2Tree.CreateFile(FilePipePrinterAccessMaskFlags,
                   ShareAccessFlags.ShareRead | ShareAccessFlags.ShareWrite | ShareAccessFlags.ShareDelete,
                   ECreateDisposition.FILE_OPEN, 0,
                   PipeName);
            IsConnectd = true;
        }
        public byte[] Read(uint length)
        {
            if (IsConnectd)
            {
                return SMB2File.Read(length);
            }
            else
            {
                throw new Exception("Pipe No Connect");
            }

        }
        public uint Write(byte[] datas)
        {
            if (IsConnectd)
            {
                return SMB2File.Write(0, datas);
            }
            else
            {
                throw new Exception("Pipe No Connect");
            }
        }
        public void Close()
        {
            SMB2File.Close();
            SMB2Tree.Disconnect();
            SMB2Client.Logout();
        }
    }
}
