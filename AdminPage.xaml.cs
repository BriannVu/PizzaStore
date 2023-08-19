using BaoNamVu_SpringFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaoNamVu_SpringFinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class adminPage : Page
    {
        private List<User> usersList = new List<User>();
        private List<Order> ordersList = new List<Order>();
        private RegisteredUser selectedUser = new RegisteredUser();
        private Order selectedOrder = new Order();
        public adminPage()
        {
            this.InitializeComponent();
            this.UpdateAdminPageInfo();
        }
        public void UpdateAdminPageInfo()
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = MainPage.getConnectionString();
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                string query = "SELECT * FROM users";
                cmd.CommandText = query;
                conn.Open();
                usersList = new List<User>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RegisteredUser newUser = new RegisteredUser();

                    newUser.UserID = Convert.ToInt32(reader[0].ToString());
                    newUser.FullName = reader[1].ToString();
                    newUser.UserAddress = reader[2].ToString();
                    newUser.PostalCode = reader[3].ToString();
                    newUser.UserName = reader[4].ToString();
                    newUser.UserPassword = reader[5].ToString();

                    usersList.Add(newUser);
                }
                UserListLV.ItemsSource = null;
                UserListLV.ItemsSource = usersList;
                conn.Close();


                query = "SELECT * FROM orders";
                cmd.CommandText = query;
                conn.Open();
                ordersList = new List<Order>();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Order newOrder = new Order();

                    newOrder.orderId = Convert.ToInt32(reader[0].ToString());
                    newOrder.userId = Convert.ToInt32(reader[1].ToString());
                    newOrder.orderDate = DateTime.Parse(reader[2].ToString());
                    newOrder.orderDetails = reader[3].ToString();
                    newOrder.orderAddress = reader[4].ToString();
                    newOrder.orderTotalAmount = Math.Round(Convert.ToDouble(reader[5].ToString()), 2);

                    ordersList.Add(newOrder);
                }

                OrderListLV.ItemsSource = null;
                OrderListLV.ItemsSource = ordersList;
                reader.Close();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MainPage.Notice(message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void UserListLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = (RegisteredUser)(((ListView)sender).SelectedItem);
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if(UserListLV.SelectedIndex == -1)
            {
                MainPage.Notice("Please select user");
                return;
            }
            string query = "UPDATE users SET full_name='', user_address='', postal_code='', username='', user_password='' WHERE user_id=@userId;";
            using (SqlConnection conn = new SqlConnection(MainPage.getConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("userId", selectedUser.UserID);
                conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MainPage.Notice("User is deleted !!!");
                }
                else
                    MainPage.Notice("User is NOT deleted!!!");

                cmd.Dispose();
                conn.Close();

            }
            UpdateAdminPageInfo();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListLV.SelectedIndex == -1)
            {
                MainPage.Notice("Please select order");
                return;
            }
            string query = "DELETE FROM orders WHERE order_id = @order_id;";
            using (SqlConnection conn = new SqlConnection(MainPage.getConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("order_id", selectedOrder.orderId);
                conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MainPage.Notice("Order is deleted !!!");
                }
                else
                    MainPage.Notice("Order is NOT deleted!!!");

                cmd.Dispose();
                conn.Close();

            }
            UpdateAdminPageInfo();
        }

        private void OrderListLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrder = (Order)(((ListView)sender).SelectedItem);
        }

        private void returnLoginPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
