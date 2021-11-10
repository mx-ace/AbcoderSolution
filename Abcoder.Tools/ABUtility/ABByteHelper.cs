using System;
using System.Collections.Generic;

namespace Abcoder.Tools.ABUtility
{
    public static  class ABByteHelper
    {
        /// <summary>
        /// 字节流字符串转换,间隔符为"-"
        /// 格式如:01-03-00-41-00-0A-95-D9
        /// </summary>
        /// <param name="hexStr">用于转换的字符串</param>
        /// <returns>转换的字节数组</returns>
        public static List<byte> ByteArryConvert(this string hexStr)
        {
            hexStr = hexStr.ToUpper();
            var hexSequence = "0123456789ABCDEF";
            var strByteArray = new List<string>();
            if (hexStr.Contains("-"))
                strByteArray = new List<string>(hexStr.Split('-'));
            else if (hexStr.Length % 2 == 0)
            {
                for (int i = 0; i < hexStr.Length; i += 2)
                {
                    strByteArray.Add(hexStr.Substring(i, 2));
                }
            }
            else
            {
                throw new Exception("转换失败");
            }
            var buffer = new List<byte>();
            foreach (var item in strByteArray)
            {
                if (item.Length == 2 && hexSequence.Contains(item[0].ToString()) && hexSequence.Contains(item[1].ToString()))
                {
                    buffer.Add((byte)((hexSequence.IndexOf(item[0]) << 4) | (hexSequence.IndexOf(item[1]))));
                }
            }
            return buffer;
        }
    }
}
