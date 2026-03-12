using FootWear.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FootWear.UC
{
    /// <summary>
    /// Логика взаимодействия для GuestCatalogElementControl1.xaml
    /// </summary>
    public partial class GuestCatalogElementControl1 : UserControl
    {
        
        public GuestCatalogElementControl1()
        {
            InitializeComponent();
            DataContextChanged += GuestCatalogElementControl1_DataContextChanged;
        }

        private void GuestCatalogElementControl1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            Good good = DataContext as Good;
            if (good.CurrentDiscount > 0)
            {
                good.hasDiscount = true;
            }
            if (!good.hasDiscount)
            {
                discountPanel.Visibility = Visibility.Collapsed;
                PriceTb.Visibility = Visibility.Visible;
            }
            if (good.AmountOnStorage == 0)
            {
                Amount.Background = new SolidColorBrush(Colors.LightBlue);
            }
            if (good.CurrentDiscount >= 15)
            {
                Background = new SolidColorBrush(Color.FromRgb(46, 139, 87));

            }
            if (good.Photo == null)
            {
                goodImage.Source = new BitmapImage(new Uri("/res/picture.png", UriKind.Relative));
            }
            if(good.CategoryNavigation == null)
            {
                goodImage.Visibility = Visibility.Collapsed;
                Amount.Visibility = Visibility.Collapsed;
                Unittb.Visibility = Visibility.Collapsed;
                discountPanel.Visibility = Visibility.Collapsed;
                Discount.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                categoryGood.Text = $"{good.CategoryNavigation.NameCategory} | {good.NameGood}";
            }
            
            
        }

        public GuestCatalogElementControl1(Good good)
        {
            InitializeComponent();
            if (good.CurrentDiscount > 0)
            {
                good.hasDiscount = true;
            }
            if (!good.hasDiscount)
            {
                discountPanel.Visibility = Visibility.Collapsed;
                PriceTb.Visibility = Visibility.Visible;
            }
            if (good.AmountOnStorage == 0)
            {
                Amount.Background = new SolidColorBrush(Colors.LightBlue);
            }
            if (good.CurrentDiscount >= 15)
            {
                Background = new SolidColorBrush(Color.FromRgb(46, 139, 87));

            }
            if (good.Photo == null)
            {
                goodImage.Source = new BitmapImage(new Uri("/res/picture.png", UriKind.Relative));
            }
            categoryGood.Text = $"{good.CategoryNavigation.NameCategory} | {good.NameGood}";
            DataContext = good;
        }
    }
}
