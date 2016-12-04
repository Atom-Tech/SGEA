using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Apresentacao.Slide
{
    /// <summary>
    /// Interaction logic for Slide11.xaml
    /// </summary>
    public partial class Slide11 : Page
    {
        string add;

        public Slide11()
        {
            InitializeComponent();
            add = Directory.GetCurrentDirectory().Replace("\\bin\\x86\\Debug","") +
                "\\Site\\index.php";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            chrome.Address = add;
        }
        
    }
}
