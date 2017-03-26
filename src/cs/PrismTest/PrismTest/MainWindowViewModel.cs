using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrismTest
{
    public class MainWindowViewModel:BindableBase
    {
        private InteractionRequest<ConfirmModel> confirm = new InteractionRequest<ConfirmModel>();

        public IInteractionRequest Confirm
        {
            get { return confirm; }
        }


        public ICommand AlertCommand => new DelegateCommand(AlertCommandExecute, AlertCommandCanExecute);

        private string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }


        private bool AlertCommandCanExecute() => true;

        private void AlertCommandExecute()
        {
            confirm.Raise(new ConfirmModel()
            {
                Title = "hello",
                Content = "hello contents.",
                Name = "confirm name",
                Value = "confirm value"
            }, n =>
            {
                this.Message = n.Confirmed ? "OKが押されました" : "キャンセルが押されました";
            });
        }
    }

}
