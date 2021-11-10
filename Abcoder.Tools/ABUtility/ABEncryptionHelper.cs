using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Abcoder.Tools.ABUtility
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
   public static  class ABEncryptionHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plaintext">加密明文</param>
        /// <returns>加密密文</returns>
        public static string MD5Encryption(string plaintext)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).ToUpper().Replace("-", "");
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < hashBytes.Length; i++)
            //{
            //    sb.Append(hashBytes[i].ToString("x2"));
            //}
            //return sb.ToString();
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>明文</returns>
        public static byte[] AESDecrypt(byte[] Data, byte[] bKey, byte[] bVector)
        {
            bKey = bKey.Take(16).ToArray();
            byte[] original = null; // 解密后的明文
            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流，存储密文
                using (MemoryStream Memory = new MemoryStream(Data))
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream Decryptor = new CryptoStream(Memory,
                    Aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read))
                    {
                        // 明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] Buffer = new byte[1024];
                            int readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }
                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }
            return original;
        }
    }
}
