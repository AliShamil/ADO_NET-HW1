using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADO_NET_HW1.Views
{
    /// <summary>
    /// Interaction logic for AddView.xaml
    /// </summary>
    public partial class AddView : Window
    {
        private DbConnection? _connection;

        public int Id { get; set; }
        public string? BookName { get; set; }
        public int Pages { get; set; }
        public int YearPress { get; set; }
        public int IdAuthor { get; set; }
        public int IdCategory { get; set; }
        public int IdTheme { get; set; }
        public int IdPress { get; set; }
        public string? Comment { get; set; }
        public int Quantity { get; set; }

        public AddView(DbConnection? connection)
        {
            InitializeComponent();
            _connection = connection;
            DataContext = this;
        }

        private void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection?.Open();

                SqlCommand command = new SqlCommand($"INSERT Books(Id,[Name],Pages,YearPress,Id_Themes,Id_Category,Id_Author,Id_Press,Comment,Quantity) VALUES({Id},'{BookName}',{Pages},{YearPress},{IdTheme},{IdCategory},{IdAuthor},{IdPress},'{Comment}',{Quantity})", (SqlConnection)_connection);

                command.ExecuteNonQuery();

                MessageBox.Show("Succesfully added", "Information");
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _connection?.Close();
            }
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
