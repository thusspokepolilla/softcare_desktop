using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace softcare_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
            string Connect = "server=localhost; user=root; database=";
            MySqlConnection mySqlConnection = new MySqlConnection(Connect);
            try
            {
                mySqlConnection.Open();
            }
            catch
            {

            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Login Successful!");
            DoctorPage doctor = new DoctorPage();
            doctor.Show();
            //AdminPage admin = new AdminPage();
            //admin.Show();
            this.Close();
            
        }
    }
}