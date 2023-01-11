using ADO_NET_HW1.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
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

namespace ADO_NET_HW1.Views;


public partial class MainView : Window
{
    SqlConnection? connection = null;
    string id = default;
    public MainView(string? connectionStr)
    {
        InitializeComponent();
        DataContext = this;
        connection = new SqlConnection(connectionStr);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            connection?.Open();

            using SqlCommand command = new SqlCommand("SELECT * FROM Authors", connection);
            var result = command.ExecuteReader();

            int line = 0;

            while (result.Read())
            {
                if (line == 0)
                {
                    line++;
                    continue;
                }

                int? id = result["Id"] as int?;
                string? firstName = result["FirstName"] as string;
                string? lastName = result["LastName"] as string;

                Author_Cbox.Items.Add(firstName + " " + lastName);
            }
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

    private void Author_Cbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!Category_Cbox.IsEnabled)
            Category_Cbox.IsEnabled = !Category_Cbox.IsEnabled;

        Category_Cbox.Items.Clear();
        
        try
        {
            connection?.Open();
            using SqlCommand findId = new SqlCommand("SELECT Id FROM Authors WHERE (FirstName + ' ' + LastName) =@p1", connection);
            findId.Parameters.AddWithValue("@p1", Author_Cbox.SelectedItem.ToString());
            var result = findId.ExecuteReader();
            while (result.Read())
            {
                id = result[0].ToString();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            connection?.Close();
        }

        try
        {
            connection?.Open();
            using SqlCommand command = new SqlCommand("SELECT DISTINCT Categories.[Name] FROM Categories\r\nJOIN Books ON Id_Category = Categories.Id\r\nJOIN Authors ON Id_Author = Authors.Id\r\nWHERE Authors.Id =@id", connection);
            command.Parameters.AddWithValue("@id", id);
            var result = command.ExecuteReader();

                while (result.Read())
                    Category_Cbox.Items.Add(result["Name"] as string);
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

    private void Category_Cbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Category_Cbox.Items.IsEmpty)
            return;
        try
        {
            connection?.Open();
            using SqlCommand findId = new SqlCommand("SELECT Id FROM Authors WHERE (FirstName + ' ' + LastName) =@p1", connection);
            findId.Parameters.AddWithValue("@p1", Author_Cbox.SelectedItem.ToString());
            var result = findId.ExecuteReader();
            while (result.Read())
            {
                id = result[0].ToString();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            connection?.Close();
        }

        try
        {
            connection?.Open();

            
            var name = Category_Cbox.SelectedItem.ToString();

            using SqlCommand command = new SqlCommand("SELECT * FROM Books\r\nJOIN Categories ON Categories.Id = Id_Category \r\nJOIN Authors ON Authors.Id = Id_Author \r\nWHERE Categories.Name =@name AND Id_Author =@id\r\n", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@id", id);

            var result = command.ExecuteReader();

            ListBooks.Items.Clear();

            while (result.Read())
            {
                var bookId = result["Id"] as int?;
                var bookName = result["Name"] as string;
                var bookPages = result["Pages"] as int?;
                var bookYearPress = result["YearPress"] as int?;
                var IdAuthor = result["Id_Author"] as int?;
                var IdTheme = result["Id_Themes"] as int?;
                var IdCategory = result["Id_Category"] as int?;
                var IdPress = result["Id_Press"] as int?;
                var comment = result["Comment"] as string;
                var quantity = result["Quantity"] as int?;

                var book = new Book(bookId, bookName, bookPages, bookYearPress, IdAuthor, IdTheme, IdCategory, IdPress, comment, quantity);
                ListBooks.Items.Add(book);
            }
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

    private void Btn_Search_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Txt_Search.Text))
            return;

        try
        {
            connection?.Open();

            using SqlCommand command = new SqlCommand($"SELECT * FROM Books\r\nWHERE (Name LIKE @text)", connection);
            command.Parameters.AddWithValue("@text", "%" + Txt_Search.Text + "%");
            var result = command.ExecuteReader();

            ListBooks.Items.Clear();

            while (result.Read())
            {
                var bookId = result["Id"] as int?;
                var bookName = result["Name"] as string;
                var bookPages = result["Pages"] as int?;
                var bookYearPress = result["YearPress"] as int?;
                var IdAuthor = result["Id_Author"] as int?;
                var IdTheme = result["Id_Themes"] as int?;
                var IdCategory = result["Id_Category"] as int?;
                var IdPress = result["Id_Press"] as int?;
                var comment = result["Comment"] as string;
                var quantity = result["Quantity"] as int?;

                var book = new Book(bookId, bookName, bookPages, bookYearPress, IdAuthor, IdTheme, IdCategory, IdPress, comment, quantity);
                ListBooks.Items.Add(book);
            }
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
    private void Btn_Add_Click(object sender, RoutedEventArgs e)
    {
        AddView addView = new(connection);

        addView.ShowDialog();
    }

    private void Btn_Update_Click(object sender, RoutedEventArgs e)
    {
        if (ListBooks.SelectedItem is null)
            return;

        var book = ListBooks.SelectedItem as Book;

        if (book != null)
        {
            UpdateView updateView = new(connection, book.Id, book.Name, book.Pages, book.YearPress, book.IdAuthor, book.IdCategory, book.IdTheme, book.IdPress, book.Comment, book.Quantity);

            updateView.ShowDialog();
        }
    }

    private void Btn_Delete_Click(object sender, RoutedEventArgs e)
    {
        if (ListBooks.SelectedItem is null)
            return;

        try
        {
            connection?.Open();

            var id = (ListBooks.SelectedItem as Book)?.Id;

            SqlCommand command = new SqlCommand("DELETE FROM Books WHERE Id =@id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

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

    private void Txt_Search_KeyDown(object sender, KeyEventArgs e)
    {
            if (e.Key == Key.Enter)
                Btn_Search_Click(sender, e);   
    }
}
