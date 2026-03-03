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
    /// Логика взаимодействия для RedactNewOrder.xaml
    /// </summary>
    public partial class RedactNewOrder : Window
    {
        int idOrder;
        bool isEdit = false;
        public RedactNewOrder()
        {
            InitializeComponent();
            Title = "Добавление нового заказа";
            DateStartPicker.DisplayDateStart = DateTime.Today;
            DateDeliverPicker.DisplayDateStart = DateTime.Today;
            LoadCb();
        }

        public RedactNewOrder(Order order)
        {
            InitializeComponent();
            isEdit = true;
            idOrder = order.Idorder;
            Title = "Редактирование заказа";
            DateStartPicker.DisplayDateStart = DateTime.Today;
            DateDeliverPicker.DisplayDateStart = DateTime.Today;
            LoadCb();
            LoadInfo(order);

        }

        private void LoadCb()
        {
            using var db = new FootwearContext();
            StatusOrder.ItemsSource = db.Orders.Select(u=> u.Status).Distinct().ToList();
            AdressPickUpPoint.ItemsSource = db.PickUpPoints.Select(u=>u.FullAddress).ToString();
            ClientId.ItemsSource = db.Users.Select(u => u.Iduser);
        }

        private void DateStartPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateStartPicker.SelectedDate == null) return;

            
            if (DateStartPicker.SelectedDate.Value.Date < DateTime.Today)
                DateStartPicker.SelectedDate = DateTime.Today;

            
            DateDeliverPicker.DisplayDateStart = DateStartPicker.SelectedDate.Value.Date;

            if (DateDeliverPicker.SelectedDate != null &&
                DateDeliverPicker.SelectedDate.Value.Date < DateStartPicker.SelectedDate.Value.Date)
            {
                DateDeliverPicker.SelectedDate = DateStartPicker.SelectedDate.Value.Date;
            }
        }

        private void DateDeliverPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateDeliverPicker.SelectedDate == null) return;

            var minDeliver = (DateStartPicker.SelectedDate ?? DateTime.Today).Date;

            // дата выдачи не раньше даты заказа (или сегодня, если заказ не выбран)
            if (DateDeliverPicker.SelectedDate.Value.Date < minDeliver)
                DateDeliverPicker.SelectedDate = minDeliver;
        }

        private void LoadInfo(Order order)
        {
            DateStartPicker.SelectedDate = order.DateStartOrder.HasValue
                ? order.DateStartOrder.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null;

            DateDeliverPicker.SelectedDate = order.DateDeliver.HasValue
                ? order.DateDeliver.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null;

            ArtickleOrder.Text = order.ArtikleList.ToString();
            AdressPickUpPoint.SelectedIndex = AdressPickUpPoint.Items.IndexOf(order.PickUpPointAdress);
            StatusOrder.SelectedIndex = StatusOrder.Items.IndexOf(order.Status);

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddOrder(idOrder);
        }

        private void AddOrder(int idOrder)
        {
            using var db = new FootwearContext();
            Order order;

            if (isEdit)
            {
                order = db.Orders.FirstOrDefault(u => u.Idorder == idOrder);
                if (order == null) return; 
            }
            else
            {
                order = new Order();
                db.Orders.Add(order);
            }

            order.DateStartOrder = DateOnly.FromDateTime(DateStartPicker.SelectedDate.Value);
            order.DateDeliver = DateOnly.FromDateTime(DateDeliverPicker.SelectedDate.Value);
            order.PickUpPointAdress = db.PickUpPoints
                .First(q => q.FullAddress == AdressPickUpPoint.SelectedItem.ToString())
                .IdPickUpPint;

            order.ClientId = Convert.ToInt32(ClientId.SelectedItem.ToString());
            order.Status = StatusOrder.SelectedItem.ToString();

            
            if (!isEdit)
            {
                var lastCode = db.Orders
                    .OrderByDescending(o => o.Idorder)
                    .Select(o => o.RecieveCode)
                    .FirstOrDefault();

                int codeNum = 0;
                int.TryParse(lastCode, out codeNum);
                order.RecieveCode = (codeNum + 1).ToString();
            }

            
            List<string> artikleList = ArtickleOrder.Text
                .Split(',')
                .Select(a => a.Trim())
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Distinct() 
                .ToList();

            
            db.SaveChanges();

            
            if (isEdit)
            {
                var oldItems = db.OrderItems
                    .Where(i => i.IdOrder == order.Idorder);

                db.OrderItems.RemoveRange(oldItems);
                db.SaveChanges();
            }

            
            var newItems = artikleList.Select(a => new OrderItem
            {
                IdOrder = order.Idorder,
                Articke = a, 
                Amount = 1
            });

            db.OrderItems.AddRange(newItems);

            db.SaveChanges();
        }
    }

}
