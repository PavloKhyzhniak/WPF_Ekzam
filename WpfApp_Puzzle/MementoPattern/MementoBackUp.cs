using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DP_Memento
{
    // Хранилище резервных копий
    [Serializable]
    public class MementoBackUp : IEnumerable, ISerializable//,IXmlSerializable
    {
        //        private XmlSerializer valueSerializer;
        //        private XmlSerializer keySerializer;

        [XmlAttribute]
        public XmlSerializableDictionary<(string, string, long), HashSet<IMementoBackUpObject>> Memento
        {
            get;
        } = new XmlSerializableDictionary<(string, string, long), HashSet<IMementoBackUpObject>>();

        public MementoBackUp()
        {
            //            keySerializer = new XmlSerializer(typeof((string, string, long)));
            //            valueSerializer = new XmlSerializer(typeof(HashSet<IMementoBackUpObject>));
        }

        public MementoBackUp(XmlSerializableDictionary<(string, string, long), HashSet<IMementoBackUpObject>> Memento_New)
        {
            Memento = Memento_New;
        }

        private MementoBackUp(SerializationInfo info, StreamingContext context)
        {
            int cnt_i = info.GetInt32("Keys");
            int cntInside = 0;
            for (var keys = 0; keys < cnt_i; keys++)
            {
                string key1 = info.GetString($"Key1{keys}");
                string key2 = info.GetString($"Key2{keys}");
                long key3 = info.GetInt64($"Key3{keys}");

                int cnt_j = info.GetInt32($"CountInside{keys}");

                (string, string, long) key = (key1, key2, key3);

                if (!Memento.ContainsKey(key))
                    Memento.Add(key, new HashSet<IMementoBackUpObject>());
                for (var k = 0; k < cnt_j; k++)
                {
                    string type = info.GetString($"Type{cntInside}");
                    object value = info.GetValue($"Value{cntInside}", Type.GetType(type));
                    Memento[key].Add((IMementoBackUpObject)value);
                    cntInside++;
                }
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            int keys = 0;
            int cntInside = 0;
            info.AddValue($"Keys", Memento.Keys.Count);
            foreach (var key in Memento.Keys)
            {
                info.AddValue($"Key1{keys}", key.Item1);
                info.AddValue($"Key2{keys}", key.Item2);
                info.AddValue($"Key3{keys}", key.Item3);
                if (Memento.ContainsKey(key))
                {
                    info.AddValue($"CountInside{keys}", Memento[key].Count);
                    foreach (var value in this.GetEnumerator(key))
                    {
                        info.AddValue($"Type{cntInside}", value.GetType().FullName);
                        info.AddValue($"Value{cntInside}", (IMementoBackUpObject)value);
                        cntInside++;
                    }
                }
                keys++;
            }
        }

        public IMementoBackUpObject this[(string, string, long) ID]
        {
            get
            {
                if (Memento.ContainsKey(ID))
                    return (IMementoBackUpObject)Memento[ID].Where(i => i.Data == Memento[ID].Max(j => j.Data)).LastOrDefault();
                //    throw new Exception("Key Not Found.");
                Console.WriteLine("\n-- !!! -- Данные не обнаружены -- !!! --");
                return null;
            }

            set
            {
                if (!Memento.ContainsKey(ID))
                    Memento.Add(ID, new HashSet<IMementoBackUpObject>());
                Memento[ID].Add((IMementoBackUpObject)value);
            }
        }

        //        public void Add(object item)
        //        {
        //            if (item is IMementoBackUp obj)
        //            {
        //                if (!Memento.ContainsKey(obj.ID))
        //                    Memento.Add(obj.ID, new HashSet<IMementoBackUpObject>());
        //                Memento[obj.ID].Add(obj);
        //            }
        //        }
        //        public void Add((string, string, long) key, MementoBackUpObject value)
        //        {
        //            if (!Memento.ContainsKey(key))
        //                Memento.Add(key, new HashSet<MementoBackUpObject>());
        //            Memento[key].Add((MementoBackUpObject)value);
        //        }
        /// <summary>
        /// Именованный итератор
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetEnumerator((string, string, long) ID)
        {
            if (Memento.ContainsKey(ID))
                foreach (var item in Memento[ID].OrderBy(i => i.Data))
                    yield return item;
        }

        /// <summary>
        /// Именованный итератор
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetLast()
        {
            List<IMementoBackUpObject> collection = new List<IMementoBackUpObject>();
            foreach (var item in Memento.Keys)
                if (Memento.ContainsKey(item))
                    collection.Add((IMementoBackUpObject)Memento[item].Where(i => i.Data == Memento[item].Max(j => j.Data)).LastOrDefault());
            yield return collection;
        }

        public IEnumerator GetEnumerator()
        {
            return Memento.GetEnumerator();
        }

        public void RemoveAll((string, string, long) ID)
        {
            if (Memento.ContainsKey(ID))
                Memento.Remove(ID);
        }

        public void RemoveAt((string, string, long) ID, IMementoBackUpObject obj)
        {
            if (obj != null)
                if (Memento.ContainsKey(ID))
                    Memento[ID].Remove(obj);
        }

        //        public XmlSchema GetSchema()
        //        {
        //            return null;
        //        }
        //
        //        public void ReadXml(XmlReader reader)
        //        {
        //            reader.Read();
        //            while (reader.NodeType != XmlNodeType.EndElement)
        //            {
        //                (string, string, long) key = ((string, string, long))keySerializer.Deserialize(reader);
        //                MementoBackUpObject value = (MementoBackUpObject)valueSerializer.Deserialize(reader);
        //                reader.MoveToContent();
        //                Add(key, value);
        //            }
        //        }
        //
        //        public void WriteXml(XmlWriter writer)
        //        {
        //            foreach (var key in this.Memento.Keys)
        //            {
        //                keySerializer.Serialize(writer, key);
        //                valueSerializer.Serialize(writer, this[key]);
        //            }
        //        }
    }
}



