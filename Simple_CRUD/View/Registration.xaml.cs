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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public string LoginField { get; set; }
        public string NameField { get; set; }
        public RelayCommand RegisterCommand { get; private set; }

        private readonly string passwordsNotEquals = "Пароли не совпадают!";
        private readonly string fieldIsEmpty = "Необходимо заполнить все поля!";
        private readonly string loginIsBusy = "Этот логин занят!";

        private readonly Context context;

        public Registration(AuthUser user)
        {
            context = new Context(user);
            RegisterCommand = new RelayCommand(RegisterHandler);

            InitializeComponent();
            DataContext = this;
        }

        private void RegisterHandler()
        {
            var passF = passwordFirstOne.Password.Trim();
            var passS = passwordSecondOne.Password.Trim();

            if (passF == "" || passS == "" || LoginField == "" || NameField == "")
            {
                ErrorMessageField.Text = fieldIsEmpty;
                return;
            }

            if (passF != passS)
            {
                ErrorMessageField.Text = passwordsNotEquals;
                return;
            }

            if(context.Users.FirstOrDefault(u => u.Login == LoginField) != null)
            {
                ErrorMessageField.Text = loginIsBusy;
                return;
            }

            context.Users.Add(new User
            {
                Id = context.Users.Count() == 0 ? 1 : context.Users.Select(u => u.Id).Max() + 1,
                Name = NameField,
                Login = LoginField,
                PasswordHash = Cryptographer.Encrypt(passF)
            });



            context.SaveChanges();

            MessageBox.Show($"Пользователь \"{NameField}\" успешно создан!", "Пользователь создан", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            
            Close();
        }
    }
}
