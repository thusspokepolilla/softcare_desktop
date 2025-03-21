using System.Windows;

namespace softcare_desktop
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        public AdminPage()
        {
            InitializeComponent();
            this.DataContext = new AdminViewmodel(); // initialize current page
            tbkAdminName.Text = UserDetails.Username;
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
