using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Util
{
    public static class XmlHelper
    {
        public static string Serialize<T>(T dataToSerialize, XmlSerializerNamespaces namespaces = null)
        {
            try
            {
                var s = new MemoryStream();
                var sw = new StreamWriter(s);
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                if (namespaces != null)
                    serializer.Serialize(sw, dataToSerialize, namespaces);
                else
                    serializer.Serialize(sw, dataToSerialize);

                var xml = Encoding.UTF8.GetString(s.ToArray());

                return xml;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }
    }
}