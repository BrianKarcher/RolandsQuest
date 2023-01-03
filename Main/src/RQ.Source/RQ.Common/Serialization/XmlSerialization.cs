using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace RQ.Common.Serialization
{
    public static class XmlSerialization
    {
        public static T Deserialize<T>(string filename) where T : class, new()
        {
            XmlSerializer serializer = null;
            //FileStream stream = null;
            serializer = new XmlSerializer(typeof(T));
            //stream = new FileStream(filename, FileMode.Open);
            var encoding = Encoding.UTF8;
            using (StreamReader stream = new StreamReader(filename, encoding))
            {
                return (T)serializer.Deserialize(stream);
            }

            //return null;
        }

        public static void Serialize<T>(string filename, T obj) where T : class
        {
            XmlSerializer serializer = null;
            //FileStream stream = null;
            serializer = new XmlSerializer(typeof(T));
            //stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            var encoding = Encoding.UTF8;
            using (StreamWriter stream = new StreamWriter(filename, false, encoding))
            {
                serializer.Serialize(stream, obj);
            }
        }
    }
}
