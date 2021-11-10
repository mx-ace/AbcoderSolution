using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abcoder.Tools.ABUtility
{
    /// <summary>
    /// CRC校验类
    /// date:2016-7-25
    /// author:abcoder
    /// </summary>
    public static class ABCrcCheck
    {
        /// <summary>
        /// 根据流以及需要校验的长度,计算校验码的值
        /// </summary>
        /// <param name="data">校验的数据流</param>
        /// <param name="len">需要校验的流长度长度;负数,则全部校验</param>
        /// <returns></returns>
        public static byte[] GetCrc16Code(this byte[] data, int len = -1)
        {
            if (len < 0)
                len = data.Length;
            var temdata = new byte[2];
            int i;
            var xda = 0xFFFF;
            const int xdapoly = 0xA001;
            for (i = 0; i < len; i++)
            {
                xda ^= data[i];
                int j;
                for (j = 0; j < 8; j++)
                {
                    var xdabit = xda & 0x01;
                    xda >>= 1;
                    if (xdabit == 1)
                        xda ^= xdapoly;
                }
            }
            temdata[0] = (byte)(xda & 0xFF);
            temdata[1] = (byte)(xda >> 8);
            return temdata;
        }
    }
}
