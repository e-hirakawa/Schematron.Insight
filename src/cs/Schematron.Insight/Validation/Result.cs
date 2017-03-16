using Schematron.Insight.Utilities;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Schematron.Insight.Validation
{
    [Serializable()]
    [DataContract]
    public class Result
    {
        private FileInfo _xmlfile;
        private FileInfo _schfile;
        [XmlIgnore]
        public FileInfo XmlFile
        {
            get { return _xmlfile; }
            set
            {
                if (_xmlfile != value)
                {
                    _xmlfile = value;
                    XmlName = value.Name;
                }
            }
        }
        [XmlIgnore]
        public FileInfo SchFile
        {
            get { return _schfile; }
            set
            {
                if (_schfile != value)
                {
                    _schfile = value;
                    SchName = value.Name;
                }
            }
        }

        [XmlElement(ElementName = "XmlFile", Order = 1)]
        [DataMember(Name = "XmlFile", Order = 1)]
        public string XmlName { get; set; } = "";
        [XmlElement(ElementName = "SchemaFile", Order = 2)]
        [DataMember(Name = "SchemaFile", Order = 2)]
        public string SchName { get; set; } = "";
        [XmlIgnore]
        [IgnoreDataMember]
        public ResultStatus Status { get; set; } = ResultStatus.None;
        [XmlElement(ElementName = "Status", Order = 3)]
        [DataMember(Name = "Status", Order = 3)]
        public string StatusName
        {
            get
            {
                return Status.DisplayName();
            }
            set
            {
                Status = EnumerationTypeHelper.GetValueFromDisplayName<ResultStatus>(value);
            }
        }
        [XmlElement(ElementName = "Location", Order = 4)]
        [DataMember(Name = "Location", Order = 4)]
        public string Location { get; set; } = "";
        [XmlElement(ElementName = "Line", Order = 5)]
        [DataMember(Name = "Line", Order = 5)]
        public int Line { get; set; } = -1;
        [XmlElement(ElementName = "Pos", Order = 6)]
        [DataMember(Name = "Pos", Order = 6)]
        public int Pos { get; set; } = -1;

        [XmlElement(ElementName = "Message", Order = 7)]
        [DataMember(Name = "Message", Order = 7)]
        public string Message { get; set; } = "";
        [XmlElement(ElementName = "Test", Order = 8)]
        [DataMember(Name = "Test", Order = 8)]
        public string Test { get; set; } = "";
        [XmlElement(ElementName = "Role", Order = 9)]
        [DataMember(Name = "Role", Order = 9)]
        public Role Role { get; set; } = new Role();
    }

}
