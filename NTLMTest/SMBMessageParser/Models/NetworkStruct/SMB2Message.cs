using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models
{
    class SMB2Message : IBinary
    {
        SMB2Header _SMB2Header;
        SMB2Body _SMB2Body;

        public SMB2Header SMB2Header { get => _SMB2Header; private set => _SMB2Header = value; }
        public SMB2Body SMB2Body { get => _SMB2Body; private set => _SMB2Body = value; }
        public SMB2Message(SMB2Header sMB2Header, SMB2Body sMB2Body)
        {
            SMB2Header = sMB2Header;
            SMB2Body = sMB2Body;
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(SMB2Header.DumpBinary());
            vs.AddRange(SMB2Body.DumpBinary());
            return vs;
        }
        public static SMB2Message Parser(byte[] vs)
        {
            var sh = SMB2Header.Parser(vs, 0);
            SMB2Body sb = SMB2BodyFactory.CreateSMB2Body(vs, sh.StructureSize, sh.Command, sh.Flag, sh.Status);
            return new SMB2Message(sh, sb);
        }
    }
}
