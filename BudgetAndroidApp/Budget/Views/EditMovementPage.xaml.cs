using Budget.Model;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

namespace Budget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMovementPage : ContentPage
    {
        int id;
        int walletId;
        string value;
        string type;

        public EditMovementPage()
        {
            InitializeComponent();

            Label_Title.Text = "Crea movimento";
            MovementDescription.Text = MovementValue.Text = string.Empty;
            MovementDate.Date = DateTime.Now;
            RB_Draft.IsChecked = true;
            Button_Edit.Text = "Crea";
            Button_Edit.Clicked -= EditMovement;
            Button_Edit.Clicked += CreateMovement;
            Button_Confirm.IsVisible = false;
            Button_Delete.IsVisible = false;

            UpdatePickerWallets();
        }

        public EditMovementPage(string description, string value, string date, string type, int walletId, int id)
        {
            InitializeComponent();

            MovementDescription.Text = description;
            MovementValue.Text = value;

            DateTime data;
            string[] datePart = date.Split('-');
            string newDate = datePart[1] + "-" + datePart[0];
            string dateFinal = DateTime.Now.Year.ToString() + "-" + newDate;

            // Set italian culture
            CultureInfo culture = new CultureInfo("it-IT");
            if (DateTime.TryParse(dateFinal, culture, DateTimeStyles.None, out data))
            {
                MovementDate.Date = data;
            }

            if (type == "+")
            {
                RB_In.IsChecked = true;
                this.type = type;
            }
            else if (type == "-")
            {
                RB_Out.IsChecked = true;
                this.type = type;
            }
            else
            {
                RB_Draft.IsChecked = true;
                this.type = type;
            }

            Picker_Wallets.IsVisible = false;
            this.id = id;
            this.walletId = walletId;
            this.value = value;
        }

        /// <summary>
        /// Occurs when page disappears
        /// </summary>
        protected override void OnDisappearing()
        {
            GoToPreviousPage();
        }

        /// <summary>
        /// Update wallets in picker
        /// </summary>
        void UpdatePickerWallets()
        {
            Picker_Wallets.ItemsSource = App.Database.GetWalletsAsync().Result;
            Picker_Wallets.ItemDisplayBinding = new Binding("Name");
            Picker_Wallets.SelectedIndex = 0;
        }

        /// <summary>
        /// Edit movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void CreateMovement(object sender, EventArgs e)
        {
            float result;
            string type;

            if (!string.IsNullOrWhiteSpace(MovementDescription.Text) && !string.IsNullOrWhiteSpace(MovementValue.Text) && float.TryParse(MovementValue.Text, out result) && (RB_In.IsChecked || RB_Out.IsChecked || RB_Draft.IsChecked) && Picker_Wallets.SelectedItem != null)
            {
                // Obtaining type
                if (RB_In.IsChecked)
                {
                    type = "+";
                }
                else if (RB_Out.IsChecked)
                {
                    type = "-";
                }
                else
                {
                    type = "/";
                }

                // Obtaining walletId
                Wallet x = (Wallet)Picker_Wallets.SelectedItem;
                int walletId = x.Id;

                // Add movement to database
                await App.Database.SaveMovementAsync(new Movement
                {
                    Description = MovementDescription.Text,
                    Value = result,
                    Date = MovementDate.Date.ToString("dd-MM"),
                    Type = type,
                    WalletId = walletId,
                });
                GoToPreviousPage();
            }
        }

        /// <summary>
        /// Edit movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void EditMovement(object sender, EventArgs e)
        {
            float result;
            string type;

            if (!string.IsNullOrWhiteSpace(MovementDescription.Text) && !string.IsNullOrWhiteSpace(MovementValue.Text) && float.TryParse(MovementValue.Text, out result) && (RB_In.IsChecked || RB_Out.IsChecked || RB_Draft.IsChecked))
            {
                // Obtaining type
                if (RB_In.IsChecked)
                {
                    type = "+";
                }
                else if (RB_Out.IsChecked)
                {
                    type = "-";
                }
                else
                {
                    type = "/";
                }

                // Add movement to database
                await App.Database.SaveMovementAsync(new Movement
                {
                    Id = id,
                    Description = MovementDescription.Text,
                    Value = result,
                    Date = MovementDate.Date.ToString("dd-MM"),
                    Type = type,
                    WalletId = walletId,
                });
                GoToPreviousPage();
            }
        }

        /// <summary>
        /// Confirm movement by adding it to wallet amount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void ConfirmMovement(object sender, EventArgs e)
        {
            float valueToAdd;

            if (float.TryParse(value, out valueToAdd))
            {
                // Get wallet data
                Wallet wallet = App.Database.GetWalletAsync(walletId).Result;

                if (type == "+")
                {
                    wallet.Amount += valueToAdd;
                }
                else if (type == "-")
                {
                    wallet.Amount -= valueToAdd;
                }

                // Update wallet on db
                await App.Database.SaveWalletAsync(new Wallet
                {
                    Id = wallet.Id,
                    Name = wallet.Name,
                    Amount = wallet.Amount,
                    Visible = wallet.Visible
                });

                //Delete movement
                Movement movement = App.Database.GetMovementAsync(id).Result;
                await App.Database.DeleteMovementAsync(movement);

                GoToPreviousPage();
            }
        }

        /// <summary>
        /// Delete movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteMovement(object sender, EventArgs e)
        {
            Movement movement = App.Database.GetMovementAsync(id).Result;

            App.Database.DeleteMovementAsync(movement);

            GoToPreviousPage();
        }

        /// <summary>
        /// Go to previous page
        /// </summary>
        async void GoToPreviousPage()
        {
            await Navigation.PopAsync();
        }

    }
}