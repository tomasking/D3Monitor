using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace D3Monitor.SubscriptionService
{
    public interface IXmlFileAccess<T>
    {
        T Load(string fileName);
        List<T> LoadMultiple(string fileName);
        void Save(T item, string fileName);
    }

    public class XmlFileAccess<T> : IXmlFileAccess<T>
    {
        public object fileLock = new object();

        public T Load(string fileName)
        {
            lock (fileLock)
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Position = 0;
                    using (var file = new StreamReader(fs))
                    {
                        var reader = new XmlSerializer(typeof (T));
                        T service = ((T) reader.Deserialize(file));
                        file.Close();
                        return service;
                    }
                }
            }
        }

        public List<T> LoadMultiple(string fileName)
        {
            lock (fileLock)
            {
                var reader = new XmlSerializer(typeof (T[]));
                var file = new StreamReader(fileName);
                var services = ((T[]) reader.Deserialize(file)).ToList();
                file.Close();
                file.Dispose();
                return services;
            }
        }

        public void Save(T item, string fileName)
        {
            lock (fileLock)
            {
                var serializer = new XmlSerializer(typeof (T));
                TextWriter writer = new StreamWriter(fileName, false);
                serializer.Serialize(writer, item);
                writer.Close();
            }
        }

    }


}
