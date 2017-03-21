using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Insight.Validation.Report.Strategies
{
    public static class EmbeddReportStrategies
    {
        static List<IReportStrategy> _strategies = new List<IReportStrategy>();

        static EmbeddReportStrategies()
        {
            IReportStrategy defaultStrategy = new ReportDefaultStrategy();
            Type dstype = typeof(IReportStrategy);
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach(Type t in asm.GetTypes())
            {
                if (t.Namespace != dstype.Namespace)
                    continue;
                Type targetInterfaceType = t.GetInterface("IReportStrategy", true);
                if(targetInterfaceType == dstype && !t.IsInterface)
                {
                    _strategies.Add((IReportStrategy)Activator.CreateInstance(t));
                }
            }
            _strategies.TrimExcess();
        }
        public static int Count => _strategies.Count;

        public static IEnumerable<IReportStrategy> GetEnumerator()
        {
            foreach (IReportStrategy rs in _strategies)
                yield return rs;
        }
        public static string[] GetNames()
        {
            string[] names = new string[Count];
            for (int i = 0; i < Count; i++)
                names[i] = _strategies[i].Name;
            return names;
        }
        public static IReportStrategy Find(string name, bool ignore = true)
        {
            if (ignore)
                name = name.ToLower();
            foreach(IReportStrategy rs in _strategies)
            {
                string sname = (ignore) ? rs.Name.ToLower() : rs.Name;
                if (sname == name)
                    return rs;
            }
            return null;
        }
    }
}
