using SMBMessageParser.Models.NetworkStruct.IOCTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBMessageParser
{
    public class SMB2File
    {

        SMB2Worker SMB2Worker;
        public SMB2Tree Tree { get; private set; }
        public string fileName { get; private set; }
        public Guid FileId { get; private set; }
        public ulong Length { get; private set; }
        internal SMB2File(SMB2Worker smb2Client, SMB2Tree tree, string strfileName, Guid fileId, ulong length)
        {
            SMB2Worker = smb2Client;
            Tree = tree;
            fileName = strfileName;
            FileId = fileId;
            Length = length;
        }
        public void Close()
        {
            SMB2Worker.CloseFile(Tree.TreeId, FileId);
        }
        public uint Write(ulong offset, byte[] datas)
        {
            return SMB2Worker.Write(Tree.TreeId, offset, FileId, datas);
        }
        public byte[] Read(uint fileLength)
        {
            return SMB2Worker.Read(Tree.TreeId, fileLength, FileId);
        }
        public byte[] IOCTL(CTLCodeType ctlCodeType, CTLType ctlType, byte[] buffer)
        {
            return SMB2Worker.IOCTL(Tree.TreeId, ctlCodeType, FileId, ctlType, buffer);
        }
    }
}
