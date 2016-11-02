using System.Text.RegularExpressions;

namespace schematron.tester.cui
{
    /// <summary>
    /// 実行時パラメータアイテム
    /// </summary>
    public class ArgumentItem
    {
        public static string OPERATOR = "=";
        public string[] Keys { get; set; } = new string[] { };
        public string Name { get; set; } = "";
        public bool IsRequired { get; set; } = false;
        public bool IsOmitValue { get; set; } = false;
        public string[] TrustOfValues = new string[] { };
        public string Description { get; set; } = "";
        public object Value { get; set; } = null;
        public bool IsMatch(string arg)
        {
            foreach (string key in Keys)
            {
                string pattern = key + (!IsOmitValue ? OPERATOR + "(?<v>.*)" : "");
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match mc = regex.Match(arg);
                if (mc.Success)
                {
                    if (!IsOmitValue)
                    {
                        Value = mc.Groups["v"].Value;
                    }
                    return true;
                }
            }
            return false;
        }
        public bool ContainsTrust()
        {
            string value = Value.ToString().ToLower();
            foreach (string item in TrustOfValues)
                if (item.ToLower() == value)
                    return true;
            return false;
        }
    }

}
