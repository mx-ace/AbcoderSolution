using Abcoder.Tools.ABLoger;
using System;
using System.Configuration;

namespace Abcoder.Tools.ABUtility
{
    public class ABConfigUtility
    {
        /// <summary>
        /// 获取Setting设置的字符串值
        /// </summary>
        /// <param name="key">Setting的Key</param>
        /// <param name="def">获取失败时的默认值</param>
        /// <returns>配置值</returns>
        public static string AppSettingsGetString(string key, string def = "")
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(value) ? def : value;
            }
            catch (Exception ex)
            {
                ABLogHelper.WriteLog($"配置错误,Key[{key}]:" + ex.Message, ABLogTypeEnum.Config);
                return def;
            }
        }

        /// <summary>
        /// 获取Setting设置的Int值
        /// </summary>
        /// <param name="key">Setting的Key</param>
        /// <param name="def">获取失败时的默认值</param>
        /// <returns></returns>
        public static int AppSettingsGetInt(string key, int def = 0)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return !int.TryParse(value, out var intValue) ? def : intValue;
            }
            catch (Exception)
            {
                ABLogHelper.WriteLog($"配置错误,Key[{key}]", ABLogTypeEnum.Config);
                return def;
            }
        }

        /// <summary>
        /// 获取Setting设置的Guid值
        /// </summary>
        /// <param name="key">Setting的Key</param>
        /// <returns>配置的值，如果获取失败，返回Guid.Empty</returns>
        public static Guid AppSettingsGetGuid(string key)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return !Guid.TryParse(value, out var intValue) ? Guid.Empty : intValue;
            }
            catch (Exception)
            {
                ABLogHelper.WriteLog($"配置错误,Key[{key}]", ABLogTypeEnum.Config);
                return Guid.Empty;
            }
        }
    }
}
