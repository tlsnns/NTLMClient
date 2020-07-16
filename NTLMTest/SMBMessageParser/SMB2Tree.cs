using SMBMessageParser.Models.NetworkStruct.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBMessageParser
{
    public class SMB2Tree
    {
        SMB2Worker SMB2Client;
        public string ShareName { get; private set; }
        public uint TreeId { get; private set; }
        internal SMB2Tree(SMB2Worker smb2Client, string strShareName, uint treeId)
        {
            SMB2Client = smb2Client;
            ShareName = strShareName;
            TreeId = treeId;
        }

        public void Disconnect()
        {
            SMB2Client.TreeDisconnect(TreeId);
        }


        public SMB2File CreateFile(
           FilePipePrinterAccessMaskFlags maskFlags,
           ShareAccessFlags shareAccess,
           ECreateDisposition createDisposition,
           uint createOptions,
           string strFileName)
        {
            var fid = SMB2Client.CreateFile(TreeId, maskFlags, shareAccess, createDisposition, createOptions, strFileName,
                out ulong length);
            return new SMB2File(SMB2Client, this, strFileName, fid, length);
        }
    }
}
