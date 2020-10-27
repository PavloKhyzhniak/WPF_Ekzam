using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DP_Memento
{
    public class XML
    {
        //XmlSerializableDictionary<(string, string, long), HashSet<IMementoBackUpObject>>
        public static bool Save(MementoBackUp memento, string filename)
        {
            // класс для записи XML
            XmlWriter writer;

            // настройки записи
            XmlWriterSettings settings = new XmlWriterSettings
            {
                // перенос на новые строки
                Indent = true,
                // символы перехода на следующую строку
                NewLineChars = "\r\n",
                // кодировка
                Encoding = Encoding.ASCII,
                // переход на новую строку для атрибутов
                NewLineOnAttributes = false
            };

            // создание нового файла
            writer = XmlWriter.Create(filename, settings);

            // вывести заголовок XML
            writer.WriteStartDocument();

            // записать начальный тег элемента
            writer.WriteStartElement("Memento");
                        
            // запись коментария xml
            writer.WriteComment("Save All Object in BackUp");

            foreach (var keys in memento.Memento.Keys)
            {
                // записать начальный тег элемента
                writer.WriteStartElement("Key");

                // запись атрибута
                writer.WriteAttributeString("KeyValue1", $"{keys.Item1}");
                writer.WriteAttributeString("KeyValue2", $"{keys.Item2}");
                writer.WriteAttributeString("KeyValue3", $"{keys.Item3}");
                
                foreach (IMementoBackUpObject value in memento.GetEnumerator(keys))
                    value.XmlBuild(ref writer);
                
                // закрытие элемента Key
                writer.WriteEndElement();
            }

            // закрыть элемент 
            writer.WriteEndElement();

            // закрытие записи всего документа
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
            return true;
        }

        public class StringArrayEqualityComparer : IEqualityComparer<string[]>
        {
            public bool Equals(string[] x, string[] y)
            {
                bool result = true;

                for (int i = 0; i < x.Length; i++)
                    result &= (x[i] == y[i]);

                return result;
            }
            public int GetHashCode(string[] obj)
            {
                int hash = 13;

                foreach (var item in obj)
                    hash = (hash * 13) + item.GetHashCode();

                return obj[0].GetHashCode();
            }
        }

        public static MementoBackUp Load(string filename) 
        {
            MementoBackUp memento = new MementoBackUp();
            StringDictionary arg = new StringDictionary();
            (string, string, long) key = (null, null, 0);
              // объект для чтения XML
              XmlReader reader;

            // открытие существующего файла
            reader = XmlReader.Create(filename);

            // читать файл по одному тегу
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Whitespace)
                    continue;

                // проверка разных типов узлов XML
                switch (reader.NodeType)
                {
                    case XmlNodeType.Whitespace:
                    //                continue;
                    case XmlNodeType.Comment:
                        continue;
                    case XmlNodeType.Element:
                    case XmlNodeType.XmlDeclaration:
                        arg.Clear();
                        // если есть атрибуты в элементе
                        if (reader.HasAttributes)
                        {
                            // переместить указатель на чтение атрибутов
                            reader.MoveToFirstAttribute();
                            arg.Clear();
                            // вывод данных текущего атрибута
                            arg.Add(reader.Name, reader.Value);

                            // вывод информации обо всех атрибутах элемента
                            while (reader.MoveToNextAttribute())
                                arg.Add(reader.Name, reader.Value);

                            // переместить указатель на элементы
                            reader.MoveToElement();                          
                        }
                        if (reader.Name.Equals("Key"))
                            key = (arg["KeyValue1"], arg["KeyValue2"], long.Parse(arg["KeyValue3"]));
                        else
                            if ((key.Item1 != null)
                            && (key.Item2 != null)
                            && (key.Item3 != 0))
                            {
                                Type info = Type.GetType(key.Item1);
                                // создаем экземпляр класса
                                object obj = Activator.CreateInstance(info);
                                object obj_backup = ((IMementoBackUp)obj).Put();
                                ((IMementoBackUpObject)obj_backup).XmlRebuild(arg);
    
                                //Create Correct Object by Reflection and Save Object in Collection
                                memento[key] = (IMementoBackUpObject)obj_backup;//сохраним в коллекции
                            }
                        continue;
                    case XmlNodeType.Text:
                        //Parse Data
                        continue;
                    case XmlNodeType.EndElement:
                         break;
                }
            }

            reader.Close();

            return memento;
        }

