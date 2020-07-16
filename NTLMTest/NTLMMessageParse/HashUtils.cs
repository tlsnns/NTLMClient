using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NTLMMessageParse
{
    static class HashUtils
    {
        public static void WriteRandomBytes(byte[] vs)
        {
            new Random(Guid.NewGuid().GetHashCode()).NextBytes(vs);
        }
        public static byte[] GetNTProofStr(byte[] serverChallenges, byte[] blob, byte[] ntlmv2Hash)
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(serverChallenges);
            vs.AddRange(blob);
            return HMACMD5Compute(vs.ToArray(), ntlmv2Hash);
        }
        public static byte[] NTLMV2Hash(byte[] passwordNTHash, string strUserName, byte[] targets)
        {
            List<byte> vs = new List<byte>();
            vs.AddRange(Encoding.Unicode.GetBytes(strUserName.ToUpper()));
            vs.AddRange(targets);
            return HMACMD5Compute(vs.ToArray(), passwordNTHash);
        }
        public static byte[] NTLMV2Hash(byte[] passwordNTHash, string strUserName, string strTarget)
        {
            var pt = Encoding.Unicode.GetBytes(strUserName.ToUpper() + strTarget);
            return HMACMD5Compute(pt, passwordNTHash);
        }
        public static byte[] PasswordNTHash(string strPassword)
        {
            var bPassword = Encoding.Unicode.GetBytes(strPassword);
            MD4 md4 = new MD4();
            return md4.ComputeHash(bPassword);
        }
        public static byte[] PasswordLMHash(string strPassword)
        {
            List<byte> tmp = new List<byte>();
            tmp.AddRange(Encoding.ASCII.GetBytes(strPassword.ToUpper()));
            while (tmp.Count < 14)
            {
                tmp.Add(0);
            }
            var a1 = SevenByteToEightByte(tmp.GetRange(0, 7).ToArray());
            var a2 = SevenByteToEightByte(tmp.GetRange(7, 7).ToArray());

            List<byte> vs = new List<byte>();
            byte[] plaintext = Encoding.ASCII.GetBytes(@"KGS!@#$%");
            vs.AddRange(DESEncrypt(plaintext, a1));
            vs.AddRange(DESEncrypt(plaintext, a2));
            return vs.ToArray();
        }
        static byte[] SevenByteToEightByte(byte[] tmp)
        {
            if (tmp.Length != 7)
            {
                throw new ArgumentOutOfRangeException("mast Must be 7 bytes");
            }
            List<byte> vs = new List<byte>();
            vs.Add((byte)(tmp[0] & 0xfe));
            for (int i = 1; i < 7; i++)
            {
                vs.Add(TwoInOne(tmp[i - 1], tmp[i], i));
            }
            vs.Add((byte)((tmp[6] << 1) & 0xfe));
            return vs.ToArray();
        }
        static byte TwoInOne(byte first, byte Second, int rightShift)
        {
            int leftShift = 8 - rightShift;
            var t = (first << leftShift) | (Second >> rightShift);
            return (byte)(t & 0xfe);
        }
        public static byte[] MD5Compute(byte[] plaintext)
        {
            return MD5.Create().ComputeHash(plaintext);
        }
        public static byte[] HMACMD5Compute(byte[] plaintext, byte[] key)
        {
            return new HMACMD5(key).ComputeHash(plaintext);
        }
        public static byte[] DESEncrypt(byte[] plaintext, byte[] key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Padding = PaddingMode.None;
            MemoryStream mStream = new MemoryStream();

            FieldInfo fi = des.GetType().GetField("FeedbackSizeValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var FeedbackSizeValue = fi.GetValue(des);

            Type t = Type.GetType("System.Security.Cryptography.CryptoAPITransformMode");
            object cryptoAPITransformMode = t.GetField("Encrypt", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).GetValue(t);

            MethodInfo mi = des.GetType().GetMethod("_NewEncryptor", BindingFlags.Instance | BindingFlags.NonPublic);
            ICryptoTransform desCrypt = mi.Invoke(des, new object[] { key, CipherMode.ECB, null, FeedbackSizeValue, cryptoAPITransformMode }) as ICryptoTransform;

            CryptoStream cStream = new CryptoStream(mStream, desCrypt, CryptoStreamMode.Write);
            cStream.Write(plaintext, 0, plaintext.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }
    }
}
