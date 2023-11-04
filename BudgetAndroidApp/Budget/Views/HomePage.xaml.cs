using Budget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;

namespace Budget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
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

            if (wallets.Count == 0) 
            {
                // Title
                Label title = new Label();
                title.FontSize = 20;
                title.HorizontalTextAlignment = TextAlignment.Center;
                title.Margin = new Thickness(20, 20, 20, 10);
                title.TextColor = Color.Red;
                title.Text = "Prima devi creare un conto!";

                Button_InsertMovement.IsEnabled = false;
                StackLayout_Content.Children.Add(title);
            } else
            {
                Button_InsertMovement.IsEnabled = true;
            }

            // Obtain movement list
            List<Movement> movements = App.Database.GetMovementsAsync().Result;

            foreach (Wallet wallet in wallets)
            {
                // Get wallet data
                int walletId = wallet.Id;
                string walletName = wallet.Name;
                float walletAmount = wallet.Amount;
                bool walletVisibility = wallet.Visible;

                // Skip if visibility is set to false
                if (!walletVisibility) { continue; }

                // Title
                Label title = new Label();
                title.FontAttributes = FontAttributes.Bold;
                title.FontSize = 25;
                title.HorizontalTextAlignment = TextAlignment.Center;
                title.Margin = new Thickness(20, 20, 20, 10);
                title.TextColor = Color.White;
                title.Text = walletName;
                
                // Add listview to section
                StackLayout_Content.Children.Add(title);

                #region "Create list to be displayed"

                // Get tuple (Item1 is movement list, Item2 is movements sum)
                var data = FilterMovementByWalletId(App.Database.GetMovementsAsync().Result, walletId);

                // Get movements
                List<Movement> filtered = new List<Movement>();
                
                // Wallet data row
                filtered.Add(new Movement(-1, walletAmount, "Saldo iniziale", null, "·", walletId));
                
                // Movement rows
                filtered.AddRange(data.Item1);
                
                // Sum row
                filtered.Add(new Movement(-2, (walletAmount + data.Item2), "Saldo totale", null, "=", walletId));

                // Check if there are movements
                if (filtered.Count == 2)
                {
                    filtered[1].Type = "·";
                    filtered.Remove(filtered[0]);
                }

                #endregion

                #region "Create templates"

                // Wallet data row template
                DataTemplate firstRowDataTemplate = new DataTemplate(() =>
                {
                    var fakeTypeLabel = new Label
                    {
                        FontSize = 15,
                        HeightRequest = 25,
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                        WidthRequest = 10
                    };
                    fakeTypeLabel.SetBinding(Label.TextProperty, "Type");

                    var fakeValueLabel = new Label
                    {
                        FontSize = 15,
                        HeightRequest = 25,
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    fakeValueLabel.SetBinding(Label.TextProperty, "Value");

                    var fakeDescriptionLabel = new Label
                    {
                        FontSize = 15,
                        HeightRequest = 25,
                        Margin = new Thickness(0, 0, 0, 0),
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    fakeDescriptionLabel.SetBinding(Label.TextProperty, "Description");

                    // Create grid
                    var grid = new Grid { Margin = new Thickness(0, 10) };

                    // Add column
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(65, GridUnitType.Absolute) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });

                    // Add items to their column
                    grid.Children.Add(fakeTypeLabel, 0, 0);
                    grid.Children.Add(fakeValueLabel, 1, 0);
                    grid.Children.Add(fakeDescriptionLabel, 2, 0);

                    return new ViewCell { View = grid };
                });

                // Movement rows template
                DataTemplate movementRowDataTemplate = new DataTemplate(() =>
                {
                    var typeLabel = new Label
                    {
                        FontSize = 15,
                        HeightRequest = 25,
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                        WidthRequest = 10
                    };
                    typeLabel.SetBinding(Label.TextProperty, "Type");

                    var valueLabel = new Label
                    {
                        FontSize = 15,
                        HeightRequest = 25,
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    valueLabel.SetBinding(Label.TextProperty, "Value");

                    var descriptionLabel = new Label
                    {
                        FontSize = 15,
                        HeightRequest = 25,
                        Margin = new Thickness(0, 0, 0, 0),
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    descriptionLabel.SetBinding(Label.TextProperty, "Description");

                    var dateLabel = new Label
                    {
                        HeightRequest = 25,
                        TextColor = Color.White,
                        VerticalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(3, 0, 5, 0)
                    };
                    dateLabel.SetBinding(Label.TextProperty, "Date");

                    var openEditPageButton = new Button
                    {
                        BackgroundColor = Color.Transparent,
                        HeightRequest = 25, 
                        ImageSource = ImageSource.FromResource("Budget.Resources.editIcon48.png"),
                        WidthRequest = 25
                    };
                    openEditPageButton.SetBinding(Button.CommandParameterProperty, "Id");
                    openEditPageButton.Clicked += OpenEditMovement;

                    // Create grid
                    var grid = new Grid { Margin = new Thickness(0, 10) };

                    // Add column
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(65, GridUnitType.Absolute) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                    // Add items to their column
                    grid.Children.Add(typeLabel, 0, 0);
                    grid.Children.Add(valueLabel, 1, 0);
                    grid.Children.Add(descriptionLabel, 2, 0);
                    grid.Children.Add(dateLabel, 3, 0);
                    grid.Children.Add(openEditPageButton, 4, 0);

                    return new ViewCell { View = grid };
                });

                #endregion

                // Create template selector object
                var templateSelector = new MyTemplateSelector();
                templateSelector.FirstRow = firstRowDataTemplate;
                templateSelector.MovementsRow = movementRowDataTemplate;

                // Create listview
                ListView listView = new ListView();
                listView.ItemsSource = filtered;
                listView.ItemTemplate = templateSelector;
                listView.HasUnevenRows = true;
                listView.HeightRequest = 45 * (filtered.Count + 1.5);
                listView.RowHeight = 45;
                listView.SelectionMode = ListViewSelectionMode.None;
                listView.VerticalOptions = LayoutOptions.StartAndExpand;
                listView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

                // Add listview to section
                StackLayout_Content.Children.Add(listView);
            }
        }

        /// <summary>
        /// Occurs when page becames visible
        /// </summary>
        protected override void OnAppearing()
        {
            CreatePage();
        }

        /// <summary>
        /// Filter movement by wallet id and return their sum
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public Tuple<List<Movement>,float> FilterMovementByWalletId(List<Movement> movements, int walletId)
        {
            List<Movement> filtered = new List<Movement>();
            float sum = 0;
            foreach (Movement movement in movements)
            {
                if (movement.WalletId == walletId)
                {
                    filtered.Add(movement);

                    if (movement.Type == "+")
                    {
                        sum += movement.Value;
                    } else if (movement.Type == "-")
                    {
                        sum -= movement.Value;
                    }

                }
            }
            return Tuple.Create(filtered, sum);
        }

        /// <summary>
        /// Opens the window to create movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OpenCreateMovement(object sender, EventArgs e)
        {
            EditMovementPage editMovementPage = new EditMovementPage();
            await Navigation.PushAsync(editMovementPage);
        }

        /// <summary>
        /// Opens the window to edit movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OpenEditMovement(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;

            int movementId = (int)clickedButton.CommandParameter;

            Movement movement = App.Database.GetMovementAsync(movementId).Result;

            EditMovementPage editMovementPage = new EditMovementPage(movement.Description, movement.Value.ToString(), movement.Date, movement.Type, movement.WalletId, movementId);
            await Navigation.PushAsync(editMovementPage);
        }

    }
}