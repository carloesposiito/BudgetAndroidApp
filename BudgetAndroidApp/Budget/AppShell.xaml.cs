using Budget.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Budget
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(WalletsPage), typeof(WalletsPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
        }
    }
}