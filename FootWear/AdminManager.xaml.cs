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
    /// Логика взаимодействия для AdminManager.xaml
    /// </summary>
    public partial class AdminManager : Window
    {
        public AdminManager(bool isAdmin)
        {
            InitializeComponent();
            InitializeComponent();
            FilterSupplierBar.ItemsSource = FootwearContext.db.Suppliers.Select(u => u.NameSupplier).ToList();
            FilterSupplierBar.SelectedIndex = -1;
            LoadGoods();
            if (isAdmin)
            {
                title.Content = "Каталог Администратора";
                Title = "Каталог Администратора";
                AddNewGood.Visibility = Visibility.Visible;
            }
        }

        private void AddNewGood_Click(object sender, RoutedEventArgs e)
        {
            new NewRedactGood().ShowDialog();

        }

        private void MainLW_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Good good = MainLW.SelectedItem as Good;
            new NewRedactGood(good).ShowDialog();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            new Auhorization().Show();
            this.Close();
        }

        private void LoadGoods()
        {
            MainLW.ItemsSource = FootwearContext.db.Goods.Include(u => u.CategoryNavigation).Include(u => u.SupplierNavigation).Include(u => u.ManufacturerNavigation).ToList();
        }

        private void SortBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void FilterSupplierBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
      {
            if (FilterSupplierBar == null)
            {
                return;
            }

            var allGoods = FootwearContext.db.Goods.Include(u => u.SupplierNavigation);
            
            var searched = allGoods.Where(ProductMatchesSearch).ToList();
            var filtered = searched;
            if (FilterSupplierBar.SelectedItem != null)
            {
                string? a = FilterSupplierBar.SelectedItem.ToString();
                filtered = searched.Where(u => u.SupplierNavigation.NameSupplier.ToString() == a).ToList();
            }
            
            string? b = SortBar.SelectedItem.ToString().Substring(38);
            if (b == "По возрастанию")
            {
                MainLW.ItemsSource = filtered.OrderBy(u => u.AmountOnStorage);
            }
            else if (b == "По убыванию")
            {
                MainLW.ItemsSource = filtered.OrderBy(u => u.AmountOnStorage).Reverse();
            }
            else
            {
                MainLW.ItemsSource = filtered.ToList();
            }
        }
        private bool ProductMatchesSearch(Good product)
        {
            if (product == null)
                return false;

            if (string.IsNullOrWhiteSpace(SearchBar.Text))
                return true;

            string search = SearchBar.Text.ToLower();

            return
                (product.NameGood?.ToLower().Contains(search) ?? false) ||
                (product.Description?.ToLower().Contains(search) ?? false) ||
                (product.ManufacturerNavigation.NameManufacturer?.ToLower().Contains(search) ?? false) ||
                (product.SupplierNavigation.NameSupplier?.ToLower().Contains(search) ?? false);
        }

        
    }
}
