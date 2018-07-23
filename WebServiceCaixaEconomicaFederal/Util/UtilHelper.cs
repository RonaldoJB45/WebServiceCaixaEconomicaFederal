using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace WebServiceCaixaEconomicaFederal.Util
{
    public static class UtilHelper
    {
        public static string CortaPalavra(this string text, int tamanho)
        {
            return text.Length > tamanho ? text.Substring(0, tamanho) : text;
        }
        public static string GetHashSha256(string text)
        {
            var bytesText = Encoding.UTF8.GetBytes(text);
            var hasher = SHA256.Create();
            var hashValue = hasher.ComputeHash(bytesText);
            return Convert.ToBase64String(hashValue);
        }
        
        public static string CallWebRequest(string url, string xml, string operacao)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(xml);
            HttpWebRequest webRequest = CreateWebRequest(url, operacao);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            try
            {
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        return soapResult;
                    }
                }
            }
            catch (WebException err)
            {
                using (var stream = err.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string erro = reader.ReadToEnd();                        
                        throw new Exception(erro);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}