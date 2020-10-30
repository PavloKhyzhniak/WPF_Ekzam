using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DP_Memento
{
    [Serializable]
    [XmlRoot("Dictionary")]// для какова класса пишем сереализацыю/десереализацыю
    public class XmlSerializableDictionary<TKey, TValue>//создаем класс-наследник Dictionary с данным интерфейсом
       : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public XmlSerializableDictionary() { }

        protected XmlSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public System.Xml.Schema.XmlSchema GetSchema()//зарезервированный метод. должен возвращать null 
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)//при десериализации передаем сюда XmlReader
        {                                               // как его передовать и тд. не ясно =(

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }
}



