using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DP_Memento
{

    // Класс, данные которого нужно сохранять
    public class Object_BarleyBreak : IMementoBackUp
    {      
        //для сохранения состояния игры, нам понадобятся такие поля
        //time - время игры
        public int time { get; set; } = 0;
        //SoundOnOff - включение звуков
        public bool SoundOnOff { get; set; } = true;
        //MusicOnOff - включение музыки
        public bool MusicOnOff { get; set; } = true;
        //array - массив расположения пятнашек на игровом поле
        public int[][] array { get; set; }//массив номеров элементов на игровом поле
        //flag_image - игра с картинкой или цифрами
        public bool flag_image { get; set; } = false;
        //ImageFilename - имя файла картинки
        public string ImageFilename { get; set; } = "\\Resources\\yellow_breakfast.jpg";
        //Count - количество перетасовок
        public int Count { get; set; } = 10;//установим количество перемешиваний
        //rows - количество строк на игровом поле
        public int rows { get; set; } = 4;
        //columns - количество столбцов на игровом поле
        public int columns { get; set; } = 4;
        //PlayerName - имя игрока
        public string PlayerName { get; set; }


        //количество созданных элементов
        static private long cnt = 0;
        public Object_BarleyBreak()
        {
            //присвоим уникальный номер экземпляру данного класса
            IDnumber = ++cnt;
            ID = (this.GetType().FullName, "", IDnumber);
        }
              
        //уникальный номер экземпляра класса
        public long IDnumber { get; }
        //уникальный идентификатор экземпляра класса
        public (string, string, long) ID
        {
            get;
            set;
        }

        public IMementoBackUpObject Put()
        {
            Console.WriteLine("\nSaving state --\n");
            return (IMementoBackUpObject)(new MementoBackUp_BarleyBreak()
            {
                time = time,
                SoundOnOff = SoundOnOff,
                MusicOnOff = MusicOnOff,
                array = array,//(int[][])array.Clone(),
                flag_image = flag_image,
                ImageFilename = ImageFilename,
                Count = Count,
                rows = rows,
                columns = columns,
                PlayerName = PlayerName
            });
        }

        public void Get(IMementoBackUpObject obj)
        {
            if (obj is MementoBackUp_BarleyBreak concret_object)
            {
                Console.WriteLine("\nRestoring state --\n");
                this.time = concret_object.time;
                this.SoundOnOff = concret_object.SoundOnOff;
                this.MusicOnOff = concret_object.MusicOnOff;
                this.array = concret_object.array;// (int[][])concret_object.array.Clone();
                this.flag_image = concret_object.flag_image;
                this.ImageFilename = concret_object.ImageFilename;
                this.Count = concret_object.Count;
                this.rows = concret_object.rows;
                this.columns = concret_object.columns;
                this.PlayerName = concret_object.PlayerName;
            }
        }

        public void Set(object obj)
        {
     //       if (obj is TabControl tabControl)
     //       {
     //           Files = new string[tabControl.TabPages.Count];
     //           int i = 0;
     //           foreach (var tabElement in tabControl.TabPages)
     //           {
     //               if (tabElement is TabPage tabPage)
     //               {
     //                   if (tabPage.Tag is FileInfo info)
     //                       Files[i++] = info.FullName;
     //               }
     //           }
     //       }
        }
    }
}



