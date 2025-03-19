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
            DataRowView selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                tbxUserID.Text = selected_row["user_id"].ToString();
                tbxUsername.Text = selected_row["username"].ToString();
                cmbRole.Text = selected_row["role"].ToString();
                tbxLastName.Text = selected_row["last_name"].ToString();
                tbxFirstName.Text = selected_row["first_name"].ToString();
                tbxMiddleName.Text = selected_row["middle_name"].ToString();

                
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
            DataColumn user_id = new DataColumn("user_id", typeof(int));
            DataColumn username = new DataColumn("username", typeof(string));
            // DataColumn password = new DataColumn("password", typeof(string));
            DataColumn role = new DataColumn("role", typeof(string));
            DataColumn last_name = new DataColumn("last_name", typeof(string));
            DataColumn first_name = new DataColumn("first_name", typeof(string));
            DataColumn middle_name = new DataColumn("middle_name", typeof(string));

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
                if (tbxUsername.Text != "" && tbxPassword.Password != "" && tbxLastName.Text != "" && tbxFirstName.Text != "")
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
                }
                else
                {
                    MessageBox.Show("Username, password, last name, and first name can't be left blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
