using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for DoctorDashboard.xaml
    /// </summary>
    public partial class DoctorDashboard : Page
    {
        public DoctorDashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            tbkWelcomeName.Text = "Welcome, " + UserDetails.FirstName + " " + UserDetails.LastName;
            string DoctorID = UserDetails.UserID;

            string loadString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection load = new MySqlConnection(loadString);
            String queryPatientRecords = $"SELECT COUNT(*) FROM patient_records WHERE doctor_id=@doctor_id";
            String queryAppointments = $"SELECT COUNT(*) FROM appointments WHERE doctor_id=@doctor_id AND status='Pending'";
            String queryBills = @"SELECT SUM(total) AS doctor_billings
                                  FROM(SELECT COUNT(*) AS total
                                  FROM billings
                                  JOIN patient_records ON billings.patient_record_id = patient_records.patient_id
                                  WHERE patient_records.doctor_id = @doctor_id AND billings.is_paid = 'No'

                                  UNION ALL

                                  SELECT COUNT(*) AS total
                                  FROM billings
                                  JOIN appointments ON billings.appointment_id = appointments.appointment_id
                                  WHERE appointments.doctor_id = @doctor_id AND billings.is_paid = 'No') AS total";
            MySqlCommand cmdPatientRecords = new MySqlCommand(queryPatientRecords, load);
            MySqlCommand cmdAppointments = new MySqlCommand(queryAppointments, load);
            MySqlCommand cmdBills = new MySqlCommand(queryBills, load);

            String tableAppointments = $"SELECT * FROM appointments WHERE doctor_id=@doctor_id AND status='Pending'";
            String tableBills = @"SELECT billings.billing_id, patient_records.patient_id, patient_records.doctor_id, patient_records.last_name, patient_records.first_name, patient_records.middle_name, billings.mode_of_payment, billings.total_amount, billings.is_paid, billings.balance
                                  FROM billings
                                  JOIN patient_records ON billings.patient_record_id = patient_records.patient_id
                                  WHERE patient_records.doctor_id = @doctor_id AND billings.is_paid = 'No'

                                  UNION ALL

                                  SELECT billings.billing_id, appointments.appointment_id, appointments.doctor_id, appointments.last_name, appointments.first_name, appointments.middle_name, billings.mode_of_payment, billings.total_amount, billings.is_paid, billings.balance
                                  FROM billings
                                  JOIN appointments ON billings.appointment_id = appointments.appointment_id
                                  WHERE appointments.doctor_id = @doctor_id AND billings.is_paid = 'No'";

            MySqlCommand tablecmdAppointments = new MySqlCommand(tableAppointments, load);
            MySqlCommand tablecmdBills = new MySqlCommand(tableBills, load);
            try
            {
                load.Open();
                cmdPatientRecords.Parameters.AddWithValue("@doctor_id", DoctorID);
                cmdAppointments.Parameters.AddWithValue("@doctor_id", DoctorID);
                cmdBills.Parameters.AddWithValue("@doctor_id", DoctorID);

                tablecmdAppointments.Parameters.AddWithValue("@doctor_id", DoctorID);
                tablecmdBills.Parameters.AddWithValue("@doctor_id", DoctorID);
                
                PatientCount.Text = cmdPatientRecords.ExecuteScalar().ToString();
                DoctorAppointments.Text = cmdAppointments.ExecuteScalar().ToString();
                Billings.Text = cmdBills.ExecuteScalar().ToString();

                MySqlDataAdapter storeAppointments = new MySqlDataAdapter(tablecmdAppointments);
                DataTable dataAppointments = new DataTable();
                storeAppointments.Fill(dataAppointments);
                dtgPendingAppointments.ItemsSource = dataAppointments.DefaultView;

                MySqlDataAdapter storeBillings = new MySqlDataAdapter(tablecmdBills);
                DataTable dataBillings = new DataTable();
                storeBillings.Fill(dataBillings);
                dtgUnpaidBillings.ItemsSource = dataBillings.DefaultView;
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
    }
}
