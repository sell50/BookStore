/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using BookStoreLIB;
using System.Collections.ObjectModel;

namespace BookStoreGUI
{
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData.LoggedIn)
            {
                // get user account information
                var dbUser = new DALUserInfo();
                Dictionary<string, string> accountInfo = dbUser.AccountInfo(userData.UserID);

                // create account summary dialog with users information
                AccountSummaryDialog dlgg = new AccountSummaryDialog();
                dlgg.FistNameTextBox.Text = accountInfo["fname"];
                dlgg.LastNameTextBox.Text = accountInfo["lname"];
                dlgg.EmailTextBox.Text = accountInfo["email"];
                dlgg.PhoneNmuberTextBox.Text = accountInfo["phone"];
                dlgg.DateofBirthTextBox.Text = accountInfo["bdate"];
                dlgg.Owner = this;
                dlgg.ShowDialog();

                if (dlgg.DialogResult == true)
                {
                    //process data from account summary dialog

                }
                return;
            }

            //add register page here when user is not logged in
            var reg = new Register();
            reg.Owner = this;
            reg.ShowDialog();

            if (reg.DialogResult == true) MessageBox.Show("Registration successful!");
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // check if user is logged in
            if (userData.LoggedIn)
            {
                userData.Logout();
                this.loginButton.Content = "Login";
                this.registerButton.Content = "Register";
                this.statusTextBlock.Text = "You are logged out!";
                return;
            }

            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true)
                {
                    this.statusTextBlock.Text = "You are logged in as User #" + userData.UserID;
                    // change login button to logout
                    this.loginButton.Content = "Logout";
                    this.registerButton.Content = "Account";
                }
                else
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
            }
        }
        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            // check if user is logged in
            if (userData.LoggedIn)
            {
                this.registerButton.Content = "Yee";
                return;
            }

            Register reg = new Register();
            reg.Owner = this;
            reg.ShowDialog();
            // Process data entered by user if dialog box is accepted
            /*
            if (reg.DialogResult == true)
            {
                if (userData.LogIn(reg.usernameBox.Text, reg.password.Password) == true)
                {
                    this.statusTextBlock.Text = "You are logged in as User #" + userData.UserID;
                    // change login button to logout
                    this.loginButton.Content = "Logout";
                }
                else
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
            }
            */
        }
        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
        public MainWindow() { InitializeComponent(); }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"];
            bookOrder = new BookOrder();
            userData = new UserData();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
            orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
            orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
            orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();
            if (orderItemDialog.DialogResult == true)
            {
                string isbn = orderItemDialog.isbnTextBox.Text;
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
            }
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }
        private void chechoutButton_Click(object sender, RoutedEventArgs e)
        {
            int orderId;
            //This line below doesn't actually work so when someone asks about why orderID is just userID idk either - Bryce
            //orderId = bookOrder.PlaceOrder(userData.UserID);
            orderId = userData.UserID;
            MessageBox.Show("Your order has been placed. Your order id is " +
            orderId.ToString());

            //Connect to database
            string connection = "Data Source=tfs.cspc1.uwindsor.ca; Initial Catalog = AgileDB21AX; Persist Security Info = True; User ID = AgileDB21AX; Password = AgileDB21AX#";
            SqlConnection con = new SqlConnection(connection);


            //So  we want to iterate now over each item and then do that shit.
            for (int x = 0; x < bookOrder.OrderItemList.Count(); x++)
            {

                string query = "INSERT INTO PurchaseHistory(UserID, Date, BookTitle, Quantity, SubTotal) " +
                    "VALUES (" + orderId + ", GETDATE(), @title, " + bookOrder.OrderItemList[x].Quantity + ", " + bookOrder.OrderItemList[x].SubTotal + ")";
   
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@title", bookOrder.OrderItemList[x].BookTitle);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Query Executed");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error Generated. Details: " + ex.ToString());
                }
                finally
                {
                    con.Close();
                    Console.WriteLine("Connecction Closed");
                }
            } 
            

            // add this line on successful checkout
            bookOrder.OrderItemList.Clear();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //Im so sorry for this.
            String search = searchInput.Text;
            int comboBoxIndex = categoriesComboBox.SelectedIndex + 1;

            string test = "Data Source=tfs.cspc1.uwindsor.ca; Initial Catalog = AgileDB21AX; Persist Security Info = True; User ID = AgileDB21AX; Password = AgileDB21AX#";
            string cmdString = string.Empty;

            using (SqlConnection conn = new SqlConnection(test))
            {
                cmdString = "Select ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher from BookData WHERE (Title LIKE '%" + search + "%' OR Author LIKE '%" + search + "%') AND CategoryID =" + comboBoxIndex;
                SqlCommand cmd = new SqlCommand(cmdString, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("updatedTable");
                sda.Fill(dt);
                ProductsDataGrid.ItemsSource = dt.DefaultView;

            }

        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int comboBoxIndex = categoriesComboBox.SelectedIndex + 1;

            string test = "Data Source=tfs.cspc1.uwindsor.ca; Initial Catalog = AgileDB21AX; Persist Security Info = True; User ID = AgileDB21AX; Password = AgileDB21AX#";
            string cmdString = string.Empty;

            using (SqlConnection conn = new SqlConnection(test))
            {
                cmdString = "Select ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher from BookData WHERE CategoryID =" + comboBoxIndex;
                SqlCommand cmd = new SqlCommand(cmdString, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("updatedTable");
                sda.Fill(dt);
                ProductsDataGrid.ItemsSource = dt.DefaultView;

            }
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = userData.UserID;

            string message = string.Empty;
            


            //Connect to database
            string connection = "Data Source=tfs.cspc1.uwindsor.ca; Initial Catalog = AgileDB21AX; Persist Security Info = True; User ID = AgileDB21AX; Password = AgileDB21AX#";
            SqlConnection con = new SqlConnection(connection);

            con.Open();
            string query = "SELECT * FROM PurchaseHistory WHERE UserID = " + userId;
            SqlCommand cmd = new SqlCommand(query, con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string copyAmount = reader["Quantity"] is 1 ? " copy of " : " copies of ";
                        message += Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy") + " " + reader["Quantity"] + copyAmount + reader["BookTitle"] + " for $" + reader["SubTotal"] + "\n";
                    }
                }
                else
                {
                    message = "You have not ordered anything yet.";
                }
            }
            con.Close();

            MessageBox.Show(message, "Order History for User " + userId);
        }

    }
}
