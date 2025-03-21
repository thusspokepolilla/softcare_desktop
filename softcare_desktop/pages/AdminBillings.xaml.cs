using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for AdminBillings.xaml
    /// </summary>
    public partial class AdminBillings : Page
    {
        public AdminBillings()
        {
            InitializeComponent();
        }

        static void loadData(DataGrid dtgBillings)
        {
            string loadString = "server=localhost; user=root; database=softcare_hospital_db";
            string queryString = "SELECT * FROM billings";
            MySqlConnection load = new MySqlConnection(loadString);
            try
            {
                load.Open();
                MySqlDataAdapter store = new MySqlDataAdapter(queryString, load);
                DataTable billingTable = new DataTable();
                store.Fill(billingTable);
                dtgBillings.ItemsSource = billingTable.DefaultView;
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

        private void dtgBillings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView? selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                tbxBillingID.Text = selected_row["billing_id"].ToString();
                tbxPatientID.Text = selected_row["patient_record_id"].ToString();
                tbxAppointmentID.Text = selected_row["appointment_id"].ToString();
                cmbModeOfPayment.Text = selected_row["mode_of_payment"].ToString();
                tbxTotalAmount.Text = selected_row["total_amount"].ToString();
                cmbIsPaid.Text = selected_row["is_paid"].ToString();
                tbxBalance.Text = selected_row["balance"].ToString();
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

            loadData(dtgBillings);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            string addString = "INSERT INTO billings (patient_record_id, appointment_id, mode_of_payment, total_amount, is_paid, balance) " +
                               "VALUES (@patient_record_id, @appointment_id, @mode_of_payment, @total_amount, @is_paid, @balance)";
            MySqlCommand cmdAdd = new MySqlCommand(addString, con);
            try
            {
                if ((tbxPatientID.Text != "" ^ tbxAppointmentID.Text != "") && cmbModeOfPayment.SelectedIndex != -1 && tbxTotalAmount.Text != "" && cmbIsPaid.SelectedIndex != -1 && tbxBalance.Text != "")
                {
                    con.Open();
                    //cmdAdd.Parameters.AddWithValue("@", .Text);
                    if (string.IsNullOrEmpty(tbxPatientID.Text))
                    {
                        cmdAdd.Parameters.AddWithValue("@patient_record_id", DBNull.Value);
                        cmdAdd.Parameters.AddWithValue("@appointment_id", tbxAppointmentID.Text);
                    }
                    else if (string.IsNullOrEmpty(tbxAppointmentID.Text))
                    {
                        cmdAdd.Parameters.AddWithValue("@appointment_id", DBNull.Value);
                        cmdAdd.Parameters.AddWithValue("@patient_record_id", tbxPatientID.Text);
                    }
                    
                    cmdAdd.Parameters.AddWithValue("@mode_of_payment", cmbModeOfPayment.Text);
                    cmdAdd.Parameters.AddWithValue("@total_amount", tbxTotalAmount.Text);
                    cmdAdd.Parameters.AddWithValue("@is_paid", cmbIsPaid.Text);
                    cmdAdd.Parameters.AddWithValue("@balance", tbxBalance.Text);

                    int result = cmdAdd.ExecuteNonQuery();
                    if (result < 0)
                    {
                        MessageBox.Show("An error occured while inserting data into the database!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Data inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    loadData(dtgBillings);
                    tbxBillingID.Clear();
                    tbxPatientID.Clear();
                    tbxAppointmentID.Clear();
                    cmbModeOfPayment.SelectedIndex = -1;
                    tbxTotalAmount.Clear();
                    cmbIsPaid.SelectedIndex = -1;
                    tbxBalance.Clear();
                }
                else
                {
                    MessageBox.Show("Required fields (*) cannot be left blank. A billing record must be part of a patient record or an appointment record, but not both.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string updString = "UPDATE billings " +
                               "SET patient_record_id=@patient_record_id, appointment_id=@appointment_id, mode_of_payment=@mode_of_payment, total_amount=@total_amount, is_paid=@is_paid, balance=@balance " +
                               "WHERE billing_id=@billing_id";
            MySqlCommand cmdUpd = new MySqlCommand(updString, con);
            try
            {
                if (dtgBillings.SelectedIndex != -1 && (tbxPatientID.Text != "" ^ tbxAppointmentID.Text != "") && cmbModeOfPayment.SelectedIndex != -1 && tbxTotalAmount.Text != "" && cmbIsPaid.SelectedIndex != -1 && tbxBalance.Text != "")
                {
                    if (MessageBox.Show("Are you sure you want to update this patient?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        con.Open();
                        cmdUpd.Parameters.AddWithValue("@billing_id", tbxBillingID.Text);
                        if (string.IsNullOrEmpty(tbxPatientID.Text))
                        {
                            cmdUpd.Parameters.AddWithValue("@patient_record_id", DBNull.Value);
                            cmdUpd.Parameters.AddWithValue("@appointment_id", tbxAppointmentID.Text);
                        }
                        else if (string.IsNullOrEmpty(tbxAppointmentID.Text))
                        {
                            cmdUpd.Parameters.AddWithValue("@appointment_id", DBNull.Value);
                            cmdUpd.Parameters.AddWithValue("@patient_record_id", tbxPatientID.Text);
                        }
                        cmdUpd.Parameters.AddWithValue("@mode_of_payment", cmbModeOfPayment.Text);
                        cmdUpd.Parameters.AddWithValue("@total_amount", tbxTotalAmount.Text);
                        cmdUpd.Parameters.AddWithValue("@is_paid", cmbIsPaid.Text);
                        cmdUpd.Parameters.AddWithValue("@balance", tbxBalance.Text);


                        if (cmdUpd.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("An error occured while updating the billing's data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Billing updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgBillings);
                        tbxBillingID.Clear();
                        tbxPatientID.Clear();
                        tbxAppointmentID.Clear();
                        cmbModeOfPayment.SelectedIndex = -1;
                        tbxTotalAmount.Clear();
                        cmbIsPaid.SelectedIndex = -1;
                        tbxBalance.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Required fields (*) cannot be left blank. A billing record must be part of a patient record or an appointment record, but not both.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string delString = "DELETE FROM billings WHERE billing_id=@billing_id";
            MySqlCommand cmdDel = new MySqlCommand(delString, con);
            try
            {
                if (dtgBillings.SelectedIndex != -1 && tbxBillingID.Text != "")
                {
                    con.Open();
                    cmdDel.Parameters.AddWithValue("@billing_id", tbxBillingID.Text);
                    if (MessageBox.Show("Are you sure you want to delete this billing?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (cmdDel.ExecuteNonQuery() < 0)
                        {
                            MessageBox.Show("Billing deletion failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Billing deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        loadData(dtgBillings);
                        tbxBillingID.Clear();
                        tbxPatientID.Clear();
                        tbxAppointmentID.Clear();
                        cmbModeOfPayment.SelectedIndex = -1;
                        tbxTotalAmount.Clear();
                        cmbIsPaid.SelectedIndex = -1;
                        tbxBalance.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("No billing is currently selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            loadData(dtgBillings);
            tbxBillingID.Clear();
            tbxPatientID.Clear();
            tbxAppointmentID.Clear();
            cmbModeOfPayment.SelectedIndex = -1;
            tbxTotalAmount.Clear();
            cmbIsPaid.SelectedIndex = -1;
            tbxBalance.Clear();
        }
    }
}
