using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace DP_Memento
{
    // Контейнер, позволяющий хранить данные (резервная копия)
    [Serializable]
    [XmlInclude(typeof(DateTime))]
    public class MementoBackUp_BarleyBreak : IMementoBackUpObject
    {
        //для сохранения состояния игры, нам понадобятся такие поля
        //time - время игры
        internal int time { get; set; }
        //SoundOnOff - включение звуков
        internal bool SoundOnOff { get; set; }
        //MusicOnOff - включение музыки
        internal bool MusicOnOff { get; set; }
        //array - массив расположения пятнашек на игровом поле
        internal int[][] array { get; set; }//массив номеров элементов на игровом поле
        //flag_image - игра с картинкой или цифрами
        internal bool flag_image { get; set; }
        //ImageFilename - имя файла картинки
        internal string ImageFilename { get; set; }
        //Count - количество перетасовок
        internal int Count { get; set; }//установим количество перемешиваний
        //rows - количество строк на игровом поле
        internal int rows { get; set; }
        //columns - количество столбцов на игровом поле
        internal int columns { get; set; }
        //PlayerName - имя игрока
        internal string PlayerName { get; set; }

        // Constructor
        public MementoBackUp_BarleyBreak()
        {
            Data = DateTime.Now;//DateTime.UtcNow;
        }

        [XmlElement(DataType = "date")]
        public DateTime Data { get; set; }

        public void XmlBuild(ref XmlWriter writer)
        {
            // записать начальный тег элемента
            writer.WriteStartElement("State_BarleyBreak");



            // запись атрибута
            writer.WriteAttributeString("Date", $"{Data}");


            // запись атрибута
            //time - время игры
            writer.WriteAttributeString("time", $"{time}");
            // запись атрибута
            //SoundOnOff - включение звуков
            writer.WriteAttributeString("SoundOnOff", $"{SoundOnOff}");
            // запись атрибута
            //MusicOnOff - включение музыки
            writer.WriteAttributeString("MusicOnOff", $"{MusicOnOff}");
            // запись атрибута
            //array - массив расположения пятнашек на игровом поле
            writer.WriteAttributeString($"array_rows", $"{array.GetLength(0)}");
            writer.WriteAttributeString($"array_columns", $"{array[0].GetLength(0)}");
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array[i].GetLength(0); j++)
                    writer.WriteAttributeString($"array_{i * array[i].GetLength(0) + j}", $"{array[i][j]}");
            // запись атрибута
            //flag_image - игра с картинкой или цифрами
            writer.WriteAttributeString("flag_image", $"{flag_image}");
            // запись атрибута
            //ImageFilename - имя файла картинки
            writer.WriteAttributeString("ImageFilename", $"{ImageFilename}");
            // запись атрибута
            //Count - количество перетасовок
            writer.WriteAttributeString("Count", $"{Count}");
            // запись атрибута
            //rows - количество строк на игровом поле
            writer.WriteAttributeString("rows", $"{rows}");
            // запись атрибута
            //columns - количество столбцов на игровом поле
            writer.WriteAttributeString("columns", $"{columns}");
            // запись атрибута
            //PlayerName - имя игрока
            writer.WriteAttributeString("PlayerName", $"{PlayerName}");



            // закрытие элемента Value
            writer.WriteEndElement();
        }

        public void XmlRebuild(StringDictionary arg)
        {
            if (arg.ContainsKey("date"))
                Data = DateTime.Parse(arg["date"]);

            if (arg.ContainsKey("time"))
                time = int.Parse(arg["time"]);
            if (arg.ContainsKey("SoundOnOff"))
                SoundOnOff = bool.Parse(arg["SoundOnOff"]);
            if (arg.ContainsKey("MusicOnOff"))
                MusicOnOff = bool.Parse(arg["MusicOnOff"]);

            if (arg.ContainsKey("flag_image"))
                flag_image = bool.Parse(arg["flag_image"]);
            if (arg.ContainsKey("ImageFilename"))
                ImageFilename = arg["ImageFilename"];
            if (arg.ContainsKey("Count"))
                Count = int.Parse(arg["Count"]);
            if (arg.ContainsKey("rows"))
                rows = int.Parse(arg["rows"]);
            if (arg.ContainsKey("columns"))
                columns = int.Parse(arg["columns"]);
            if (arg.ContainsKey("PlayerName"))
                PlayerName = arg["PlayerName"];

            if (arg.ContainsKey("array_rows") && arg.ContainsKey("array_columns"))
            {
                int array_rows = int.Parse(arg["array_rows"]);
                int array_columns = int.Parse(arg["array_columns"]);

                //создадим массив
                array = new int[array_rows][];
                for (int k = 0; k < array_rows; k++)
                    array[k] = new int[array_columns];

                string key;
                int i = 0;
                while (arg.ContainsKey(key = "array_" + i.ToString()))
                {
                    array[i / array_columns][i % array_columns] = int.Parse(arg[key]);
                    i++;
                }

                if (i != (array_rows * array_columns))
                    throw new NotImplementedException();
            }
            else
                throw new NotImplementedException();
        }


    }
}



