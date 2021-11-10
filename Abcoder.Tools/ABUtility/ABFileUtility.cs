using System;
using System.IO;

namespace Abcoder.Tools.ABUtility
{
    /// <summary>
    /// 文件保存类
    /// </summary>
    public class ABFileUtility
    {
        /// <summary>
        /// 把路径下的文件处理为Base64数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileBase64(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                var buffer = new byte[fileInfo.Length];
                fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                return Convert.ToBase64String(buffer);
            }
            return null;
        }


        /// <summary>
        /// 把Base64文件保存到指定位置
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="filePath"></param>
        public static void SaveFile(string base64, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var buffer = Convert.FromBase64String(base64);
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }
    }
}
