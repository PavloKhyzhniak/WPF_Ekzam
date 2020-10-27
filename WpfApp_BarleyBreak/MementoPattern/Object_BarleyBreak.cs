using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DP_Memento
{

    // Класс, данные которого нужно сохранять
    public class Object_BarleyBreak : IMementoBackUp
    {
        public string[] Files { get; set; } = new string[0];

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
            return (IMementoBackUpObject)new MementoBackUp_BarleyBreak(Files);
        }

        public void Get(IMementoBackUpObject obj)
        {
            if (obj is MementoBackUp_BarleyBreak concret_object)
            {
                Console.WriteLine("\nRestoring state --\n");
                this.Files = concret_object.Files;
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



