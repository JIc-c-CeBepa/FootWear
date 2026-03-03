using FootWear.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FootWear
{
    /// <summary>
    /// Логика взаимодействия для Auhorization.xaml
    /// </summary>
    public partial class Auhorization : Window
    {
        
        public Auhorization()
        {
            InitializeComponent();
            FootwearContext db = new FootwearContext();
            db.Users.Any();
        }

        private void auth_Click(object sender, RoutedEventArgs e)
        {
            FootwearContext db = new FootwearContext();
            if (String.IsNullOrWhiteSpace(password.Text)|| String.IsNullOrWhiteSpace(login.Text))
            {
                MessageBox.Show("Заполните поля");
                return;
            }
            
            User user = db.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefault(u => u.Login == login.Text && u.Password == password.Text);
            if (user != null)
            {
                if(user.RoleNavigation.NameRole == "Авторизированный клиент")
                {
                    MessageBox.Show("Добро пожаловать", "Успешно", MessageBoxButton.OK);
                    new GuestCatalog(false).Show();
                    
                    this.Close();
                }
                else if(user.RoleNavigation.NameRole == "Администратор")
                {
                    MessageBox.Show("Добро пожаловать", "Успешно", MessageBoxButton.OK);
                    new AdminManager(true).Show();
                    
                    this.Close();
                }
                else if (user.RoleNavigation.NameRole == "Менеджер")
                {
                    MessageBox.Show("Добро пожаловать", "Успешно", MessageBoxButton.OK);
                    new AdminManager(false).Show();
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Добро пожаловать","Успешно",MessageBoxButton.OK);
                    new GuestCatalog(true).Show();
                    
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
                return;
            }
                



        }

        private void guest_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Добро пожаловать", "Успешно", MessageBoxButton.OK);
            new GuestCatalog(true).Show();
            
            this.Close();
        }
    }
}
