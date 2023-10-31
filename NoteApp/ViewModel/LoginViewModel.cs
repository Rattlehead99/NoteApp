using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NoteApp.Model;
using NoteApp.ViewModel.Commands.LoginCommands;
using NoteApp.ViewModel.Helpers;

namespace NoteApp.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private User _user;
        private bool _isShowingRegister = false;
        private Visibility _loginVisibility;
        private Visibility _registerVisibility;
        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _lastName;
        private string _name;

        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this);
            RegisterCommand = new RegisterCommand(this);
            ShowRegisterCommand = new ShowRegisterCommand(this);

            LoginVisibility = Visibility.Visible;
            RegisterVisibility = Visibility.Collapsed;

            User = new User();
        }

        public Visibility LoginVisibility
        {
            get { return _loginVisibility; }
            set
            {
                _loginVisibility = value;
                OnPropertyChanged(nameof(LoginVisibility));
            }
        }

        public Visibility RegisterVisibility
        {
            get { return _registerVisibility; }
            set
            {
                _registerVisibility = value;
                OnPropertyChanged(nameof(RegisterVisibility));
            }
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                User = new User
                {
                    Username = _username,
                    Password = this.Password
                };
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                User = new User
                {
                    Username = this.Username,
                    Password = _password
                };
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (value == _confirmPassword) return;
                _confirmPassword = value;
                User = new User
                {
                    Username = this.Username,
                    Password = this.Password,
                    Name = this.Name,
                    LastName = this.LastName,
                    ConfirmPassword = _confirmPassword
                };
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                User = new User
                {
                    Username = this.Username,
                    Password = this.Password,
                    LastName = _lastName
                };
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                User = new User
                {
                    Username = this.Username,
                    Password = this.Password,
                    LastName = this.LastName,
                    Name = _name
                };
                OnPropertyChanged(nameof(Name));
            }
        }

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }
        

        public void SwitchViews()
        {
            _isShowingRegister = !_isShowingRegister;

            if (_isShowingRegister)
            {
                RegisterVisibility = Visibility.Visible;
                LoginVisibility = Visibility.Collapsed;
                return;
            }

            RegisterVisibility = Visibility.Collapsed;
            LoginVisibility = Visibility.Visible;
        }

        public async void Login()
        {
            bool result = await FirebaseAuthHelper.Login(this.User);

            if (result)
            {
                Authenticated?.Invoke(this, new EventArgs());
            }
        }

        public async void Register()
        {
            bool result = await FirebaseAuthHelper.Register(this.User);

            if (result)
            {
                Authenticated?.Invoke(this, new EventArgs());
            }
        }


        public event EventHandler Authenticated;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
