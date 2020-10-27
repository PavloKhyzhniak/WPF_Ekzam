using System;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;

namespace DP_Memento
{
    /// <summary>
    /// Interface for XmlSerialization Object in BackUp
    /// </summary>
    public interface IXmlMementoSerialization
    {
        //сохранить объект
        void XmlBuild(ref XmlWriter writer);
        //восстановить объект
        void XmlRebuild(StringDictionary arg);
    }

    /// <summary>
    /// Interface for Memento Object for BackUp Data
    /// </summary>
    public interface IMementoBackUpObject:IXmlMementoSerialization
    {
        [XmlElement(DataType = "date")]
        DateTime Data { get; set; }

        string Show();
    }

//    [Serializable]
//    public abstract class MementoBackUpObject : IMementoBackUpObject
//    {
//        abstract public DateTime Data { get; set; } 
//    }
}



