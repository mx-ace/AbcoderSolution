using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace Abcoder.Tools.ABUtility
{
    /// <summary>
    /// 网络帮助类
    /// </summary>
    public  class ABHttpHelper
    {
        /// <summary>
        /// Http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string HttpGet(string url, int timeout = 30000)
        {
            try
            {
                string retString = "";
                //创建Request对象
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.Timeout = timeout;
                request.ReadWriteTimeout = 30000;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //输出响应流
                    Stream stream = response.GetResponseStream();
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        retString = streamReader.ReadToEnd().ToString();
                    }
                }
                return retString;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string HttpPost(string url, string postData, string contentType, NameValueCollection header = null, int timeout = 30000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = "POST";
            request.Timeout = timeout;
            if (header != null)
            {
                AddHeaders(request, header);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            response.Close();
            return result;
        }

        public static bool AddHeaders(HttpWebRequest request, NameValueCollection header)
        {
            request.Headers.Clear();
            foreach (var item in header.AllKeys)
            {
                try
                {
                    var h_value = header.Get(item);
                    switch (item)
                    {
                        case "Connection":
                            {
                                if (h_value == "Keep-Alive")
                                    request.KeepAlive = true;
                                else
                                    request.Connection = h_value;
                                break;
                            }
                        case "Accept":
                            {
                                request.Accept = h_value;
                                break;
                            }
                        case "Content-Type":
                            {
                                request.ContentType = h_value;
                                break;
                            }
                        case "Expect":
                            {
                                request.Expect = h_value;
                                break;
                            }
                        case "Host":
                            {
                                request.Host = h_value;
                                break;
                            }
                        case "User-Agent":
                            {
                                request.UserAgent = h_value;
                                break;
                            }
                        case "Referer":
                            {
                                request.Referer = h_value;
                                break;
                            }
                        default:
                            {
                                request.Headers.Add(item, h_value);
                                break;

                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
            return true;
        }
    }
}
