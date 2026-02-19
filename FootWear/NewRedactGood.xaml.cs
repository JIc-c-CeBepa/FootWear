using FootWear.Model;
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
    /// Логика взаимодействия для NewRedactGood.xaml
    /// </summary>
    public partial class NewRedactGood : Window
    {
        public NewRedactGood()
        {
            InitializeComponent();
            Title = "Добавление нового товара";
            LoadCBs();
        }
        public NewRedactGood(Good good)
        {
            InitializeComponent();
            Title = "Редактирование товара";
            LoadCBs();
            LoadInfo(good);

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImageGood_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void LoadCBs()
        {
            //категория производитель поставщик единица измерения
            CategoryGood.ItemsSource = FootwearContext.db.Categories.Select(u => u.NameCategory).ToList();
            SupplierGood.ItemsSource = FootwearContext.db.Suppliers.Select(u => u.NameSupplier).ToList();
            ManufacturerGood.ItemsSource = FootwearContext.db.Manufacturers.Select(u => u.NameManufacturer).ToList();
            UnitGood.ItemsSource = FootwearContext.db.Goods.Select(u => u.Unit).Distinct().ToList();
        }
        public void LoadInfo(Good good) 
        {
            if(good.Photo == null)
            {
                ImageGood.Source = new BitmapImage(new Uri("/res/picture.png", UriKind.Relative));
            }
        }

    }
}
