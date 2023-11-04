using Budget.Data;
using System;
using System.IO;
using Xamarin.Forms;

namespace Budget
{
    public partial class App : Application
    {
        static DataAccessObject database;

        /// <summary>
        /// Create connection to database as singleton
        /// </summary>
        internal static DataAccessObject Database
        {
            get
            {
                if (database == null)
                {
                    database = new DataAccessObject(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Budget.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
