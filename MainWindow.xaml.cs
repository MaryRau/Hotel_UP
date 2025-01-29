using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Hotel_UP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateStatus.UpdateRoomStatus();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is Page page)) return;

            if (page is Pages.ReservationPage)
                btnBack.Visibility = Visibility.Hidden;
            else
                btnBack.Visibility = Visibility.Visible;
        }
    }
}
