using Budget.Converters;
using Budget.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

namespace Budget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

            CreatePage();
        }

        void CreatePage()
        {
            // Clear section
            StackLayout_Content.Children.Clear();

            // Obtain wallet list
            List<Wallet> wallets = App.Database.GetWalletsAsync().Result;

            var title = new Label
            {
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = new Thickness(0, 20, 0, 0),
                Text = "Modifica visibilità dei conti:",
                TextColor = Color.Gray
            };
            StackLayout_Content.Children.Add(title);

            // Row template
            DataTemplate template = new DataTemplate(() =>
            {
                var nameLabel = new Label
                {
                    FontSize = 15,
                    HeightRequest = 25,
                    TextColor = Color.White,
                    VerticalTextAlignment = TextAlignment.Center,
                    WidthRequest = 10
                };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var visibleButton = new Button
                {
                    BackgroundColor = Color.Transparent
                };
                visibleButton.SetBinding(Button.CommandParameterProperty, "Id");
                visibleButton.SetBinding(Button.TextProperty, new Binding("Visible", converter: new BooleanToStringConverter()));
                visibleButton.Clicked += SetVisibility;

                // Create grid
                var grid = new Grid { Margin = new Thickness(0, 10) };

                // Add column
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                // Add items to their column
                grid.Children.Add(nameLabel, 0, 0);
                grid.Children.Add(visibleButton, 1, 0);

                return new ViewCell { View = grid };
            });

            // Create listview
            ListView listView = new ListView();
            listView.ItemsSource = wallets;
            listView.ItemTemplate = template;
            listView.HasUnevenRows = true;
            listView.HeightRequest = 50 * (wallets.Count + 1.5);
            listView.RowHeight = 50;
            listView.SelectionMode = ListViewSelectionMode.None;
            listView.VerticalOptions = LayoutOptions.StartAndExpand;
            listView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

            // Add listview section
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
        /// Set wallet visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void SetVisibility(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;

            // Get wallet id from CommandParameter property
            int movementId = (int)clickedButton.CommandParameter;

            // Get wallet data
            Wallet wallet = App.Database.GetWalletAsync(movementId).Result;

            if (wallet.Visible == true)
            {
                wallet.Visible = false;
            }
            else
            {
                wallet.Visible = true;
            }

            // Save changes on database
            await App.Database.SaveWalletAsync(new Wallet
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Amount = wallet.Amount,
                Visible = wallet.Visible
            });

            CreatePage();
        }

        /// <summary>
        /// Opens the about page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OpenAboutPage(object sender, EventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            await Navigation.PushAsync(aboutPage);
        }

    }
}