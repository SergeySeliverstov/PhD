using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Tools
{
    public class XmlTools
    {
        public static T Load<T>(string fileName)
        {
            T obj = default(T);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(fileName))
                {
                    obj = (T)serializer.Deserialize(reader);
                }
            }
            catch { }

            return obj;
        }

        public static void Save<T>(string fileName, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, obj);
            }
        }
    }
}
