using System.Windows;

namespace softcare_desktop
{
    /// <summary>
    /// Interaction logic for DoctorPage.xaml
    /// </summary>
    public partial class DoctorPage : Window
    {
        public DoctorPage()
        {
            InitializeComponent();
            this.DataContext = new DoctorViewmodel();
            tbkDoctorName.Text = UserDetails.Username;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                LoginScreen login = new LoginScreen();
                login.Show();
                this.Close();
            }
        }
    }
}
