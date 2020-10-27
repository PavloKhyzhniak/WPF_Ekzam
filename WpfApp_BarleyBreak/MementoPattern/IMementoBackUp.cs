using System.Dynamic;

namespace DP_Memento
{
    /// <summary>
    /// Interface for Memento Methods
    /// </summary>
    public interface IMementoBackUp
    {
        //уникальный идентификатор объекта сохранения
        (string,string,long) ID { get; set; }
        //уникальный номер объекта сохранения в своей иерархии однотипных экземпляров
        long IDnumber { get; }

        void Set(object obj);
        //сохранить объект
        IMementoBackUpObject Put();
        //восстановить объект
        void Get(IMementoBackUpObject obj);
    }
}



