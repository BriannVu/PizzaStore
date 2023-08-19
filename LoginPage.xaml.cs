using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Data.SqlClient;
using Windows.UI.Popups;
using Microsoft.Extensions.Configuration;
using BaoNamVu_SpringFinalProject.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BaoNamVu_SpringFinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static User currentUser;
        public static Boolean isLogin;
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(3000, 2000);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
        public static string getConnectionString()
        {
            ConfigurationBuilder cb = new ConfigurationBuilder();
            cb.SetBasePath(Directory.GetCurrentDirectory());
            cb.AddXmlFile("connectionString.xml");
            IConfiguration config = cb.Build();
            return config.GetChildren().First().Value;
        }
        public static async void Notice(string msg)
        {
            MessageDialog noticeMsg = new MessageDialog(msg);
            await noticeMsg.ShowAsync();
        }
        private List<RegisteredUser> GetUserList()
        {
            List<RegisteredUser> userList = new List<RegisteredUser>();

            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = getConnectionString();
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                string query = "SELECT * FROM users;";
                cmd.CommandText = query;
                conn.Open();

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
                    userList.Add(newUser);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                Notice(message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return userList;
        }
        private void InsertUserToDatabase(RegisteredUser newUser)
        {
            string query = "INSERT INTO users(full_name,user_address, postal_code, username, user_password) VALUES (@FullName,@UserAddress,@PostalCode,@UserName,@UserPassword)";
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("FullName", newUser.FullName);
                cmd.Parameters.AddWithValue("UserAddress", newUser.UserAddress);
                cmd.Parameters.AddWithValue("PostalCode", newUser.PostalCode);
                cmd.Parameters.AddWithValue("UserName", newUser.UserName);
                cmd.Parameters.AddWithValue("UserPassword", newUser.UserPassword);
                conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Notice("New user is successfully created");
                }
                else
                    Notice("New user is NOT created");

                cmd.Dispose();
                conn.Close();
            }
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Check the login information
            List<RegisteredUser> userList = GetUserList();
            if (userList.Count == 0)
            {
                Notice("No users in the data or Database is not connected");
                return;
            }
            if (userNameTextBox.Text == "" || passwordTextBox.Password == "")
            {
                Notice("Please enter username/password !!!");
                return;
            }

            foreach (RegisteredUser user in userList)
            {
                if (user.UserName == userNameTextBox.Text)
                {
                    if (user.UserPassword == passwordTextBox.Password)
                    {
                        currentUser = user;
                        isLogin = true;
                        this.Frame.Navigate(typeof(AfterLoginPage));
                        return;
                    }
                    else
                    {
                        Notice("Your username/password is wrong!");
                        return;
                    }

                }
            }
            Notice("Your username/password is wrong!");
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            List<RegisteredUser> userList = GetUserList();
            foreach (RegisteredUser user in userList)
            {
                if (user.UserName == registeredUserNameTextBox.Text)
                {
                    Notice("The username is already used");
                    return;
                }
            }
            RegisteredUser newUser = new RegisteredUser();
            newUser.FullName = registeredFullNameTextBox.Text;
            newUser.UserAddress = registeredAddressTextBox.Text;
            newUser.PostalCode = registeredPostalCodeTextBox.Text;
            newUser.UserName = registeredUserNameTextBox.Text;
            newUser.UserPassword = registeredPasswordTextBox.Password;
            newUser.UserID = GetUserList().Count + 1;

            currentUser = newUser;
            InsertUserToDatabase(newUser);

            registeredFullNameTextBox.Text = "";
            registeredAddressTextBox.Text = "";
            registeredPostalCodeTextBox.Text = "";
            registeredUserNameTextBox.Text = "";
            registeredPasswordTextBox.Password = "";
        }

        private void loginAsAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (userNameTextBox.Text == "" || passwordTextBox.Password == "")
            {
                Notice("Please enter username/password !!!");
                return;
            }
            List<RegisteredUser> userList = GetUserList();
            if (userList.Count == 0)
            {
                Notice("No users in the data - This noti for testing");
                return;
            }
            foreach (RegisteredUser user in userList)
            {
                if (user.UserName == userNameTextBox.Text)
                {
                    if (user.UserPassword == passwordTextBox.Password)
                    {
                        if (user.UserName == "admin")
                        {
                            currentUser = new AdminUser();
                            ((AdminUser)currentUser).FullName = user.FullName;
                            ((AdminUser)currentUser).userName = user.UserName;
                            ((AdminUser)currentUser).userPassword = user.UserPassword;
                            ((AdminUser)currentUser).userID = user.UserID;
                            ((AdminUser)currentUser).UserAddress = user.UserAddress;
                            ((AdminUser)currentUser).PostalCode= user.PostalCode;
                            this.Frame.Navigate(typeof(adminPage));
                            return;
                        }
                        else
                        {
                            Notice("The input account is not an admin account!!!");
                            return;
                        }
                    }
                    else
                    {
                        Notice("Your username/password is wrong!");
                        return;
                    }
                }
            }
            Notice("Your username/password is wrong!");
        }

        private void loginAsGuestButton_Click(object sender, RoutedEventArgs e)
        {
            isLogin = false;
            currentUser = new UnregisteredUser();
            this.Frame.Navigate(typeof(AfterLoginPage));
        }
    }
}
