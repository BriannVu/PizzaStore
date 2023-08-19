using BaoNamVu_SpringFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class AfterLoginPage : Page
    {
        private List<Item> itemsCart = new List<Item>();
        private List<Pizza> pizzas = new List<Pizza>();
        private List<Wing> wings = new List<Wing>();
        private List<Sandwich> sandwiches = new List<Sandwich>();
        private List<Poutine> poutines = new List<Poutine>();
        private List<GulabJamun> gulabJamuns = new List<GulabJamun>();
        private double[] priceList = new double[5];
        private Card userCard = new Card();
        private Order newOrder = new Order();
        private Combo[] popularCombos = new Combo[4];

        private MainPage mainPage = new MainPage();
        public AfterLoginPage()
        {
            this.InitializeComponent();
            this.loadItemToPage();
            this.UpdateUserInformationToPage();
            this.UpdateSuggestionToPage();
            this.updatePopularCombo();
        }

        private void UpdateSuggestionToPage()
        {
            if (MainPage.isLogin)
            {
                SqlConnection conn = new SqlConnection();

                conn.ConnectionString = MainPage.getConnectionString();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Parameters.AddWithValue("userId", ((RegisteredUser)(MainPage.currentUser)).UserID);
                try
                {
                    string query = "SELECT * FROM orders WHERE user_id = @userId;";
                    cmd.CommandText = query;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = -1;
                    while (reader.Read())
                    {
                        count++;
                        if (count == 0)
                        {
                            suggestionTextBox.Visibility = Visibility.Visible;
                            suggestion1TextBlock.Text = "Date: " + reader[2].ToString().Substring(0, 10) + " | " + reader[3].ToString();
                            suggestion1TextBlock.Visibility = Visibility.Visible;
                        }
                        else if (count == 1)
                        {
                            suggestion2TextBlock.Text = "Date: " + reader[2].ToString().Substring(0, 10) + " | " + reader[3].ToString();
                            suggestion2TextBlock.Visibility = Visibility.Visible;
                        }
                        else if (count == 2)
                        {
                            suggestion3TextBlock.Text = "Date: " + reader[2].ToString().Substring(0, 10) + " | " + reader[3].ToString();
                            suggestion3TextBlock.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            break;
                        }

                    }
                    conn.Close();
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
            else
            {
                fullNameInformationTextBox.IsReadOnly = false;
                addressInformationTextBox.IsReadOnly = false;
                postalCodeInformationTextBox.IsReadOnly = false;
            }
        }
        private void UpdateUserInformationToPage()
        {
            if (MainPage.isLogin)
            {
                WelcomeTextBlock.Text += ((RegisteredUser)(MainPage.currentUser)).UserName;
                fullNameInformationTextBox.Text = MainPage.currentUser.FullName;
                addressInformationTextBox.Text = MainPage.currentUser.UserAddress;
                postalCodeInformationTextBox.Text = MainPage.currentUser.PostalCode;
                changeAIButton.Visibility = Visibility.Visible;
            }
            else
            {
                WelcomeTextBlock.Text += "- GUEST LOGIN";
            }
        }

        private void addItemPizzaButtonClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            foreach (Pizza pizza in pizzas)
            {
                if (pizza.Id == Convert.ToInt32(bt.Tag.ToString()))
                {
                    if (pizza.Quantity == 0 || pizza.Topping == null || pizza.Size == null)
                    {
                        MainPage.Notice("Please fill all the needed box!!!");
                        return;
                    }
                    foreach (Item pizzaInCart in itemsCart)
                    {
                        if (pizzaInCart.Id == pizza.Id)
                        {
                            if (((Pizza)pizzaInCart).Topping == pizza.Topping && ((Pizza)pizzaInCart).Size == pizza.Size)
                            {

                                ((Pizza)pizzaInCart).Quantity += pizza.Quantity;
                                updateCart();
                                return;
                            }
                        }
                    }
                    Pizza newPizza = new Pizza();
                    newPizza.Id = pizza.Id;
                    newPizza.Name = pizza.Name;
                    newPizza.Quantity = pizza.Quantity;
                    newPizza.Topping = pizza.Topping;
                    newPizza.Size = pizza.Size;
                    itemsCart.Add(newPizza);
                    updateCart();
                    return;
                }
            }
        }
        public void loadItemToPage()
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = MainPage.getConnectionString();
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                string query = "SELECT * FROM items WHERE UPPER(item_name) LIKE '%PIZZA%'";
                cmd.CommandText = query;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pizza newPizza = new Pizza();

                    newPizza.Id = Convert.ToInt32(reader[0].ToString());
                    newPizza.Name = reader[1].ToString();
                    newPizza.ImgUrl = reader[2].ToString();
                    pizzas.Add(newPizza);
                }
                conn.Close();


                query = "SELECT * FROM items WHERE UPPER(item_name) LIKE '%SANDWICH%'";
                cmd.CommandText = query;
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sandwich newSandwich = new Sandwich();

                    newSandwich.Id = Convert.ToInt32(reader[0].ToString());
                    newSandwich.Name = reader[1].ToString();
                    newSandwich.ImgUrl = reader[2].ToString();
                    sandwiches.Add(newSandwich);
                }
                conn.Close();


                query = "SELECT * FROM items WHERE UPPER(item_name) LIKE '%POUTINE%'";
                cmd.CommandText = query;
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Poutine newPoutine = new Poutine();

                    newPoutine.Id = Convert.ToInt32(reader[0].ToString());
                    newPoutine.Name = reader[1].ToString();
                    newPoutine.ImgUrl = reader[2].ToString();
                    poutines.Add(newPoutine);
                }
                conn.Close();


                query = "SELECT * FROM items WHERE UPPER(item_name) LIKE '%WING%'";
                cmd.CommandText = query;
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Wing newWing = new Wing();

                    newWing.Id = Convert.ToInt32(reader[0].ToString());
                    newWing.Name = reader[1].ToString();
                    newWing.ImgUrl = reader[2].ToString();
                    wings.Add(newWing);
                }
                conn.Close();


                query = "SELECT * FROM items WHERE UPPER(item_name) LIKE '%GULAB JAMUN%'";
                cmd.CommandText = query;
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    GulabJamun newGulabJamun = new GulabJamun();

                    newGulabJamun.Id = Convert.ToInt32(reader[0].ToString());
                    newGulabJamun.Name = reader[1].ToString();
                    newGulabJamun.ImgUrl = reader[2].ToString();
                    gulabJamuns.Add(newGulabJamun);
                }
                conn.Close();

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

        private void sizeSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBox selectedComboBox = (ComboBox)sender;
            foreach (Pizza pizza in pizzas)
            {
                if (pizza.Id == Convert.ToInt32(selectedComboBox.Tag.ToString()))
                {
                    ComboBoxItem selectedBoxItem = (ComboBoxItem)selectedComboBox.SelectedItem;
                    pizza.Size = selectedBoxItem.Content.ToString();
                    return;
                }
            }
        }

        private void toppingSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBox selectedComboBox = (ComboBox)sender;
            foreach (Pizza pizza in pizzas)
            {
                if (pizza.Id == Convert.ToInt32(selectedComboBox.Tag.ToString()))
                {
                    ComboBoxItem selectedBoxItem = (ComboBoxItem)selectedComboBox.SelectedItem;
                    pizza.Topping = selectedBoxItem.Content.ToString();
                    return;
                }
            }
        }


        private void quantityBoxChange(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            TextBox tb = (TextBox)sender;
            int num;
            if (int.TryParse(tb.Text, out num))
            {
                foreach (Pizza pizza in pizzas)
                {
                    if (pizza.Id == Convert.ToInt32(tb.Tag.ToString()))
                    {
                        pizza.Quantity = Convert.ToInt32(tb.Text);
                        return;
                    }
                }
            }
            else
            {
                MainPage.Notice("Quantity box only takes number!!!");
                tb.Text = "";
            }
        }

        private void addItemWingButtonClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            foreach (Wing wing in wings)
            {
                if (wing.Id == Convert.ToInt32(bt.Tag.ToString()))
                {
                    if (wing.Quantity == 0)
                    {
                        MainPage.Notice("Please fill all the needed box!!!");
                        return;
                    }
                    foreach (Item wingInCart in itemsCart)
                    {
                        if (wingInCart.Id == Convert.ToInt32(bt.Tag.ToString()))
                        {
                            wingInCart.Quantity += wing.Quantity;
                            updateCart();
                            return;
                        }
                    }
                    itemsCart.Add(wing);
                    updateCart();
                    return;
                }
            }

        }

        private void quantityWingBoxChange(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            TextBox tb = (TextBox)sender;
            int num;
            if (int.TryParse(tb.Text, out num))
            {
                foreach (Wing wing in wings)
                {
                    if (wing.Id == Convert.ToInt32(tb.Tag.ToString()))
                    {
                        wing.Quantity = Convert.ToInt32(tb.Text);
                        return;
                    }
                }
            }
            else
            {
                MainPage.Notice("Quantity box only takes number!!!");
                tb.Text = "";
            }
        }
        private void quantitySandwichBoxChange(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            TextBox tb = (TextBox)sender;
            int num;
            if (int.TryParse(tb.Text, out num))
            {
                foreach (Sandwich sandwich in sandwiches)
                {
                    if (sandwich.Id == Convert.ToInt32(tb.Tag.ToString()))
                    {
                        sandwich.Quantity = Convert.ToInt32(tb.Text);
                        return;
                    }
                }
            }
            else
            {
                MainPage.Notice("Quantity box only takes number!!!");
                tb.Text = "";
            }
        }

        private void addItemSandwichButtonClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            foreach (Sandwich sandwich in sandwiches)
            {
                if (sandwich.Id == Convert.ToInt32(bt.Tag.ToString()))
                {
                    if (sandwich.Quantity == 0)
                    {
                        MainPage.Notice("Please fill all the needed box!!!");
                        return;
                    }
                    foreach (Item sandwichInCart in itemsCart)
                    {
                        if (sandwichInCart.Id == Convert.ToInt32(bt.Tag.ToString()))
                        {
                            sandwichInCart.Quantity += sandwich.Quantity;
                            updateCart();
                            return;
                        }
                    }
                    itemsCart.Add(sandwich);
                    updateCart();
                    return;
                }
            }

        }
        private void quantityPoutineBoxChange(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            TextBox tb = (TextBox)sender;
            int num;
            if (int.TryParse(tb.Text, out num))
            {
                foreach (Poutine poutine in poutines)
                {
                    if (poutine.Id == Convert.ToInt32(tb.Tag.ToString()))
                    {
                        poutine.Quantity = Convert.ToInt32(tb.Text);
                        return;
                    }
                }
            }
            else
            {
                MainPage.Notice("Quantity box only takes number!!!");
                tb.Text = "";
            }
        }

        private void addItemPoutineButtonClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            foreach (Poutine poutine in poutines)
            {
                if (poutine.Id == Convert.ToInt32(bt.Tag.ToString()))
                {
                    if (poutine.Quantity == 0)
                    {
                        MainPage.Notice("Please fill all the needed box!!!");
                        return;
                    }
                    foreach (Item poutineInCart in itemsCart)
                    {
                        if (poutineInCart.Id == Convert.ToInt32(bt.Tag.ToString()))
                        {
                            poutineInCart.Quantity += poutine.Quantity;
                            updateCart();
                            return;
                        }
                    }
                    itemsCart.Add(poutine);
                    updateCart();
                    return;
                }
            }

        }
        private void quantityGulabJamunBoxChange(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            TextBox tb = (TextBox)sender;
            int num;
            if (int.TryParse(tb.Text, out num))
            {
                foreach (GulabJamun gulabJamun in gulabJamuns)
                {
                    if (gulabJamun.Id == Convert.ToInt32(tb.Tag.ToString()))
                    {
                        gulabJamun.Quantity = Convert.ToInt32(tb.Text);
                        return;
                    }
                }
            }
            else
            {
                MainPage.Notice("Quantity box only takes number!!!");
                tb.Text = "";
            }
        }

        private void addItemGulabJamunButtonClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            foreach (GulabJamun gulabJamun in gulabJamuns)
            {
                if (gulabJamun.Id == Convert.ToInt32(bt.Tag.ToString()))
                {
                    if (gulabJamun.Quantity == 0)
                    {
                        MainPage.Notice("Please fill all the needed box!!!");
                        return;
                    }
                    foreach (Item gulabJamunInCart in itemsCart)
                    {
                        if (gulabJamunInCart.Id == Convert.ToInt32(bt.Tag.ToString()))
                        {
                            gulabJamunInCart.Quantity += gulabJamun.Quantity;
                            updateCart();
                            return;
                        }
                    }
                    itemsCart.Add(gulabJamun);
                    updateCart();
                    return;
                }
            }

        }

        private void updateCart()
        {
            double subtotal = 0;
            if (itemsCart.Count == 0)
            {
                priceList[0] = 0;
                priceList[1] = 0;
                priceList[2] = 0;
                priceList[3] = 0;
                priceList[4] = 0;
            }
            foreach (Item item in itemsCart)
            {
                if (item.Name.ToUpper().Contains("PIZZA"))
                {
                    if (((Pizza)item).Size.ToUpper() == "LARGE")
                    {
                        item.TotalPrice = 30 * item.Quantity;
                    }
                    else
                    {
                        item.TotalPrice = 20 * item.Quantity;
                    }
                }
                else
                {
                    item.TotalPrice = item.Quantity * 10;
                }
                subtotal += item.TotalPrice;
                priceList[0] = subtotal;
            }
            if (MainPage.isLogin)
            {
                priceList[1] = -(priceList[0] / 100 * 10);
            }
            else
            {
                priceList[1] = 0;
            }
            priceList[2] = Math.Round((priceList[0] + priceList[1]) / 100 * 13, 2);
            if (priceList[0] > 50)
            {
                priceList[3] = 0;
            }
            else
            {
                priceList[3] = 15;
            }
            priceList[4] = Math.Round(priceList[0] + priceList[1] + priceList[2] + priceList[3], 2);

            totalTextBox.Text = priceList[0].ToString();
            discountTextBox.Text = priceList[1].ToString();
            taxTextBox.Text = priceList[2].ToString();
            shippingTextBox.Text = priceList[3].ToString();
            grandTotalTextBox.Text = "$ " + priceList[4].ToString();

            itemCartGridView.ItemsSource = null;
            itemCartGridView.ItemsSource = itemsCart;


        }

        private void changeAIButton_Click(object sender, RoutedEventArgs e)
        {
            fullNameInformationTextBox.IsReadOnly = false;
            addressInformationTextBox.IsReadOnly = false;
            postalCodeInformationTextBox.IsReadOnly = false;
            changeAIButton.Visibility = Visibility.Collapsed;
            changeButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            if (fullNameInformationTextBox.Text == "" || addressInformationTextBox.Text == "" || postalCodeInformationTextBox.Text == "")
            {
                MainPage.Notice("Please enter all required input!!!");
            }
            else
            {
                updateUserInformation();
                string query = "UPDATE users SET full_name = @FullName, user_address = @UserAddress, postal_code=@PostalCode WHERE user_id = @userId;";
                using (SqlConnection conn = new SqlConnection(MainPage.getConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("FullName", MainPage.currentUser.FullName);
                    cmd.Parameters.AddWithValue("UserAddress", MainPage.currentUser.UserAddress);
                    cmd.Parameters.AddWithValue("PostalCode", MainPage.currentUser.PostalCode);
                    cmd.Parameters.AddWithValue("userId", ((RegisteredUser)(MainPage.currentUser)).UserID);
                    conn.Open();

                    int result = cmd.ExecuteNonQuery();
                    if (result == 1)
                    {
                        MainPage.Notice("User Information is updated successfully");
                    }
                    else
                        MainPage.Notice("User Information is NOT updated");

                    cmd.Dispose();
                    conn.Close();
                }
                fullNameInformationTextBox.IsReadOnly = true;
                addressInformationTextBox.IsReadOnly = true;
                postalCodeInformationTextBox.IsReadOnly = true;
                changeAIButton.Visibility = Visibility.Visible;
                changeButton.Visibility = Visibility.Collapsed;
                cancelButton.Visibility = Visibility.Collapsed;
            }
        }
        private void updateUserInformation()
        {
            MainPage.currentUser.FullName = fullNameInformationTextBox.Text;
            MainPage.currentUser.UserAddress = addressInformationTextBox.Text;
            MainPage.currentUser.PostalCode = postalCodeInformationTextBox.Text;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            fullNameInformationTextBox.IsReadOnly = true;
            addressInformationTextBox.IsReadOnly = true;
            postalCodeInformationTextBox.IsReadOnly = true;
            changeAIButton.Visibility = Visibility.Visible;
            changeButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
        }

        private void backToLoginPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (fullNameInformationTextBox.Text == "" || addressInformationTextBox.Text == "" || postalCodeInformationTextBox.Text == "" || nameOnCardTBox.Text == "" ||
                creadirCardNumberTBox.Text == "" || monthNumberTBox.Text == "" || yearNumberTBox.Text == "" || cvcNumberTBox.Text == "")
            {
                MainPage.Notice("Please enter your personal information and Card Number");
            }
            else if (itemsCart.Count == 0)
            {
                MainPage.Notice("Please add item to cart");
            }
            else
            {
                int num2, num3, num4;
                double num;
                if (double.TryParse(creadirCardNumberTBox.Text, out num) && int.TryParse(monthNumberTBox.Text, out num2) && int.TryParse(monthNumberTBox.Text, out num3) && int.TryParse(cvcNumberTBox.Text, out num4))
                {
                    userCard.nameOnCard = nameOnCardTBox.Text;
                    userCard.cardNumber = num;
                    userCard.mmyy = Convert.ToInt32("" + num2 + num3);
                    userCard.cvc = num4;
                    updateUserInformation();
                    checkOutConfirmMessage();
                }
                else
                {
                    MainPage.Notice("Please enter number format in card-information box !!!");
                }
            }
        }
        private async void checkOutConfirmMessage()
        {
            string msg = MainPage.currentUser.ToString()
                + Environment.NewLine + "Your total bill is: " + priceList[4].ToString()
                + Environment.NewLine + userCard.ToString();
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Are you sure to order?",
                Content = msg,
                PrimaryButtonText = "Pay",
                CloseButtonText = "Cancel"
            };
            ContentDialogResult result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                createOrderToDataBase();
            }
        }
        private async void createOrderToDataBase()
        {
            string query = "INSERT INTO orders(user_id, order_date, order_detail, order_address, total_amount) VALUES " +
                "(@userId,@orderDate,@orderDetails,@orderAddress,@totalAmount)";
            using (SqlConnection conn = new SqlConnection(MainPage.getConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (MainPage.isLogin)
                {
                    newOrder.userId = ((RegisteredUser)MainPage.currentUser).UserID;
                }
                else
                {
                    newOrder.userId = 1;
                }
                newOrder.orderDate = DateTime.Now;
                newOrder.orderDetails = "";
                foreach (Item item in itemsCart)
                {
                    if (item.Name.ToUpper().Contains("PIZZA"))
                    {
                        newOrder.orderDetails += ((Pizza)item).ToString() + " | ";
                    }
                    else
                    {
                        newOrder.orderDetails += (item).ToString() + " | ";
                    }
                }
                newOrder.orderAddress = addressInformationTextBox.Text;
                newOrder.orderTotalAmount = priceList[4];

                cmd.Parameters.AddWithValue("userId", newOrder.userId);
                cmd.Parameters.AddWithValue("orderDate", newOrder.orderDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.Parameters.AddWithValue("orderDetails", newOrder.orderDetails);
                cmd.Parameters.AddWithValue("orderAddress", newOrder.orderAddress);
                cmd.Parameters.AddWithValue("totalAmount", newOrder.orderTotalAmount);
                conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    ContentDialog confirmDialog = new ContentDialog
                    {
                        Title = "New order is successfully created !!!",
                        Content = "Do you want to print the bill?",
                        PrimaryButtonText = "Print",
                        CloseButtonText = "Cancel"
                    };
                    ContentDialogResult resultChoose = await confirmDialog.ShowAsync();
                    if (resultChoose == ContentDialogResult.Primary)
                    {
                        printBill();
                    }
                    this.Frame.Navigate(typeof(ThankYouPage));
                }
                else
                    MainPage.Notice("New order is NOT created !!!");

                cmd.Dispose();
                conn.Close();

            }
        }
        private async void printBill()
        {
            string items = "";
            foreach (Item item in itemsCart)
            {
                items += item.ToString() + " | ";
            }
            string billContent =
                "Name: " + fullNameInformationTextBox.Text + "\n" +
                "Order Date: " + newOrder.orderDate.ToString() + "\n" +
                "Address: " + newOrder.orderAddress + "\n" +
                "Items: " + items + "\n" +
                "Total amount: " + newOrder.orderTotalAmount.ToString() + "\n";

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "PapaDarioPizzaBill_" + MainPage.currentUser.FullName + "_" + newOrder.orderDate.ToString().Substring(0, 10);
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file, billContent);
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    MainPage.Notice("Your bill was saved.");
                }
                else
                {
                    MainPage.Notice("Sorry, your File " + file.Name + " couldn't be saved.");
                }
            }
            else
            {
                MainPage.Notice("You have cancelled printing the bill !\nThank You for ordering our food!!!");
            }

        }
        private void updatePopularCombo()
        {
            //First Combo
            popularCombos[0] = new Combo();
            popularCombos[0].comboItems = new Item[3];

            Pizza pizza1 = new Pizza();
            pizza1.Quantity = 1;
            pizza1.Name = pizzas[0].Name;
            pizza1.Size = "Medium";
            pizza1.Topping = "Onion";

            GulabJamun g1 = new GulabJamun();
            g1.Quantity = 1;
            g1.Name = gulabJamuns[0].Name;

            pizza1.Quantity = 1;
            popularCombos[0].comboItems[0] = pizza1;
            popularCombos[0].comboItems[1] = g1;

            //Second combo

            popularCombos[1] = new Combo();
            popularCombos[1].comboItems = new Item[3];

            Pizza pizza2 = new Pizza();
            pizza2.Quantity = 1;
            pizza2.Name = pizzas[1].Name;
            pizza2.Size = "Large";
            pizza2.Topping = "Onion";

            Sandwich s2 = new Sandwich();
            s2.Quantity = 2;
            s2.Name = sandwiches[0].Name;
            Poutine p2 = new Poutine();
            p2.Quantity = 2;
            p2.Name = poutines[0].Name;

            pizza1.Quantity = 1;
            popularCombos[1].comboItems[0] = pizza1;
            popularCombos[1].comboItems[1] = s2;
            popularCombos[1].comboItems[2] = p2;

            foreach (Combo cb in popularCombos)
            {
                if (cb == null) break;
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = "";
                foreach (Item item in cb.comboItems)
                {
                    if (item == null) break;
                    cbi.Content += item.ToString() + " ";
                }
                popularComboCB.Items.Add(cbi);
            }
        }
        private void addPopularComboButton_Click(object sender, RoutedEventArgs e)
        {
            for (int index = 0; index < popularCombos.Length; index++)
            {
                if (index == popularComboCB.SelectedIndex)
                {
                    foreach (Item newItem in popularCombos[index].comboItems)
                    {
                        if (newItem == null) break;
                        itemsCart.Add(newItem);
                        updateCart();
                    }
                }
            }
        }

        private void clearCartButton_Click(object sender, RoutedEventArgs e)
        {
            itemsCart.Clear();
            updateCart();
        }
    }
}
