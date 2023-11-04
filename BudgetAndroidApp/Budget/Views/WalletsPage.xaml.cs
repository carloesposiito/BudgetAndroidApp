using Budget.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Button = Xamarin.Forms.Button;
using ListView = Xamarin.Forms.ListView;

namespace Budget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletsPage : ContentPage
    {
        public WalletsPage()
        {
            InitializeComponent();
            CreatePage();
        }

        /// <summary>
        /// Create page with data
        /// </summary>
        void CreatePage()
        {
            // Clear section
            StackLayout_Content.Children.Clear();

            // Obtain wallet list
            List<Wallet> wallets = App.Database.GetWalletsAsync().Result;

            // Wallet data row template
            DataTemplate walletTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label
                {
                    FontSize = 15,
                    HeightRequest = 25,
                    TextColor = Color.White,
                    VerticalTextAlignment = TextAlignment.Center
                };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var openEditPageButton = new Button
                {
                    BackgroundColor = Color.Transparent,
                    HeightRequest = 25,
                    ImageSource = ImageSource.FromResource("Budget.Resources.editIcon48.png"),
                    WidthRequest = 25
                };
                openEditPageButton.SetBinding(Button.CommandParameterProperty, "Id");
                openEditPageButton.Clicked += OpenEditWallet;

                // Create grid
                var grid = new Grid { Margin = new Thickness(0, 10) };

                // Add column
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                // Add items to their column
                grid.Children.Add(nameLabel, 0, 0);
                grid.Children.Add(openEditPageButton, 1, 0);

                return new ViewCell { View = grid };
            });

            // Create listview
            ListView listView = new ListView();
            listView.ItemsSource = wallets;
            listView.ItemTemplate = walletTemplate;
            listView.HasUnevenRows = true;
            listView.HeightRequest = 45 * (wallets.Count + 1.5);
            listView.RowHeight = 45;
            listView.SelectionMode = ListViewSelectionMode.None;
            listView.VerticalOptions = LayoutOptions.StartAndExpand;
            listView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

            // Add title and movements section
            StackLayout_Content.Children.Add(listView);
        }

        /// <summary>
        /// Occurs when page becames visible
        /// </summary>
        protected override void OnAppearing()
        {
            CreatePage();
        }

        /// <summary>
        /// Opens the windows to create wallet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void OpenCreateWallet(object sender, EventArgs e)
        {
            EditWalletPage editWalletPage = new EditWalletPage();
            await Navigation.PushAsync(editWalletPage);
        }

        /// <summary>
        /// Opens the window to edit wallet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OpenEditWallet(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;

            int walletId = (int)clickedButton.CommandParameter;

            Wallet wallet = App.Database.GetWalletAsync(walletId).Result;

            EditWalletPage editWalletPage = new EditWalletPage(wallet.Name, wallet.Amount.ToString(), walletId, wallet.Visible);
            await Navigation.PushAsync(editWalletPage);
        }

    }
}