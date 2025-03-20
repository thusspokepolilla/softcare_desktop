using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for AdminPatients.xaml
    /// </summary>
    public partial class AdminPatients : Page
    {
        public AdminPatients()
        {
            InitializeComponent();
        }

        static void loadData(DataGrid dtgPatients)
        {
            string loadString = "server=localhost; user=root; database=softcare_hospital_db";
            string queryString = "SELECT * FROM patient_records";
            MySqlConnection load = new MySqlConnection(loadString);
            try
            {
                load.Open();
                MySqlDataAdapter store = new MySqlDataAdapter(queryString, load);
                DataTable appTable = new DataTable();
                store.Fill(appTable);
                dtgPatients.ItemsSource = appTable.DefaultView;
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

        private void dtgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView? selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                tbxPatientID.Text = selected_row["patient_id"].ToString();
                tbxDoctorID.Text = selected_row["doctor_id"].ToString();
                tbxLastName.Text = selected_row["last_name"].ToString();
                tbxFirstName.Text = selected_row["first_name"].ToString();
                tbxMiddleName.Text = selected_row["middle_name"].ToString();
                tbxAge.Text = selected_row["age"].ToString();
                cmbSex.Text = selected_row["sex"].ToString();
                dtpAdmissionDate.Text = selected_row["admission_date"].ToString();
                tbxAdmissionHour.Text = selected_row["admission_time"].ToString().Substring(0, 2);
                tbxAdmissionMinute.Text = selected_row["admission_time"].ToString().Substring(3, 2);
                cmbAdmissionStatus.Text = selected_row["status"].ToString();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //DataTable dataTable = new DataTable();
            //DataColumn appointment_id = new DataColumn("last_name", typeof(string));
            //DataColumn doctor_id = new DataColumn("first_name", typeof(string));
            //DataColumn last_name = new DataColumn("last_name", typeof(string));
            //DataColumn first_name = new DataColumn("first_name", typeof(string));
            //DataColumn middle_name = new DataColumn("middle_name", typeof(string));
            //DataColumn admission_date = new DataColumn("admission_date", typeof(string));
            //DataColumn admission_time = new DataColumn("admission_time", typeof(string));
            //DataColumn status = new DataColumn("status", typeof(string));

            loadData(dtgPatients);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string addString = "INSERT INTO patient_records (doctor_id, last_name, first_name, middle_name, age, sex, admission_date, admission_time, status) " +
                               "VALUES (@doctor_id, @last_name, @first_name, @middle_name, @age, @sex, @admission_date, @admission_time, @status)";
            MySqlCommand cmdAdd = new MySqlCommand(addString, con);
            try
            {
                if (tbxDoctorID.Text != "" && tbxLastName.Text != "" && tbxFirstName.Text != "" && tbxAge.Text != "" && cmbSex.SelectedIndex != -1 && dtpAdmissionDate.Text != "" && tbxAdmissionHour.Text != "" && tbxAdmissionMinute.Text != "" && cmbAdmissionStatus.SelectedIndex != -1)
                {
                    con.Open();
                    //cmdAdd.Parameters.AddWithValue("@", .Text);
                    cmdAdd.Parameters.AddWithValue("@doctor_id", tbxDoctorID.Text);
                    cmdAdd.Parameters.AddWithValue("@last_name", tbxLastName.Text);
                    cmdAdd.Parameters.AddWithValue("@first_name", tbxFirstName.Text);
                    cmdAdd.Parameters.AddWithValue("@middle_name", tbxMiddleName.Text);
                    cmdAdd.Parameters.AddWithValue("@age", tbxAge.Text);
                    cmdAdd.Parameters.AddWithValue("@sex", cmbSex.Text);
                    cmdAdd.Parameters.AddWithValue("@admission_date", dtpAdmissionDate.Text);
                    cmdAdd.Parameters.AddWithValue("@admission_time", tbxAdmissionHour.Text + ":" + tbxAdmissionMinute.Text + ":00");
                    cmdAdd.Parameters.AddWithValue("@status", cmbAdmissionStatus.Text);

                    int result = cmdAdd.ExecuteNonQuery();
                    if (result < 0)
                    {
                        MessageBox.Show("An error occured while inserting data into the database!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Data inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    loadData(dtgPatients);
                    dtgPatients.SelectedIndex = -1;
                    tbxPatientID.Clear();
                    tbxDoctorID.Clear();
                    tbxLastName.Clear();
                    tbxFirstName.Clear();
                    tbxMiddleName.Clear();
                    tbxAge.Clear();
                    cmbSex.SelectedIndex = -1;
                    dtpAdmissionDate.Text = string.Empty;
                    tbxAdmissionHour.Clear();
                    tbxAdmissionMinute.Clear();
                    cmbAdmissionStatus.SelectedIndex = -1;
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
            string updString = "UPDATE patient_records " +
                               "SET doctor_id=@doctor_id, last_name=@last_name, first_name=@first_name, middle_name=@middle_name, age=@age, sex=@sex, admission_date=@admission_date, admission_time=@admission_time, status=@status " +
                               "WHERE patient_id=@patient_id";
            MySqlCommand cmdUpd = new MySqlCommand(updString, con);
            try
            {
                if (dtgPatients.SelectedIndex != -1 && tbxDoctorID.Text != "" && tbxLastName.Text != "" && tbxFirstName.Text != "" && tbxAge.Text != "" && cmbSex.SelectedIndex != -1 && dtpAdmissionDate.Text != "" && tbxAdmissionHour.Text != "" && tbxAdmissionMinute.Text != "" && cmbAdmissionStatus.SelectedIndex != -1)
                {
                    if (MessageBox.Show("Are you sure you want to update this patient?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        con.Open();
                        cmdUpd.Parameters.AddWithValue("@patient_id", tbxPatientID.Text);
                        cmdUpd.Parameters.AddWithValue("@doctor_id", tbxDoctorID.Text);
                        cmdUpd.Parameters.AddWithValue("@last_name", tbxLastName.Text);
                        cmdUpd.Parameters.AddWithValue("@first_name", tbxFirstName.Text);
                        cmdUpd.Parameters.AddWithValue("@middle_name", tbxMiddleName.Text);
                        cmdUpd.Parameters.AddWithValue("@age", tbxAge.Text);
                        cmdUpd.Parameters.AddWithValue("@sex", cmbSex.Text);
                        cmdUpd.Parameters.AddWithValue("@admission_date", dtpAdmissionDate.Text);
                        cmdUpd.Parameters.AddWithValue("@admission_time", tbxAdmissionHour.Text + ":" + tbxAdmissionMinute.Text + ":00");
                        cmdUpd.Parameters.AddWithValue("@status", cmbAdmissionStatus.Text);


                        if (cmdUpd.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("An error occured while updating the patient's data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Patient updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgPatients);
                        dtgPatients.SelectedIndex = -1;
                        tbxPatientID.Clear();
                        tbxDoctorID.Clear();
                        tbxLastName.Clear();
                        tbxFirstName.Clear();
                        tbxMiddleName.Clear();
                        tbxAge.Clear();
                        cmbSex.SelectedIndex = -1;
                        dtpAdmissionDate.Text = string.Empty;
                        tbxAdmissionHour.Clear();
                        tbxAdmissionMinute.Clear();
                        cmbAdmissionStatus.SelectedIndex = -1;
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
            string delString = "DELETE FROM patient_records WHERE patient_id=@patient_id";
            MySqlCommand cmdDel = new MySqlCommand(delString, con);
            try
            {
                if (dtgPatients.SelectedIndex != -1 && tbxPatientID.Text != "")
                {
                    con.Open();
                    cmdDel.Parameters.AddWithValue("@patient_id", tbxPatientID.Text);
                    if (MessageBox.Show("Are you sure you want to delete this patient?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (cmdDel.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("Patient deletion failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Patient deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgPatients);
                        dtgPatients.SelectedIndex = -1;
                        tbxPatientID.Clear();
                        tbxDoctorID.Clear();
                        tbxLastName.Clear();
                        tbxFirstName.Clear();
                        tbxMiddleName.Clear();
                        tbxAge.Clear();
                        cmbSex.SelectedIndex = -1;
                        dtpAdmissionDate.Text = string.Empty;
                        tbxAdmissionHour.Clear();
                        tbxAdmissionMinute.Clear();
                        cmbAdmissionStatus.SelectedIndex = -1;
                    }
                }
                else
                {
                    MessageBox.Show("No appointment is currently selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            loadData(dtgPatients);
            dtgPatients.SelectedIndex = -1;
            tbxPatientID.Clear();
            tbxDoctorID.Clear();
            tbxLastName.Clear();
            tbxFirstName.Clear();
            tbxMiddleName.Clear();
            tbxAge.Clear();
            cmbSex.SelectedIndex = -1;
            dtpAdmissionDate.Text = string.Empty;
            tbxAdmissionHour.Clear();
            tbxAdmissionMinute.Clear();
            cmbAdmissionStatus.SelectedIndex = -1;
        }


    }
}
