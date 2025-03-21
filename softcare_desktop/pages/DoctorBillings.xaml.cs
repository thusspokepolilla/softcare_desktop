using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for DoctorBillings.xaml
    /// </summary>
    public partial class DoctorBillings : Page
    {
        DataTable billingTable = new DataTable();
        string DoctorID = UserDetails.UserID;
        string loadString = "server=localhost; user=root; database=softcare_hospital_db";

        public DoctorBillings()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string queryString = @"SELECT billings.billing_id, patient_records.patient_id, patient_records.doctor_id, patient_records.last_name, patient_records.first_name, patient_records.middle_name, billings.mode_of_payment, billings.total_amount, billings.is_paid, billings.balance
                                  FROM billings
                                  JOIN patient_records ON billings.patient_record_id = patient_records.patient_id
                                  WHERE patient_records.doctor_id = @doctor_id

                                  UNION ALL

                                  SELECT billings.billing_id, appointments.appointment_id, appointments.doctor_id, appointments.last_name, appointments.first_name, appointments.middle_name, billings.mode_of_payment, billings.total_amount, billings.is_paid, billings.balance
                                  FROM billings
                                  JOIN appointments ON billings.appointment_id = appointments.appointment_id
                                  WHERE appointments.doctor_id = @doctor_id";
            MySqlConnection load = new MySqlConnection(loadString);
            try
            {
                load.Open();
                MySqlCommand queryAppointments = new MySqlCommand(queryString, load);
                queryAppointments.Parameters.AddWithValue("@doctor_id", DoctorID);

                MySqlDataAdapter store = new MySqlDataAdapter(queryAppointments);
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
    }
}
