using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Schematron.Insight.Validation
{
    [Serializable()]
    [DataContract]
    public class ResultCollection : IEnumerable<Result>
    {
        private List<Result> _items = new List<Result>();

        #region Public Members
        [XmlIgnore]
        [IgnoreDataMember]
        public Result this[int index]
        {
            get
            {
                return index > -1 && index < _items.Count ? _items[index] : null;
            }
            set
            {
                if (index < 0 || index >= _items.Count)
                    throw new IndexOutOfRangeException();
                _items[index] = value;
            }
        }
        [XmlIgnore]
        [IgnoreDataMember]
        public int Count => _items.Count;

        [XmlIgnore]
        [IgnoreDataMember]
        public int TotalSyntaxError => GetTotal(ResultStatus.SyntaxError);
        [XmlIgnore]
        [IgnoreDataMember]
        public int TotalAssert => GetTotal(ResultStatus.Assert);
        [XmlIgnore]
        [IgnoreDataMember]
        public int TotalReport => GetTotal(ResultStatus.Report);
        private int GetTotal(ResultStatus state) => _items.Count((item) => { return item.Status == state; });
        [XmlIgnore]
        [IgnoreDataMember]
        public bool IsValid => _items.Count == 0;
        #endregion

        #region Public Methods
        public Result Find(int index) => this[index];
        public Result Find(Result target) => _items.Find((item) => { return target == item; });
        public List<Result> FindAll(Predicate<Result> match) => _items.FindAll(match);
        public bool Exists(Result target) => _items.Exists((item) => { return target == item; });
        public void Add(Result result) => _items.Add(result);
        public void AddRange(IEnumerable<Result> results) => _items.AddRange(results);
        public bool Remove(Result result) => _items.Remove(result);
        public void RemoveAt(int index) => _items.RemoveAt(index);
        public void Clear() => _items.Clear();
        #endregion

        #region IEnumerable
        public IEnumerator<Result> GetEnumerator()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        #endregion
    }

}
