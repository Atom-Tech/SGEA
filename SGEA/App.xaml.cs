using System.Windows;
using System.Windows.Threading;

namespace SGEA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Error.Erro(e.Exception);
            e.Handled = true;
        }
    }
}
