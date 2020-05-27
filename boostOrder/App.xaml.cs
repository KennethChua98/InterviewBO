using boostOrder.View;
using System.Windows;

namespace boostOrder
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreen splash= new SplashScreen("Resources/loading.png");
            splash.Show(autoClose: true, topMost: true);
        }
    }

}





