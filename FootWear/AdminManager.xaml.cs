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
            using var db = new FootwearContext();
            foreach (var item in db.Suppliers.Select(u => u.NameSupplier).ToList()) 
            {
                FilterSupplierBar.Items.Add(item);
            }
            
            
            LoadGoods();
            if (isAdmin)
            {
                title.Content = "Каталог Администратора";
                Title = "Каталог Администратора";
                AddNewGood.Visibility = Visibility.Visible;
                Orders.Visibility = Visibility.Visible;
            }
        }

        private void AddNewGood_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewRedactGood();

            if (window.ShowDialog() == true)
            {
                LoadGoods(); 
            }
        }

        private void MainLW_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainLW.SelectedItem is Good good)
            {
                var window = new NewRedactGood(good);

                if (window.ShowDialog() == true)
                {
                    LoadGoods();
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            new Auhorization().Show();
            this.Close();
        }

        private void LoadGoods()
        {
            using var db = new FootwearContext();
            MainLW.ItemsSource = db.Goods.Include(u => u.CategoryNavigation).Include(u => u.SupplierNavigation).Include(u => u.ManufacturerNavigation).ToList();
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
            Good good = new Good();
            good.Description = "Товаров не найдено";
            List<Good> def = new List<Good>();
            def.Add(good);

            
            if (FilterSupplierBar == null || MainLW == null)
            {
                return;
            }
            using var db = new FootwearContext();
            var allGoods = db.Goods
                .Include(u => u.CategoryNavigation)
                .Include(u => u.SupplierNavigation)
                .Include(u => u.ManufacturerNavigation)
                .ToList();
            
            var searched = allGoods.Where(ProductMatchesSearch).ToList();
            
            var filtered = searched;
            if (FilterSupplierBar.SelectedIndex != 0)
            {
                string? a = FilterSupplierBar.SelectedItem.ToString();
                filtered = searched.Where(u => u.SupplierNavigation.NameSupplier.ToString() == a).ToList();
            }
            
            string? b = SortBar.SelectedItem.ToString().Substring(38);
            if (b == "По возрастанию")
            {

                MainLW.ItemsSource = filtered.OrderBy(u => u.AmountOnStorage).ToList();
                if (filtered.OrderBy(u => u.AmountOnStorage).ToList().Count == 0)
                {
                    MainLW.ItemsSource = def;
                }
            }
            else if (b == "По убыванию")
            {
                MainLW.ItemsSource = filtered.OrderBy(u => u.AmountOnStorage).Reverse().ToList();
                if (filtered.OrderBy(u => u.AmountOnStorage).Reverse().ToList().Count == 0)
                {
                    MainLW.ItemsSource = def;
                }
            }
            else
            {
                MainLW.ItemsSource = filtered.ToList();
                if (filtered.ToList().Count == 0)
                {
                    MainLW.ItemsSource = def;
                }
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

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            new Orders().Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FootwearContext();
            if (MainLW.SelectedItem is Good good)
            {
                if (db.OrderItems.FirstOrDefault(u=> u.Articke == good.Artikle) != null)
                {
                    MessageBox.Show("Товар находиться в коризине", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if(MessageBox.Show("Вы точно хотите удалить товар?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        db.Goods.Remove(good);
                        MessageBox.Show("Товар удален", "Успешно", MessageBoxButton.OK);
                        db.SaveChanges();
                        LoadGoods();
                    }
                    else
                    {
                        return;
                    }
                    
                }
            }
        }
    }
}
