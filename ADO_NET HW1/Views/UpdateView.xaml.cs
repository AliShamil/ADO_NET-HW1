using ADO_NET_HW1.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;

namespace ADO_NET_HW1.Views
{
 
    public partial class UpdateView : Window
    {
        private SqlConnection? connection;
        public Book? Book { get; set; }


        public UpdateView(SqlConnection? connection, int? id, string? bookName, int? pages, int? yearPress, int? idAuthor, int? idCategory, int? idTheme, int? idPress, string? comment, int? quantity)
        {
            InitializeComponent();
            DataContext = this;
            this.connection = connection;
            Book = new Book( id, bookName,  pages,  yearPress, idAuthor, idTheme,  idCategory, idPress,comment, quantity);
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection?.Open();

                SqlCommand command = new($"UPDATE Books SET [Name] = '{Book?.Name}', Pages= {Book?.Pages}, YearPress = {Book?.YearPress},Id_Author = {Book?.IdAuthor}, Id_Themes = {Book?.IdTheme},Id_Category = {Book?.IdCategory},Id_Press = {Book?.IdPress},Comment = '{Book?.Comment}',Quantity = {Book?.Quantity} WHERE Id = {Book?.Id}", connection);
                command.ExecuteNonQuery();
                
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
