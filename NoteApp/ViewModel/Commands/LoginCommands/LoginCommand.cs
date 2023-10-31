using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteApp.Model;

namespace NoteApp.ViewModel.Commands.LoginCommands
{
    public class LoginCommand : ICommand
    {
        public LoginCommand(LoginViewModel loginVm)
        {
            LoginVM = loginVm;
        }

        public LoginViewModel LoginVM { get; set; }
        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;

            if (user == null ) { return false; }
            if (string.IsNullOrEmpty(user.Username)) { return false; }
            if (string.IsNullOrEmpty(user.Password)) { return false; }

            return true;
        }

        public void Execute(object? parameter)
        {
            LoginVM.Login();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
