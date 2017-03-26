using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismTest
{
    public class ConfirmModel:BindableBase, INotification, IConfirmation
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name , value); }
        }
        private string _value;

        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value,  value); }
        }

        public string Title { get; set; } = "";

        public object Content { get; set; } = "";

        public bool Confirmed { get; set; } = true;
    }
}
