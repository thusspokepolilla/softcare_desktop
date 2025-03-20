using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for AdminUsers.xaml
    /// </summary>
    public partial class AdminUsers : Page
    {
        public AdminUsers()
        {
            InitializeComponent();

        }

        static void loadData(DataGrid dtgUsers)
        {
            string loadString = "server=localhost; user=root; database=softcare_hospital_db";
            string queryString = "SELECT user_id, username, role, last_name, first_name, middle_name FROM users";
            MySqlConnection load = new MySqlConnection(loadString);
            try
            {
                load.Open();
                MySqlDataAdapter store = new MySqlDataAdapter(queryString, load);
                DataTable userTable = new DataTable();
                store.Fill(userTable);
                dtgUsers.ItemsSource = userTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                
            }
            finally
            {
                load.Close();
            }
        }

        private void dtgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView? selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                tbxUserID.Text = selected_row["user_id"].ToString();
                tbxUsername.Text = selected_row["username"].ToString();
                tbxPassword.Clear();
                cmbRole.Text = selected_row["role"].ToString();
                tbxLastName.Text = selected_row["last_name"].ToString();
                tbxFirstName.Text = selected_row["first_name"].ToString();
                tbxMiddleName.Text = selected_row["middle_name"].ToString();

                
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //DataTable dataTable = new DataTable();
            //DataColumn user_id = new DataColumn("user_id", typeof(int));
            //DataColumn username = new DataColumn("username", typeof(string));
            //// DataColumn password = new DataColumn("password", typeof(string));
            //DataColumn role = new DataColumn("role", typeof(string));
            //DataColumn last_name = new DataColumn("last_name", typeof(string));
            //DataColumn first_name = new DataColumn("first_name", typeof(string));
            //DataColumn middle_name = new DataColumn("middle_name", typeof(string));

            loadData(dtgUsers);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string addString = "INSERT INTO users (username, password, role, last_name, first_name, middle_name) " +
                               "VALUES (@username, @password, @role, @last_name, @first_name, @middle_name)";
            MySqlCommand cmdAdd = new MySqlCommand(addString, con);
            try
            {
                if (tbxUsername.Text != "" && tbxPassword.Password != "" && tbxLastName.Text != "" && tbxFirstName.Text != "" && cmbRole.SelectedIndex != -1)
                {
                    con.Open();
                    cmdAdd.Parameters.AddWithValue("@username", tbxUsername.Text);
                    string hashedPass = BCrypt.Net.BCrypt.EnhancedHashPassword(tbxPassword.Password, 13);
                    cmdAdd.Parameters.AddWithValue("@password", hashedPass);
                    cmdAdd.Parameters.AddWithValue("@role", cmbRole.Text);
                    cmdAdd.Parameters.AddWithValue("@last_name", tbxLastName.Text);
                    cmdAdd.Parameters.AddWithValue("@first_name", tbxFirstName.Text);
                    cmdAdd.Parameters.AddWithValue("@middle_name", tbxMiddleName.Text);

                    int result = cmdAdd.ExecuteNonQuery();
                    if (result < 0)
                    {
                        MessageBox.Show("An error occured while inserting data into the database!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Data inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    loadData(dtgUsers);
                    dtgUsers.SelectedIndex = -1;
                    tbxUserID.Clear();
                    tbxUsername.Clear();
                    tbxPassword.Clear();
                    cmbRole.SelectedIndex = 0;
                    tbxLastName.Clear();
                    tbxFirstName.Clear();
                    tbxMiddleName.Clear();
                }
                else
                {
                    MessageBox.Show("Required fields (*) cannot be left blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string updString = "UPDATE users " +
                               "SET username=@username, role=@role, last_name=@last_name, first_name=@first_name, middle_name=@middle_name " +
                               "WHERE user_id=@user_id";
            string updPassString = "UPDATE users " +
                                   "SET password=@hashed_pass " +
                                   "WHERE user_id=@user_id";
            MySqlCommand cmdUpd = new MySqlCommand(updString, con);
            MySqlCommand cmdUpdPass = new MySqlCommand(updPassString, con);
            try
            {
                if (dtgUsers.SelectedIndex != -1 && tbxUsername.Text != "" && tbxLastName.Text != "" && tbxFirstName.Text != "" && cmbRole.SelectedIndex != -1)
                {
                    if (MessageBox.Show("Are you sure you want to update this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        con.Open();
                        cmdUpd.Parameters.AddWithValue("@user_id", tbxUserID.Text);
                        cmdUpd.Parameters.AddWithValue("@username", tbxUsername.Text);
                        cmdUpd.Parameters.AddWithValue("@role", cmbRole.Text);
                        cmdUpd.Parameters.AddWithValue("@last_name", tbxLastName.Text);
                        cmdUpd.Parameters.AddWithValue("@first_name", tbxFirstName.Text);
                        cmdUpd.Parameters.AddWithValue("@middle_name", tbxMiddleName.Text);

                        if (cmdUpd.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("An error occured while updating the user's data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (tbxPassword.Password != "")
                            {
                                string hashedPass = BCrypt.Net.BCrypt.EnhancedHashPassword(tbxPassword.Password, 13);
                                cmdUpdPass.Parameters.AddWithValue("@hashed_pass", hashedPass);
                                cmdUpdPass.Parameters.AddWithValue("@user_id", tbxUserID.Text);
                                cmdUpdPass.ExecuteNonQuery();
                            }
                            MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgUsers);
                        dtgUsers.SelectedIndex = -1;
                        tbxUserID.Clear();
                        tbxUsername.Clear();
                        tbxPassword.Clear();
                        cmbRole.SelectedIndex = 0;
                        tbxLastName.Clear();
                        tbxFirstName.Clear();
                        tbxMiddleName.Clear();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Required fields (*) cannot be left blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string delString = "DELETE FROM users WHERE user_id=@user_id";
            MySqlCommand cmdDel = new MySqlCommand(delString, con);
            try
            {
                if (dtgUsers.SelectedIndex != -1 && tbxUserID.Text != "")
                {
                    con.Open();
                    cmdDel.Parameters.AddWithValue("@user_id", tbxUserID.Text);
                    if (MessageBox.Show("Are you sure you want to delete the currently selected user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (cmdDel.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("User deletion failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgUsers);
                        dtgUsers.SelectedIndex = -1;
                        tbxUserID.Clear();
                        tbxUsername.Clear();
                        tbxPassword.Clear();
                        cmbRole.SelectedIndex = 0;
                        tbxLastName.Clear();
                        tbxFirstName.Clear();
                        tbxMiddleName.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("No user is currently selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            loadData(dtgUsers);
            dtgUsers.SelectedIndex = -1;
            tbxUserID.Clear();
            tbxUsername.Clear();
            tbxPassword.Clear();
            cmbRole.SelectedIndex = 0;
            tbxLastName.Clear();
            tbxFirstName.Clear();
            tbxMiddleName.Clear();
        }


    }
}
