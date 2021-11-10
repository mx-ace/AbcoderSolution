using System;
using System.IO;
using System.Xml.Serialization;

namespace Abcoder.Tools.ABUtility
{
    /// <summary>
    /// xml数据转换类
    /// </summary>
    public class ABXmlSerializer
    {
        #region 反序列化
        /// <summary>
        /// 反序列化 string(xml)=>T
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="xmlscript">反序列化的xml脚本</param>
        /// <returns>序列化的数据对象</returns>
        public static T Deserialize<T>(string xmlscript) where T : class
        {
            return Deserialize(xmlscript, typeof(T)) as T;
        }
        /// <summary>
        /// 反序列化 string(xml)=>obj
        /// </summary>
        /// <param name="xmlscript">反序列化的xml文本</param>
        /// <param name="type">反序列化的xml文本</param>
        /// <returns></returns>
        public static object Deserialize(string xmlscript, Type type)
        {
            using (StringReader sr = new StringReader(xmlscript))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream) where T : class
        {
            stream.Position = 0;
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return xmldes.Deserialize(stream) as T;
        }
        #endregion

        #region 序列化

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ns">设置xml的命名空间</param>
        /// <returns>序列化的字符串</returns>
        public static string Serializer(object obj, XmlSerializerNamespaces ns = null)
        {
            if (ns == null)
            {
                ns = new XmlSerializerNamespaces();
                //去除命名空间
                ns.Add("", "");
            }
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xmlxs = new XmlSerializer(obj.GetType());
            //序列化对象
            xmlxs.Serialize(Stream, obj, ns);
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion
    }
}
