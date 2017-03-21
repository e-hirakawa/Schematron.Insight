using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Schematron.Validator.Mvvm.Models
{
    public class XmlResourceCollection :ObservableCollection<XmlResourceModel>
    {
        public XmlResourceCollection() :base()
        {
            BindingOperations.EnableCollectionSynchronization(this, new object());
        }
        public bool Exists(string path)
        {
            foreach(ResourceModel model in this)
            {
                if (model.FullPath == path)
                    return true;
            }
            return false;
        }
        public void Add(string path)
        {
            this.Add(new XmlResourceModel(path));
        }
    }
}
