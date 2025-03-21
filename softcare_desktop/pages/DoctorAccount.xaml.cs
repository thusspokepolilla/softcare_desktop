using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for DoctorAccount.xaml
    /// </summary>
    /// 
    public partial class DoctorAccount : Page
    {
        string UserID = UserDetails.UserID;
        string Username = UserDetails.Username;
        string conString = "server=localhost; user=root; database=softcare_hospital_db";
        
        public DoctorAccount()
        {
            InitializeComponent();
            tbkUsername.Text = Username;
        }

        private void btnUpdateUsername_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(conString);
            string updString = "UPDATE users " +
                               "SET username=@username " +
                               "WHERE user_id=@user_id";

            MySqlCommand cmdUpdUsername = new MySqlCommand(updString, con);
            try
            {
                if (tbxUsername.Text != "")
                {
                    if (MessageBox.Show("Are you sure you want to update your username?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        con.Open();
                        cmdUpdUsername.Parameters.AddWithValue("@user_id", UserID);
                        cmdUpdUsername.Parameters.AddWithValue("@username", tbxUsername.Text);

                        if (cmdUpdUsername.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("An error occured while updating your username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Username updated successfully. Please log in again.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoginScreen log = new LoginScreen();
                            log.Show();
                            Window.GetWindow(this).Close();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Username cannot be left blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(conString);
            string updPassString = "UPDATE users " +
                       "SET password=@hashed_pass " +
                       "WHERE user_id=@user_id";
            MySqlCommand cmdUpdPass = new MySqlCommand(updPassString, con);

            try
            {
                if (tbxNewPassword.Password != "" && tbxConfirmPassword.Password != "" && (tbxNewPassword.Password == tbxConfirmPassword.Password))
                {
                    if (MessageBox.Show("Are you sure you want to update your password?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        con.Open();
                        string hashedPass = BCrypt.Net.BCrypt.EnhancedHashPassword(tbxNewPassword.Password, 13);
                        cmdUpdPass.Parameters.AddWithValue("@hashed_pass", hashedPass);
                        cmdUpdPass.Parameters.AddWithValue("@user_id", UserID);
                        if (cmdUpdPass.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("An error occured while updating your password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Password updated successfully. Please log in again.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoginScreen log = new LoginScreen();
                            log.Show();
                            Window.GetWindow(this).Close();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Passwords must match and cannot be left blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con.Close();
            }


        }

    }
}
