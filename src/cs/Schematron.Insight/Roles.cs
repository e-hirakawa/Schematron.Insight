using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Schematron.Insight
{
    public class Roles
    {

    }
    [Serializable()]
    [DataContract]
    public class Role
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; } = "";
        [XmlIgnore]
        [IgnoreDataMember]
        public RoleColor ForeColor { get; set; } = new RoleColor();
        [XmlIgnore]
        [IgnoreDataMember]
        public RoleColor BackColor { get; set; } = new RoleColor();
        [XmlIgnore]
        [IgnoreDataMember]
        public bool IsEnabled => !String.IsNullOrWhiteSpace(Name);

        public override string ToString() => Name;

    }
    public class RoleColor
    {
        public int R { get; set; } = -1;
        public int G { get; set; } = -1;
        public int B { get; set; } = -1;
        public int A { get; set; } = 255;
        public RoleColor(int r, int g, int b, int a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public RoleColor(int r, int g, int b) : this(r, g, b, 255) { }
        public RoleColor(byte r, byte g, byte b, byte a) : this((int)r, (int)g, (int)b, (int)a) { }
        public RoleColor(byte r, byte g, byte b) : this((int)r, (int)g, (int)b) { }
        public RoleColor() : this(-1, -1, -1, 255) { }
        public bool IsEnabled => R >= 0 && G >= 0 && B >= 0;
    }
}
