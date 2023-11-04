using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Budget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        

        public AboutPage ()
        {
            InitializeComponent();

            //Add pic
            Image_Photo.Source = ImageSource.FromResource("Budget.Resources.photo_profile500.png");

            // Add event to mail label to open mail app
            string recipientEmail = "carloesposito.c@gmail.com";
            string subject = "[Budget]";
            string mailtoUri = $"mailto:{recipientEmail}?subject={subject}";
            
            Mail.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Device.OpenUri(new Uri(mailtoUri));
                })
            });

            // Add event to github label
            GitHub.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Device.OpenUri(new Uri("https://github.com/carloesposiito?tab=repositories"));
                })
            });
        }

        /// <summary>
        /// Occurs when page disappears
        /// </summary>
        protected override void OnDisappearing()
        {
            GoToPreviousPage();
        }

        /// <summary>
        /// Create page
        /// </summary>
        void CreatePage()
        {

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