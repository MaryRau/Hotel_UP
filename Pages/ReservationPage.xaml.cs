﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel_UP.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReservationPage.xaml
    /// </summary>
    public partial class ReservationPage : Page
    {
        public ReservationPage()
        {
            InitializeComponent();
            DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList();
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (dateStart.SelectedDate == null && dateEnd.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату для фильтрации!");
                return;
            }

            DateTime end;
            DateTime start;

            if (dateStart.SelectedDate == null && dateEnd.SelectedDate != null)
            {
                end = (DateTime)dateEnd.SelectedDate;
                DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList().Where(x => x.DepartureDate == end);
            }

            else if (dateStart.SelectedDate != null && dateEnd.SelectedDate == null)
            {
                start = (DateTime)dateStart.SelectedDate;
                DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList().Where(x => x.EntryDate == start);
            }

            else
            {
                end = (DateTime)dateEnd.SelectedDate;
                start = (DateTime)dateStart.SelectedDate;
                DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList().Where(x => x.EntryDate == start && x.DepartureDate == end);
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            dateEnd.SelectedDate = null;
            dateStart.SelectedDate = null;
            DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var elemForRemoving = DataGridReservations.SelectedItems.Cast<ClientsInHotel>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {elemForRemoving.Count()} элементов?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().ClientsInHotel.RemoveRange(elemForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddReservationPage());
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridReservations.ItemsSource = Entities.GetContext().ClientsInHotel.ToList();
            }
        }
    }
}
