using Budget.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Budget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWalletPage : ContentPage
    {
        int id;
        bool visible;

        public EditWalletPage()
        {
            InitializeComponent();

            Label_Title.Text = "Crea conto";
            WalletName.Text = WalletAmount.Text = string.Empty;
            Button_Edit.Text = "Crea";
            Button_Edit.Clicked -= EditWallet;
            Button_Edit.Clicked += CreateWallet;
            Button_Delete.IsVisible = false;
        }

        public EditWalletPage(string name, string amount, int id, bool visible)
        {
            InitializeComponent();

            Label_Title.Text = name;
            WalletName.Text = name;
            WalletAmount.Text = amount;
            this.id = id;
            this.visible = visible;
        }

        /// <summary>
        /// Occurs when page disappears
        /// </summary>
        protected override void OnDisappearing()
        {
            GoToPreviousPage();
        }

        /// <summary>
        /// Add wallet to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void CreateWallet(object sender, EventArgs e)
        {
            float result;

            if (!string.IsNullOrWhiteSpace(WalletName.Text) && !string.IsNullOrWhiteSpace(WalletAmount.Text) && float.TryParse(WalletAmount.Text, out result))
            {
                await App.Database.SaveWalletAsync(new Wallet
                {
                    Name = WalletName.Text,
                    Amount = result,
                    Visible = true,
                });
                GoToPreviousPage();
            }
        }

        /// <summary>
        /// Edit wallet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void EditWallet(object sender, EventArgs e)
        {
            float result;

            if (!string.IsNullOrWhiteSpace(WalletName.Text) && !string.IsNullOrWhiteSpace(WalletAmount.Text) && float.TryParse(WalletAmount.Text, out result))
            {
                await App.Database.SaveWalletAsync(new Wallet
                {
                    Id = id,
                    Name = WalletName.Text,
                    Amount = result,
                    Visible = visible
                });
                GoToPreviousPage();
            }
            
        }

        /// <summary>
        /// Delete wallet and related movements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteWallet(object sender, EventArgs e)
        {
            Wallet wallet = App.Database.GetWalletAsync(id).Result;

            App.Database.DeleteWalletAsync(wallet);

            foreach (Movement movement in App.Database.GetMovementsAsync().Result)
            {
                if (movement.WalletId == id)
                {
                    App.Database.DeleteMovementAsync(movement);
                }
            }
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