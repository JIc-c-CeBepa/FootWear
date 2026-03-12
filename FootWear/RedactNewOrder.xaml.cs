using FootWear.Model;
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
    /// Логика взаимодействия для RedactNewOrder.xaml
    /// </summary>
    public partial class RedactNewOrder : Window
    {
        int idOrder;
        bool isEdit = false;
        List<PickUpPoint> pickUpPoints;
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
            pickUpPoints = db.PickUpPoints.ToList();
            AdressPickUpPoint.ItemsSource = pickUpPoints;//.Select(u=>u.FullAddress).ToList();
            ClientId.ItemsSource = db.Users.Select(u => u.Iduser).ToList();
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
            using var db = new FootwearContext();
            DateStartPicker.SelectedDate = order.DateStartOrder.HasValue
                ? order.DateStartOrder.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null;

            DateDeliverPicker.SelectedDate = order.DateDeliver.HasValue
                ? order.DateDeliver.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null;

            ArtickleOrder.Text = order.ArtikleList.ToString();
            PickUpPoint a = pickUpPoints.FirstOrDefault(u => u.IdPickUpPint == order.PickUpPointAdressNavigation.IdPickUpPint);
            AdressPickUpPoint.SelectedIndex = AdressPickUpPoint.Items.IndexOf(a);
            
            StatusOrder.SelectedIndex = StatusOrder.Items.IndexOf(order.Status);
            ClientId.SelectedItem = order.ClientId;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddOrder(idOrder);
            this.Close();
        }

        private void AddOrder(int idOrder)
        {
            using var db = new FootwearContext();
            Order order = new Order();

            if (isEdit)
            {
                order = db.Orders.FirstOrDefault(u => u.Idorder == idOrder);
                if (order == null) return; 
            }
            else
            {
                
                db.Orders.Add(order);
                db.SaveChanges();
            }

            order.DateStartOrder = DateOnly.FromDateTime(DateStartPicker.SelectedDate.Value);
            order.DateDeliver = DateOnly.FromDateTime(DateDeliverPicker.SelectedDate.Value);
            order.PickUpPointAdress = db.PickUpPoints
                .FirstOrDefault(q => q == AdressPickUpPoint.SelectedItem as PickUpPoint)
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

            
            List<OrderItem> newItems = artikleList.Select(a => new OrderItem
            {
                IdOrder = order.Idorder,
                Articke = a, 
                Amount = 1
            }).ToList();

            foreach(var a in newItems)
            {
                db.OrderItems.Add(a);
                db.SaveChanges();
            }

            

            
        }
    }

}
