using System;
using System.Text;
using Common;

namespace Common
{
    public static class Extend
    {
        public static byte[] GetBytes(this ushort instance)
        {
            return BitConverter.GetBytes(instance);
        }
        public static byte[] GetBytes(this int instance)
        {
            return BitConverter.GetBytes(instance);
        }
        public static byte[] GetBytes(this uint instance)
        {
            return BitConverter.GetBytes(instance);
        }
        public static byte[] GetBytes(this long instance)
        {
            return BitConverter.GetBytes(instance);
        }
        public static byte[] GetBytes(this ulong instance)
        {
            return BitConverter.GetBytes(instance);
        }
        public static byte[] GetUnicodeBytes(this string instance)
        {
            return Encoding.Unicode.GetBytes(instance);
        }
        public static string ToBase64String(this byte[] instance)
        {
            return Convert.ToBase64String(instance);
        }
        public static bool CompareArray(this byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }
            return a1.CompareArray(a2, 0, a2.Length);
        }
        public static bool CompareArray(this byte[] smallArrey, byte[] largeArray, int largeArrayStartIndex, int count)
        {
            if (smallArrey.Length < count || largeArray.Length < count)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = 0; i < count; i++)
            {
                if (smallArrey[i] != largeArray[i + largeArrayStartIndex])
                {
                    return false;
                }
            }
            return true;
        }
    }
}