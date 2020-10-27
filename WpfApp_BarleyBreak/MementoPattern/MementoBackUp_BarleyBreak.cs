using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DP_Memento
{
    // Контейнер, позволяющий хранить данные (резервная копия)
    [Serializable]
    [XmlInclude(typeof(DateTime))]
    public class MementoBackUp_BarleyBreak : IMementoBackUpObject
    {
        public string[] Files { get; set; } = new string[0];

        // Constructor
        public MementoBackUp_BarleyBreak(string[] files)
        {
            this.Files = files;

            Data = DateTime.Now;//DateTime.UtcNow;
        }

        [XmlElement(DataType = "date")]
        public DateTime Data { get; set; }

        public string Show()
        {
            StringBuilder str = new StringBuilder(1000);
            foreach (var filename in Files)
                str.Append(filename + "\n");
            return str.ToString();
        }

        public void XmlBuild(ref XmlWriter writer)
        {
            // записать начальный тег элемента
            writer.WriteStartElement("Value");

            // запись атрибута
            writer.WriteAttributeString("Date", $"{Data}");
            for (int i=0;i<Files.Length;i++)
                writer.WriteAttributeString($"Filename{i}", $"{Files[i]}");

            // закрытие элемента Value
            writer.WriteEndElement();
        }

        public void XmlRebuild(StringDictionary arg)
        {
            List<string> list = new List<string>();
            if (arg.ContainsKey("filename0") && arg.ContainsKey("date"))
            {
                Data = DateTime.Parse(arg["date"]);

                string key;
                int i = 0;
                while (arg.ContainsKey(key = "filename" + i.ToString()))
                {
                    list.Add(arg[key]);
                    i++;
                }
                Files = list.ToArray();
            }
            else
                throw new NotImplementedException();
        }


    }
}



