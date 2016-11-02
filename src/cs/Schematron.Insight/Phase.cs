using System;
using System.Collections.Generic;

namespace Schematron.Insight
{
    public class Phase
    {
        public static string ALL = "#ALL";
        public static string DEFAULT = "#DEFAULT";
        public string Id { get; set; } = "";
        public List<string> Patterns { get; set; } = new List<string>();
        public bool IsEnabled => !String.IsNullOrWhiteSpace(Id) && Patterns.Count > 0;

    }
}
