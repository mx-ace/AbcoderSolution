using System;

namespace Abcoder.Tools.ABUtility
{
    public static class ABDateTimeTools
    {
        private const string WeekSequence = "日一二三四五六";
        /// <summary>
        /// 把时间对象计算出时间戳值
        /// </summary>
        /// <param name="dateTime">转换的时间对象</param>
        /// <returns></returns>
        public static long GetTimestamp(this DateTime dateTime)
        {
            //时间戳起始时间
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取当前时间的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTimestamp()
        {
            return DateTime.Now.GetTimestamp();
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime ConvertTimeStampToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(timeStamp);
        }

        /// <summary>
        /// 获取时间对应星期中文
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>返回实例：星期日</returns>
        public static string GetWeekText_CH(this DateTime dateTime)
        {
            var week = (int)dateTime.DayOfWeek;
            return $"星期{WeekSequence[week]}";
        }
    }
}
