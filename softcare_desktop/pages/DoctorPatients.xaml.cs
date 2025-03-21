using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for DoctorPatients.xaml
    /// </summary>
    public partial class DoctorPatients : Page
    {
        DataTable patientTable = new DataTable();
        string DoctorID = UserDetails.UserID;
        string loadString = "server=localhost; user=root; database=softcare_hospital_db";

        public DoctorPatients()
        {
            InitializeComponent();
        }

        public void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string queryString = "SELECT * FROM patient_records WHERE doctor_id=@doctor_id";
            MySqlConnection load = new MySqlConnection(loadString);
            try
            {
                load.Open();
                MySqlCommand queryAppointments = new MySqlCommand(queryString, load);
                queryAppointments.Parameters.AddWithValue("@doctor_id", DoctorID);

                MySqlDataAdapter store = new MySqlDataAdapter(queryAppointments);
                store.Fill(patientTable);
                dtgPatients.ItemsSource = patientTable.DefaultView;
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
