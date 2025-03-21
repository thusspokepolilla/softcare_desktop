using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace softcare_desktop.pages
{
    /// <summary>
    /// Interaction logic for AdminSQL.xaml
    /// </summary>
    public partial class AdminSQL : Page
    {
        public AdminSQL()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string query = tbxQuery.Text;
            string conString = "server=localhost; user=root; database=softcare_hospital_db";
            MySqlConnection con = new MySqlConnection(conString);
            try
            {
                MySqlDataAdapter dtaQuery = new MySqlDataAdapter(query, con);
                DataTable queryResults = new DataTable();
                dtaQuery.Fill(queryResults);
                dtgQuery.ItemsSource = queryResults.DefaultView;
                MessageBox.Show("Query executed successfully.", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
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
