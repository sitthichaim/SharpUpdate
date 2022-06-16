using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace SharpUpdate
{
    internal enum HashType
    {
        HD5,
        SHA1,
        SHA256
    }

    internal class Hasher
    {
        internal static string HashFile (string filePath, HashType algo)
        {
            switch (algo)
            {
                case HashType.HD5:
                    return MakeHashString(MD5.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                    break;
                case HashType.SHA1:
                    return MakeHashString(SHA1.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                    break;
                case HashType.SHA256:
                    return MakeHashString(SHA256.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                    break;
                default:
                    return "";
            }
        }

        private static string MakeHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
                sb.Append(b.ToString("X2").ToLower());

            return sb.ToString();
        }

    }
}
