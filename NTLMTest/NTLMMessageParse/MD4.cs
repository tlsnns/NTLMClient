using System;

namespace NTLMMessageParse
{
    public class MD4
    {
        private UInt32 A;
        private UInt32 B;
        private UInt32 C;
        private UInt32 D;

        private UInt32 F(UInt32 X, UInt32 Y, UInt32 Z)
        {
            return (X & Y) | ((~X) & Z);
        }

        private UInt32 G(UInt32 X, UInt32 Y, UInt32 Z)
        {
            return (X & Y) | (X & Z) | (Y & Z);
        }

        private UInt32 H(UInt32 X, UInt32 Y, UInt32 Z)
        {
            return X ^ Y ^ Z;
        }

        private UInt32 LShift(UInt32 X, int s)
        {
            X &= 0xFFFFFFFF; return ((X << s) & 0xFFFFFFFF) | (X >> (32 - s));
        }

        private void FF(ref UInt32 X, UInt32 Y, UInt32 Z, UInt32 O, UInt32 P, int S)
        {
            X = LShift(X + F(Y, Z, O) + P, S);
        }

        private void HH(ref UInt32 X, UInt32 Y, UInt32 Z, UInt32 O, UInt32 P, int S)
        {
            X = LShift(X + G(Y, Z, O) + P + (UInt32)0x5A827999, S);
        }

        private void GG(ref UInt32 X, UInt32 Y, UInt32 Z, UInt32 O, UInt32 P, int S)
        {
            X = LShift(X + H(Y, Z, O) + P + (UInt32)0x6ED9EBA1, S);
        }

        private void MdFour64(ref UInt32[] M)
        {
            UInt32 AA, BB, CC, DD;
            UInt32[] X = new UInt32[16];

            for (int j = 0; j < 16; j++)
            {
                X[j] = M[j];
            }

            AA = A;
            BB = B;
            CC = C;
            DD = D;

            FF(ref A, B, C, D, X[0], 3);
            FF(ref D, A, B, C, X[1], 7);
            FF(ref C, D, A, B, X[2], 11);
            FF(ref B, C, D, A, X[3], 19);
            FF(ref A, B, C, D, X[4], 3);
            FF(ref D, A, B, C, X[5], 7);
            FF(ref C, D, A, B, X[6], 11);
            FF(ref B, C, D, A, X[7], 19);
            FF(ref A, B, C, D, X[8], 3);
            FF(ref D, A, B, C, X[9], 7);
            FF(ref C, D, A, B, X[10], 11);
            FF(ref B, C, D, A, X[11], 19);
            FF(ref A, B, C, D, X[12], 3);
            FF(ref D, A, B, C, X[13], 7);
            FF(ref C, D, A, B, X[14], 11);
            FF(ref B, C, D, A, X[15], 19);

            HH(ref A, B, C, D, X[0], 3);
            HH(ref D, A, B, C, X[4], 5);
            HH(ref C, D, A, B, X[8], 9);
            HH(ref B, C, D, A, X[12], 13);
            HH(ref A, B, C, D, X[1], 3);
            HH(ref D, A, B, C, X[5], 5);
            HH(ref C, D, A, B, X[9], 9);
            HH(ref B, C, D, A, X[13], 13);
            HH(ref A, B, C, D, X[2], 3);
            HH(ref D, A, B, C, X[6], 5);
            HH(ref C, D, A, B, X[10], 9);
            HH(ref B, C, D, A, X[14], 13);
            HH(ref A, B, C, D, X[3], 3);
            HH(ref D, A, B, C, X[7], 5);
            HH(ref C, D, A, B, X[11], 9);
            HH(ref B, C, D, A, X[15], 13);

            GG(ref A, B, C, D, X[0], 3);
            GG(ref D, A, B, C, X[8], 9);
            GG(ref C, D, A, B, X[4], 11);
            GG(ref B, C, D, A, X[12], 15);
            GG(ref A, B, C, D, X[2], 3);
            GG(ref D, A, B, C, X[10], 9);
            GG(ref C, D, A, B, X[6], 11);
            GG(ref B, C, D, A, X[14], 15);
            GG(ref A, B, C, D, X[1], 3);
            GG(ref D, A, B, C, X[9], 9);
            GG(ref C, D, A, B, X[5], 11);
            GG(ref B, C, D, A, X[13], 15);
            GG(ref A, B, C, D, X[3], 3);
            GG(ref D, A, B, C, X[11], 9);
            GG(ref C, D, A, B, X[7], 11);
            GG(ref B, C, D, A, X[15], 15);

            A += AA;
            B += BB;
            C += CC;
            D += DD;

            A &= 0xFFFFFFFF;
            B &= 0xFFFFFFFF;
            C &= 0xFFFFFFFF;
            D &= 0xFFFFFFFF;

            for (int j = 0; j < 16; j++)
            {
                X[j] = 0;
            }
        }

        private void Copy64(ref UInt32[] M, byte[] input, int left)
        {
            for (int i = 0; i < 16; i++)
            {
                M[i] = (UInt32)((input[i * 4 + left + 3] << 24) | (input[i * 4 + left + 2] << 16) | (input[i * 4 + left + 1] << 8) | (input[i * 4 + left + 0] << 0));
            }
        }

        private void Copy4(ref byte[] output, int left, UInt32 x)
        {
            output[left + 0] = (byte)(x & 0xFF);
            output[left + 1] = (byte)((x >> 8) & 0xFF);
            output[left + 2] = (byte)((x >> 16) & 0xFF);
            output[left + 3] = (byte)((x >> 24) & 0xFF);
        }

        private byte[] MdFour(byte[] input, int n)
        {
            byte[] output = new byte[16];
            byte[] buf = new byte[128];
            UInt32[] M = new UInt32[16];
            UInt32 b = (UInt32)(n * 8);

            A = 0x67452301;
            B = 0xefcdab89;
            C = 0x98badcfe;
            D = 0x10325476;

            int j = 0;
            while (n > 64)
            {
                Copy64(ref M, input, 0);
                MdFour64(ref M);
                j += 64;
                n -= 64;
            }

            for (int i = 0; i < 128; i++)
            {
                buf[i] = 0;
            }

            for (int i = 0; i < n; i++)
            {
                buf[i] = input[j + i];
            }

            buf[n] = 0x80;

            if (n <= 55)
            {
                Copy4(ref buf, 56, b);
                Copy64(ref M, buf, 0);
                MdFour64(ref M);
            }
            else
            {
                Copy4(ref buf, 120, b);
                Copy64(ref M, buf, 0);
                MdFour64(ref M);
                Copy64(ref M, buf, 64);
                MdFour64(ref M);
            }

            for (int i = 0; i < 128; i++)
            {
                buf[i] = 0;
            }

            Copy64(ref M, buf, 0);
            Copy4(ref output, 0, A);
            Copy4(ref output, 4, B);
            Copy4(ref output, 8, C);
            Copy4(ref output, 12, D);
            A = B = C = D = 0;
            return output;
        }
        public byte[] ComputeHash(byte[] plaintext)
        {
            return MdFour(plaintext, plaintext.Length);
        }
    }
}