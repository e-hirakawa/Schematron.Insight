﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Schematron.Validator.Mvvm.Models
{
    public class DocumentXmlCollection :ObservableCollection<DocumentXmlModel>
    {
        public DocumentXmlCollection() :base()
        {
            BindingOperations.EnableCollectionSynchronization(this, new object());
        }
        public bool Exists(string path)
        {
            foreach(DocumentModel model in this)
            {
                if (model.FullPath == path)
                    return true;
            }
            return false;
        }
        public void Add(string path)
        {
            this.Add(new DocumentXmlModel(path));
        }
    }
}
