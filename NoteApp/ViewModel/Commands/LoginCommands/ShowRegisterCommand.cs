using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteApp.ViewModel.Commands.LoginCommands
{
    public class ShowRegisterCommand : ICommand
    {
        public ShowRegisterCommand(LoginViewModel loginVm)
        {
            LoginVM = loginVm;
        }

        public LoginViewModel LoginVM { get; set; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            LoginVM.SwitchViews();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
