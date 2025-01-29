using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Hotel_UP.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddReservationPage.xaml
    /// </summary>
    public partial class AddReservationPage : Page
    {
        private ClientsInHotel _reservation = new ClientsInHotel();

        public AddReservationPage()
        {
            InitializeComponent();

            cmbRoom.ItemsSource = Entities.GetContext().RoomFond.ToList();
            cmbRoom.DisplayMemberPath = "NumCategory";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (dateEntry.SelectedDate == null)
                errors.AppendLine("Укажите дату заезда!");
            if (dateEnd.SelectedDate != null && dateEntry.SelectedDate != null && dateEnd.SelectedDate <= dateEntry.SelectedDate)
                errors.AppendLine("Дата выезда не может быть раньше или соответствовать дате заезда!");
            if (string.IsNullOrWhiteSpace(txtSecName.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtPatr.Text))
                errors.AppendLine("Укажите ФИО гостя!");
            if (cmbRoom.SelectedValue == null)
                errors.AppendLine("Укажите комнату!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var room = (RoomFond)cmbRoom.SelectedItem;
            DateTime entryDate = dateEntry.SelectedDate.Value;
            DateTime? endDate = dateEnd.SelectedDate;
            Clients _client = new Clients();

            var reservRooms = Entities.GetContext().ClientsInHotel.Where(x => x.Number == room.ID &&
                            ((x.EntryDate <= entryDate && x.DepartureDate >= entryDate) ||
                             (x.EntryDate <= endDate && x.DepartureDate >= endDate) ||
                             (x.EntryDate >= entryDate && x.DepartureDate <= endDate))).ToList();

            if (reservRooms.Count > 0)
            {
                MessageBox.Show("Выбранный номер занят в указанный период!");
                return;
            }

            if (Entities.GetContext().Clients.Where(x => x.SecondName == txtSecName.Text && x.FirstName == txtFirstName.Text && x.Patronimic == txtPatr.Text).Count() == 0)
            {
                _client.SecondName = txtSecName.Text;
                _client.FirstName = txtFirstName.Text;
                _client.Patronimic = txtPatr.Text;
                Entities.GetContext().Clients.Add(_client);
                try
                {
                    Entities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            else
                _client = Entities.GetContext().Clients.Where(x => x.SecondName == txtSecName.Text && x.FirstName == txtFirstName.Text && x.Patronimic == txtPatr.Text).FirstOrDefault();

            _reservation.Number = room.ID;
            _reservation.Client = _client.ID;
            _reservation.EntryDate = entryDate;
            _reservation.DepartureDate = endDate;

            if (_reservation.ID == 0) 
                Entities.GetContext().ClientsInHotel.Add(_reservation);

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Бронь добавлена");
                UpdateStatus.UpdateRoomStatus();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
