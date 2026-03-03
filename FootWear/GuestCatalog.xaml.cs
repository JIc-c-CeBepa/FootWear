using FootWear.Model;
using FootWear.UC;
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
    /// Логика взаимодействия для GuestCatalog.xaml
    /// </summary>
    public partial class GuestCatalog : Window
    {
        public GuestCatalog(bool isGuest)
        {
            InitializeComponent();
            //FilterSupplierBar.ItemsSource = FootwearContext.db.Manufacturers.Select(u => u.NameManufacturer).ToList();
            //FilterSupplierBar.SelectedIndex = -1;
            LoadGoods();
            if (!isGuest)
            {
                title.Content = "Каталог пользователя";
                Title = "Каталог пользователя";
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            new Auhorization().Show();
            this.Close();
        }
            
        private void LoadGoods()
        {
            using  var db = new FootwearContext(); 
            MainLW.ItemsSource = db.Goods.Include(u => u.CategoryNavigation).Include(u => u.SupplierNavigation).Include(u => u.ManufacturerNavigation).ToList();
        }

        
        
    }
}
