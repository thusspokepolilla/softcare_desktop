using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for DoctorAppointments.xaml
    /// </summary>
    public partial class DoctorAppointments : Page
    {
        public DoctorAppointments()
        {
            InitializeComponent();
        }

        static void loadData(DataGrid dtgAppointments)
        {
            string loadString = "server=localhost; user=root; database=softcare_hospital_db";
            string queryString = "SELECT * FROM appointments";
            MySqlConnection load = new MySqlConnection(loadString);
            try
            {
                load.Open();
                MySqlDataAdapter store = new MySqlDataAdapter(queryString, load);
                DataTable appTable = new DataTable();
                store.Fill(appTable);
                dtgAppointments.ItemsSource = appTable.DefaultView;
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

        private void dtgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView? selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                tbxAppointmentID.Text = selected_row["appointment_id"].ToString();
                tbxDoctorID.Text = selected_row["doctor_id"].ToString();
                tbxLastName.Text = selected_row["last_name"].ToString();
                tbxFirstName.Text = selected_row["first_name"].ToString();
                tbxMiddleName.Text = selected_row["middle_name"].ToString();
                dtpAppointmentDate.Text = selected_row["appointment_date"].ToString();
                tbxAppointmentHour.Text = selected_row["appointment_time"].ToString().Substring(0, 2);
                tbxAppointmentMinute.Text = selected_row["appointment_time"].ToString().Substring(3, 2);
                cmbAppointmentStatus.Text = selected_row["status"].ToString();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
            DataColumn appointment_id = new DataColumn("last_name", typeof(string));
            DataColumn doctor_id = new DataColumn("first_name", typeof(string));
            DataColumn last_name = new DataColumn("last_name", typeof(string));
            DataColumn first_name = new DataColumn("first_name", typeof(string));
            DataColumn middle_name = new DataColumn("middle_name", typeof(string));
            DataColumn appointment_date = new DataColumn("appointment_date", typeof(string));
            DataColumn appointment_time = new DataColumn("appointment_time", typeof(string));
            DataColumn status = new DataColumn("status", typeof(string));

            loadData(dtgAppointments);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string addString = "INSERT INTO appointments (doctor_id, last_name, first_name, middle_name, appointment_date, appointment_time, status) " +
                               "VALUES (@doctor_id, @last_name, @first_name, @middle_name, @appointment_date, @appointment_time, @status)";
            MySqlCommand cmdAdd = new MySqlCommand(addString, con);
            try
            {
                if (tbxDoctorID.Text != "" && tbxLastName.Text != "" && tbxFirstName.Text != "" && dtpAppointmentDate.Text != "" && tbxAppointmentHour.Text != "" && tbxAppointmentMinute.Text != "" && cmbAppointmentStatus.SelectedIndex != -1)
                {
                    con.Open();
                    //cmdAdd.Parameters.AddWithValue("@", .Text);
                    cmdAdd.Parameters.AddWithValue("@doctor_id", tbxDoctorID.Text);
                    cmdAdd.Parameters.AddWithValue("@last_name", tbxLastName.Text);
                    cmdAdd.Parameters.AddWithValue("@first_name", tbxFirstName.Text);
                    cmdAdd.Parameters.AddWithValue("@middle_name", tbxMiddleName.Text);
                    cmdAdd.Parameters.AddWithValue("@appointment_date", dtpAppointmentDate.Text);
                    cmdAdd.Parameters.AddWithValue("@appointment_time", tbxAppointmentHour.Text + ":" + tbxAppointmentMinute.Text + ":00");
                    cmdAdd.Parameters.AddWithValue("@status", cmbAppointmentStatus.Text);

                    int result = cmdAdd.ExecuteNonQuery();
                    if (result < 0)
                    {
                        MessageBox.Show("An error occured while inserting data into the database!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Data inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    loadData(dtgAppointments);
                    dtgAppointments.SelectedIndex = -1;
                    tbxDoctorID.Clear();
                    tbxLastName.Clear();
                    tbxFirstName.Clear();
                    tbxMiddleName.Clear();
                    dtpAppointmentDate.Text = string.Empty;
                    tbxAppointmentHour.Clear();
                    tbxAppointmentMinute.Clear();
                    cmbAppointmentStatus.SelectedIndex = 0;
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
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string updString = "UPDATE appointments " +
                               "SET doctor_id=@doctor_id, last_name=@last_name, first_name=@first_name, middle_name=@middle_name, appointment_date=@appointment_date, appointment_time=@appointment_time, status=@status " +
                               "WHERE appointment_id=@appointment_id";
            MySqlCommand cmdUpd = new MySqlCommand(updString, con);
            try
            {
                if (dtgAppointments.SelectedIndex != -1 && tbxDoctorID.Text != "" && tbxLastName.Text != "" && tbxFirstName.Text != "" && dtpAppointmentDate.Text != "" && tbxAppointmentHour.Text != "" && tbxAppointmentMinute.Text != "" && cmbAppointmentStatus.SelectedIndex != -1)
                {
                    if (MessageBox.Show("Are you sure you want to update this appointment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        con.Open();
                        cmdUpd.Parameters.AddWithValue("@appointment_id", tbxAppointmentID.Text);
                        cmdUpd.Parameters.AddWithValue("@doctor_id", tbxDoctorID.Text);
                        cmdUpd.Parameters.AddWithValue("@last_name", tbxLastName.Text);
                        cmdUpd.Parameters.AddWithValue("@first_name", tbxFirstName.Text);
                        cmdUpd.Parameters.AddWithValue("@middle_name", tbxMiddleName.Text);
                        cmdUpd.Parameters.AddWithValue("@appointment_date", dtpAppointmentDate.Text);
                        cmdUpd.Parameters.AddWithValue("@appointment_time", tbxAppointmentHour.Text + ":" + tbxAppointmentMinute.Text + ":00");
                        cmdUpd.Parameters.AddWithValue("@status", cmbAppointmentStatus.Text);


                        if (cmdUpd.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("An error occured while updating the appointment's data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Appointment updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgAppointments);
                        dtgAppointments.SelectedIndex = -1;
                        tbxDoctorID.Clear();
                        tbxLastName.Clear();
                        tbxFirstName.Clear();
                        tbxMiddleName.Clear();
                        dtpAppointmentDate.Text = string.Empty;
                        tbxAppointmentHour.Clear();
                        tbxAppointmentMinute.Clear();
                        cmbAppointmentStatus.SelectedIndex = 0;
                    }

                }
                else
                {
                    MessageBox.Show("No row is selected, or some required fields are left blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            loadData(dtgAppointments);
            dtgAppointments.SelectedIndex = -1;
            tbxDoctorID.Clear();
            tbxLastName.Clear();
            tbxFirstName.Clear();
            tbxMiddleName.Clear();
            dtpAppointmentDate.Text = string.Empty;
            tbxAppointmentHour.Clear();
            tbxAppointmentMinute.Clear();
            cmbAppointmentStatus.SelectedIndex = 0;
        }


    }
}
