using FootWear.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        bool isEdit;
        string Artikle;
        
        private byte[] photoBytes;
        public NewRedactGood()
        {
            InitializeComponent();
            ArtickleGood.Text = "Редактирование не возможно";
            ArtickleGood.IsEnabled = false; 
            Title = "Добавление нового товара";
            isEdit = false;
            LoadCBs();
        }
        public NewRedactGood(Good good)
        {
            InitializeComponent();
            ArtickleGood.Text = "Редактирование не возможно";
            ArtickleGood.IsEnabled = false;
            Title = "Редактирование товара";
            isEdit = true;
            LoadCBs();
            Artikle = good.Artikle;
            LoadInfo(good);

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ImageGood.Source == null)
            {
                MessageBox.Show("Добавьте изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(NameGood.Text))
            {
                MessageBox.Show("Напишите название товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CategoryGood.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(DescriptionGood.Text))
            {
                MessageBox.Show("Напишите описание товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SupplierGood.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите Поставщика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ManufacturerGood.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите Производителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (decimal.TryParse(PriceGood.Text, out decimal price))
            {
                if (price <= 0)
                {
                    MessageBox.Show("Цена должна быть больше 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Цена должна быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (UnitGood.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите единицу измерения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (int.TryParse(AmountOnSrtorageGood.Text, out int amount))
            {
                if (amount < 0)
                {
                    MessageBox.Show("Количество на складе не должно быть меньше 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Количество должно быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (int.TryParse(CurrentDiscountGood.Text, out int discout))
            {
                if (discout < 0)
                {
                    MessageBox.Show("Размер скидки не должен быть меньше 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Размер скидки должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            AddGood(price, amount, discout, isEdit);
        }

        private void AddGood(decimal price, int amount, int discount, bool isEdit )
        {
            Good newGood = new Good();
            if (isEdit)
            {
                newGood = FootwearContext.db.Goods.FirstOrDefault(u=> u.Artikle == Artikle);
            }
            else
            {
                newGood.Artikle = GenerateArtickle();
                FootwearContext.db.Goods.Add(newGood);
            }

            newGood.Photo = photoBytes;
            newGood.NameGood = NameGood.Text;
            newGood.Category = FootwearContext.db.Categories.First(u => u.NameCategory == CategoryGood.SelectedItem.ToString()).Idcategory;
            newGood.Description = DescriptionGood.Text;
            newGood.Manufacturer = FootwearContext.db.Manufacturers.First(q => q.NameManufacturer == ManufacturerGood.SelectedItem.ToString()).Idmanufacturer;
            newGood.Supplier = FootwearContext.db.Suppliers.First(q => q.NameSupplier == SupplierGood.SelectedItem.ToString()).Idsupplier;
            newGood.Price = price;
            newGood.Unit = UnitGood.SelectedItem.ToString();
            newGood.AmountOnStorage = amount;
            newGood.CurrentDiscount = discount;

            FootwearContext.db.SaveChanges();
            MessageBox.Show("Сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            this.Close();
        }

        private static readonly Random random = new Random();

        private string GenerateArtickle()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string article;

            do
            {
                article = new string(Enumerable
                    .Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
            }
            while (FootwearContext.db.Goods.Any(g => g.Artikle == article));

            return article;
        }

        private void ImageGood_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                
                if (bitmap.PixelWidth > 300 || bitmap.PixelHeight > 200)
                {
                    MessageBox.Show("Размер изображения не должен превышать 200x300 пикселей!",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                photoBytes = bytes;
                ImageGood.Source = bitmap;
            }
        }
        
        public void LoadCBs()
        {
            //категория производитель поставщик единица измерения
            CategoryGood.ItemsSource = FootwearContext.db.Categories.Select(u => u.NameCategory).ToList();
            SupplierGood.ItemsSource = FootwearContext.db.Suppliers.Select(u => u.NameSupplier).ToList();
            ManufacturerGood.ItemsSource = FootwearContext.db.Manufacturers.Select(u => u.NameManufacturer).ToList();
            UnitGood.ItemsSource = FootwearContext.db.Goods.Select(u => u.Unit).Distinct().ToList();
            UnitGood.SelectedIndex = 0;
            UnitGood.IsEnabled = false;
        }

        public void LoadInfo(Good good) 
        {
            if(good.Photo == null)
            {
                ImageGood.Source = new BitmapImage(new Uri("/res/picture.png", UriKind.Relative));
            }
            else 
            {
                using (var ms = new MemoryStream(good.Photo))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze(); 

                    ImageGood.Source = image;
                }
                photoBytes = good.Photo;
            }
            ArtickleGood.Text = good.Artikle;
            NameGood.Text = good.NameGood;
            CategoryGood.SelectedIndex = CategoryGood.Items.IndexOf(good.CategoryNavigation.NameCategory);
            DescriptionGood.Text = good.Description;
            ManufacturerGood.SelectedIndex = ManufacturerGood.Items.IndexOf(good.ManufacturerNavigation.NameManufacturer);
            SupplierGood.SelectedIndex = SupplierGood.Items.IndexOf(good.SupplierNavigation.NameSupplier);
            PriceGood.Text = good.Price.ToString();
            CurrentDiscountGood.Text = good.CurrentDiscount.ToString();
            AmountOnSrtorageGood.Text = good.AmountOnStorage.ToString();

        }

    }
}
