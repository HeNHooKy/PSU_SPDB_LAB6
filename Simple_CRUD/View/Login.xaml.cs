using CRUD.Tools;
using Microsoft.EntityFrameworkCore.Internal;
using Simple_CRUD.Model;
using Simple_CRUD.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simple_CRUD.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string LoginField { get; set; }
        public RelayCommand LoginCommand { get; private set; }

        private readonly Context context;
        private readonly string incorrectUserPassword = "Неправильный логин или пароль!";
        private readonly string loginFieldEmpty = "Введите ваш логин!";
        private readonly string passwordFieldEmpty = "Введите ваш пароль!";
        private readonly AuthUser user;
        private readonly MainMenu mainMenu;

        public Login(MainMenu mainMenu, AuthUser user)
        {
            this.mainMenu = mainMenu;
            this.user = user;
            context = new Context(user);
            LoginCommand = new RelayCommand(LoginHandler);

            InitializeComponent();
            DataContext = this;
        }

        private void LoginHandler()
        {
            if(LoginField == null || LoginField.Trim() == "")
            {
                ErrorMessageField.Text = loginFieldEmpty;
                return;
            }

            if (password.Password == null || password.Password.Trim() == "")
            {
                ErrorMessageField.Text = passwordFieldEmpty;
                return;
            }

            var login = context.Users.FirstOrDefault(u => u.Login == LoginField);

            if (login == null || !Cryptographer.Equals(password.Password.Trim(), login.PasswordHash))
            {
                ErrorMessageField.Text = incorrectUserPassword;
                return;
            }

            user.Approved = true;
            user.Name = login.Name;
            user.Id = login.Id;
            mainMenu.AuthNewUser();

            Close();
        }

        
    }
}
