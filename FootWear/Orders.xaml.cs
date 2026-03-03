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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        public Orders()
        {
            InitializeComponent();
            LoadOrdes();
        }

        private void LoadOrdes()
        {
            using var db = new FootwearContext();
            MainLW.ItemsSource = db.Orders.Include(u => u.PickUpPointAdressNavigation).Include(o => o.OrderItems).ToList();
            //Order order = FootwearContext.db.Orders.Include(u => u.PickUpPointAdressNavigation).ToList().First();
            //List<OrderItem> orderItem = FootwearContext.db.OrderItems.Where(u => u.IdOrder == order.Idorder).ToList();
            //string a = string.Join(",", orderItem.Select(i => i.Articke));

        }

        private void AddNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var window = new RedactNewOrder();

            if (window.ShowDialog() == true)
            {
                LoadOrdes();
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            new AdminManager(true).Show();
            this.Close();
            
        }

        private void MainLW_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainLW.SelectedItem is Order  order)
            {
                var window = new RedactNewOrder(order);

                if (window.ShowDialog() == true)
                {
                    LoadOrdes();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FootwearContext();
            if (MainLW.SelectedItem is Order order)
            {
                var items = db.OrderItems
                    .Where(i => i.IdOrder == order.Idorder);

                db.OrderItems.RemoveRange(items);
                db.Orders.Remove(order);

                db.SaveChanges();
                LoadOrdes();
            }
            
        }
    }
}
