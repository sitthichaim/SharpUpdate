using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SharpUpdate
{
    internal enum HashType
    {
        MD5,
        SHA1,
        SHA256
    }

    internal class Hasher
    {
        internal static string HashFile (string filePath, HashType algo)
        {
            switch (algo)
            {
                case HashType.MD5:
                    Debug.WriteLine("filePath =============> " + filePath);
                    string ht = MakeHashString(MD5.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                    Debug.WriteLine("ht =============> " + ht);
                    return ht;
                case HashType.SHA1:
                    return MakeHashString(SHA1.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                case HashType.SHA256:
                    return MakeHashString(SHA256.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                default:
                    return "";
            }
        }

        private static string MakeHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

    }
}
