using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Page
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            tbkWelcomeName.Text = "Welcome, " + UserDetails.FirstName + " " + UserDetails.LastName;

            string loadString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection load = new MySqlConnection(loadString);
            String queryPatientRecords = $"SELECT COUNT(*) FROM patient_records";
            String queryAppointments = $"SELECT COUNT(*) FROM appointments";
            String queryBills = $"SELECT COUNT(*) FROM billings";
            String queryDoctors = $"SELECT COUNT(*) FROM users WHERE role='Doctor'";
            String queryAdmins = $"SELECT COUNT(*) FROM users WHERE role='Admin'";
            MySqlCommand cmdPatientRecords = new MySqlCommand(queryPatientRecords, load);
            MySqlCommand cmdAppointments = new MySqlCommand(queryAppointments, load);
            MySqlCommand cmdBills = new MySqlCommand(queryBills, load);
            MySqlCommand cmdDoctors = new MySqlCommand(queryDoctors, load);
            MySqlCommand cmdAdmins = new MySqlCommand(queryAdmins, load);
            try
            {
                load.Open();
                MySqlDataAdapter sdaPatientRecords = new MySqlDataAdapter(cmdPatientRecords);
                MySqlDataAdapter sdaAppointments = new MySqlDataAdapter(cmdAppointments);
                MySqlDataAdapter sdaBills = new MySqlDataAdapter(cmdBills);
                MySqlDataAdapter sdaDoctors = new MySqlDataAdapter(cmdDoctors);
                MySqlDataAdapter sdaAdmins = new MySqlDataAdapter(cmdAdmins);

                //DataTable patients = new DataTable();
                //DataTable appointments = new DataTable();
                //DataTable bills = new DataTable();
                //DataTable doctors = new DataTable();
                //DataTable admins = new DataTable();
                //sdaPatientRecords.Fill(patients);
                //sdaAppointments.Fill(appointments);
                //sdaBills.Fill(bills);
                //sdaDoctors.Fill(doctors);
                //sdaAdmins.Fill(admins);

                PatientCount.Text = cmdPatientRecords.ExecuteScalar().ToString();
                DoctorAppointments.Text = cmdAppointments.ExecuteScalar().ToString();
                Billings.Text = cmdBills.ExecuteScalar().ToString();
                UserAccounts.Text = cmdDoctors.ExecuteScalar().ToString() + " | " + cmdAdmins.ExecuteScalar().ToString();
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
