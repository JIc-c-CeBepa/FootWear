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
            MainLW.ItemsSource = FootwearContext.db.Goods.Include(u => u.CategoryNavigation).Include(u => u.SupplierNavigation).Include(u => u.ManufacturerNavigation);
        }

        //private void SortBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ApplyFilter();
        //}

        //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    ApplyFilter();
        //}

        //private void FilterSupplierBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ApplyFilter();
        //}

        //private void ApplyFilter()
        //{
        //    if (FilterSupplierBar == null)
        //    {
        //        return;
        //    }
            
        //    var allGoods = FootwearContext.db.Goods.Include(u => u.SupplierNavigation);
        //    var searched = allGoods.Where(ProductMatchesSearch);
        //    var filtered = searched;
        //    if(FilterSupplierBar.SelectedItem != null)
        //    {
        //        filtered = searched.Where(u => u.SupplierNavigation.NameSupplier == FilterSupplierBar.SelectedItem);
        //    }
            
        //    if(SortBar.SelectedItem.ToString() == "По возрастанию")
        //    {
        //        MainLW.ItemsSource = filtered.OrderBy(u=> u.AmountOnStorage);
        //    }
        //    else if (SortBar.SelectedItem.ToString() == "По убыванию")
        //    {
        //        MainLW.ItemsSource = filtered.OrderBy(u => u.AmountOnStorage).Reverse();
        //    }
        //    else
        //    {
        //        MainLW.ItemsSource = filtered.ToList();
        //    }
            
                

        //}
        //private bool ProductMatchesSearch(Good product)
        //{
        //    if (product == null)
        //        return false;

        //    if (string.IsNullOrWhiteSpace(SearchBar.Text))
        //        return true;

        //    string search = SearchBar.Text.ToLower();
            
        //    return
        //        (product.NameGood?.ToLower().Contains(search) ?? false) ||
        //        (product.Description?.ToLower().Contains(search) ?? false) ||
        //        (product.ManufacturerNavigation.NameManufacturer?.ToLower().Contains(search) ?? false) ||
        //        (product.SupplierNavigation.NameSupplier?.ToLower().Contains(search) ?? false);
        //}
        
    }
}
