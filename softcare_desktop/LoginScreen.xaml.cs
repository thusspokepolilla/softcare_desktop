using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

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
            string testString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection test = new MySqlConnection(testString);
            try
            {
                test.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                Environment.Exit(1);
            }
            finally
            {
                test.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // string username, password, pass_hash;
            // username = "ingalclarence2004";
            // // password = "doctor5678";
            // pass_hash = "$2a$13$VGG2dSFqx9BJxVBiCXA0QuzgIsRExg6Psxsy8uZGza13Nemwc49DW";
            // // pass_hash = "$2a$13$Z2oB.egMA1rypVTlh3UbxegoOPobu8Cd6uWJV5hE1l0qfvQztRSEO"; $2a$13$VGG2dSFqx9BJxVBiCXA0QuzgIsRExg6Psxsy8uZGza13Nemwc49DW
            // // pass_hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
            //// Clipboard.SetText(pass_hash);
            // // pass_hash = "2a$12$2uySz/m4Qyzzz1K/R/qXyOOi10hdtecqZ4aXdRfa1dXU3mSYxRkYS"; "$2a$13$Z2oB.egMA1rypVTlh3UbxegoOPobu8Cd6uWJV5hE1l0qfvQztRSEO"

            // if (tbxUser.Text == username && BCrypt.Net.BCrypt.EnhancedVerify(tbxPass.Password, pass_hash))
            // {
            //     MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //     AdminPage admin = new AdminPage();
            //     admin.Show();
            //     this.Close();
            // }
            // else
            // {
            //     MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            //     tbxUser.Text = string.Empty;
            //     tbxPass.Password = string.Empty;
            // }
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            String queryPass = $"SELECT password FROM users WHERE username=@username";
            String queryRole = $"SELECT role FROM users WHERE username=@username";
            String queryUserDetails = $"SELECT user_id, username, first_name, last_name FROM users WHERE username=@username";
            MySqlCommand cmdPass = new MySqlCommand(queryPass, con);
            MySqlCommand cmdRole = new MySqlCommand(queryRole, con);
            MySqlCommand cmdUserDetails = new MySqlCommand(queryUserDetails, con);
            try
            {
                con.Open();
                cmdPass.Parameters.AddWithValue("@username", tbxUser.Text);
                MySqlDataAdapter sdaPass = new MySqlDataAdapter(cmdPass);
                DataTable password = new DataTable();
                sdaPass.Fill(password);
                String passHash = (string)cmdPass.ExecuteScalar();

                if (password.Rows.Count > 0 && BCrypt.Net.BCrypt.EnhancedVerify(tbxPass.Password, passHash))
                {
                    cmdRole.Parameters.AddWithValue("@username", tbxUser.Text);
                    cmdUserDetails.Parameters.AddWithValue("@username", tbxUser.Text);
                    MySqlDataAdapter sdaUser = new MySqlDataAdapter(cmdUserDetails);
                    DataTable userCredentials = new DataTable();
                    sdaUser.Fill(userCredentials);

                    UserDetails.UserID = userCredentials.Rows[0][0].ToString();
                    UserDetails.Username = userCredentials.Rows[0][1].ToString();
                    UserDetails.FirstName = userCredentials.Rows[0][2].ToString();
                    UserDetails.LastName = userCredentials.Rows[0][3].ToString();

                    String role = (String)cmdRole.ExecuteScalar();
                    switch (role)
                    {
                        case "Admin":
                            MessageBox.Show("Login successful. Welcome, Admin!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            AdminPage admin = new AdminPage();
                            admin.Show();
                            this.Close();
                            break;
                        case "Doctor":
                            MessageBox.Show("Login successful. Welcome, Doctor!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            DoctorPage doctor = new DoctorPage();
                            doctor.Show();
                            this.Close();
                            break;
                        default:
                            MessageBox.Show("If you're reading this, some buffoon put a value other than Admin or Doctor in your role. Or I messed up the code. Either way, please contact me or the admin.", "Huh?????", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    tbxUser.Text = string.Empty;
                    tbxPass.Password = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                Environment.Exit(1);
            }
        }
    }
}