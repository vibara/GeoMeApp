using GeoMeApp.Services;
using System.Diagnostics;

namespace GeoMeApp;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; }


    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Services = provider;
        InitializeDatabase();
        MainPage = new AppShell();
    }

    private void InitializeDatabase()
    {
        var databaseService = Services.GetService<IDatabaseService>();
        Debug.Assert(databaseService != null);
        if (databaseService != null)
        {
            using var dataContext = databaseService?.GetDataContext(true);
            Debug.Assert(dataContext != null);
        }
    }
}