//        public static void Show<T, V>(string filename) where T : class, IData, new() where V : class, IData, new()
//        {
//            // объект для чтения XML
//            XmlReader reader;
//
//            // открытие существующего файла
//            reader = XmlReader.Create(filename);
//
//            int cnt = -1;
//
//            // читать файл по одному тегу
//            while (reader.Read())
//            {
//                if (reader.NodeType == XmlNodeType.Whitespace)
//                    continue;
//
//                Console.Write("\n");
//                for (int i = 0; i < cnt; i++)
//                    Console.Write("\t");
//
//                // проверка разных типов узлов XML
//                switch (reader.NodeType)
//                {
//                    case XmlNodeType.Whitespace:
//                        continue;
//                    case XmlNodeType.Comment:
//                        Console.Write("<!--" + reader.Value + " -->");
//                        continue;
//                    case XmlNodeType.Element:
//                    case XmlNodeType.XmlDeclaration:
//                        // если есть атрибуты в элементе
//                        if (reader.HasAttributes)
//                        {
//                            Console.Write("<" + reader.Name + " ");
//
//                            // переместить указатель на чтение атрибутов
//                            reader.MoveToFirstAttribute();
//
//                            // вывод данных текущего атрибута
//                            Console.Write(reader.Name + " = " + reader.Value + " ");
//
//                            // вывод информации обо всех атрибутах элемента
//                            while (reader.MoveToNextAttribute())
//                            {
//                                Console.Write(reader.Name + " = " + /*reader.Value*/reader[reader.Name] + " ");
//                            }
//
//                            // переместить указатель на элементы
//                            reader.MoveToElement();
//
//                            // проверка на наличие дочерних элементов
//                            if (reader.IsEmptyElement)
//                                Console.Write("/>");
//                            else
//                            {
//                                Console.Write(">");
//                                cnt++;
//                            }
//                        }
//                        else
//                        {
//                            // проверка на наличие дочерних элементов
//                            if (reader.IsEmptyElement)
//                            {
//                                Console.Write("<" + reader.Name + "/>");
//                            }
//                            else
//                            {
//                                Console.Write("<" + reader.Name + ">");
//                                //Console.Write("\r\n");
//                                cnt++;
//                            }
//                        }
//                        continue;
//                    case XmlNodeType.Text:
//                        Console.Write(reader.Value);
//                        continue;
//                    case XmlNodeType.EndElement:
//                        Console.Write("\r");
//                        cnt--;
//                        for (int i = 0; i < cnt; i++)
//                            Console.Write("\t");
//                        Console.Write("</" + reader.Name + ">");
//                        break;
//                }
//            }
//            Console.WriteLine();
//        }

        public static bool SaveDocument(MementoBackUp memento, string filename)
        {
            // создание нового XML-документа
            XmlDocument document = new XmlDocument();

            // создание и добавление заголовка документа
            XmlDeclaration decl = document.CreateXmlDeclaration("1.0", "US-ASCII", null);
            document.AppendChild(decl);

            // добавление дополнительных отступов и переходов на новую строку
            XmlWhitespace ws = document.CreateWhitespace("\r\n");

            // создание корневого элемента
            XmlElement elem1 = document.CreateElement("Memento");
            XmlNode node1 = document.AppendChild(elem1);

            elem1.AppendChild(ws);

            //    // добавление текстового узла
            //    XmlText text1 = document.CreateTextNode("<!--Save All Vertex of current Graph-->");
            //    elem1.AppendChild(text1);

            // добавление комментария
            XmlComment comment1 = document.CreateComment("Save All Vertex of current Graph");
            elem1.AppendChild(comment1);

            // добавление элемента с атрибутом
            XmlElement elem2;
            XmlAttribute attr1;
            foreach (var key in memento.Memento.Keys)
            {
                // добавление элемента с атрибутом
                elem2 = document.CreateElement("Object");

                attr1 = document.CreateAttribute("Key");
                attr1.InnerText = $"{key}";
                // добавление атрибута к элементу
                elem2.Attributes.Append(attr1);
                
                elem2.AppendChild(ws);

                elem1.AppendChild(elem2);
            }
                                   
            // сохрание документа Xml при помощи XmlTextWriter
            XmlTextWriter writer = new XmlTextWriter(filename, null)
            {
                // настройки форматирования
                Formatting = Formatting.Indented,
                Indentation = 2
            };
            document.WriteTo(writer);
            writer.Flush();
            writer.Close();

            return true;
        }

        public static void ShowDocument(string filename)
        {
            // создание пустого документа
            XmlDocument document = new XmlDocument();

            // чтение из файла
            document.Load(filename);

            // рекурсивный парсинг
            PrintNode(document);
        }

        private static void PrintNode(XmlNode node1, int cnt = 0)
        {
            // обработка всех дочерних узлов
            foreach (XmlNode node in node1.ChildNodes)
            {
                Console.Write("\n");
                for (int i = 0; i < cnt; i++)
                    Console.Write("\t");

                // проверка разных типов узлов XML
                switch (node.NodeType)
                {
                    case XmlNodeType.Whitespace:
                        continue;
                    case XmlNodeType.Comment:
                        Console.Write("<!--" + node.Value + " -->");
                        continue;
                    case XmlNodeType.Element:
                        // проверка на наличие атрибутов
                        if (node.Attributes.Count > 0)
                        {
                            string str = "<" + node.Name + " ";

                            // обработка атрибутов
                            foreach (XmlAttribute attr in node.Attributes)
                            {
                                str += attr.Name + " = " + attr.Value + " ";
                            }
                            Console.Write(str + ">");
                        }
                        else Console.Write("< " + node.Name + " >");

                        if (node.ChildNodes.Count == 0)
                        {
                            Console.Write("\b/>");
                            continue;
                        }
                        // рекурсивная обработка дочерних элементов
                        PrintNode(node, ++cnt);

                        Console.Write("\r\n");
                        cnt--;
                        for (int i = 0; i < cnt; i++)
                            Console.Write("\t");
                        Console.Write("</" + node.Name + ">");

                        continue;
                    case XmlNodeType.Text:
                        Console.Write(node.Value);
                        continue;
                }

            }
        }
    }
}



