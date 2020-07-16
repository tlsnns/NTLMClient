using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTLMMessageParse.Models.Authenticate
{
    class Blob : IBinary
    {
        public byte[] BlobSignature = new byte[] { 01, 01, 00, 00 };
        public byte[] Reserved = new byte[4];
        public long Timestamp;
        public byte[] ClientChallenge;
        public byte[] Reserved2 = new byte[4];
        public AV_PAIRs TargetInformation;
        public byte[] Reserved3 = new byte[4];
        //
        ushort _Length;
        public ushort Length
        {
            get => _Length;
        }
        public Blob(AV_PAIRs targetInformation)
        {
            if (targetInformation.TryGetTimestamp(out long times))
            {
                Timestamp = times;
            }
            else
            {
                Timestamp = DateTimeOffset.Now.ToFileTime();
            }

            ClientChallenge = new byte[8];
            HashUtils.WriteRandomBytes(ClientChallenge);
            TargetInformation = targetInformation;
            _Length = (ushort)(32 + targetInformation.Length);
        }
        public List<byte> DumpBinary()
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(BlobSignature);
            vs.AddRange(Reserved);
            vs.AddRange(Timestamp.GetBytes());
            vs.AddRange(ClientChallenge);
            vs.AddRange(Reserved2);
            vs.AddRange(TargetInformation.DumpBinary());
            vs.AddRange(Reserved3);
            return vs;
        }

        public T Parser<T>(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
}
