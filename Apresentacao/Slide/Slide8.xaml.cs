using SGEA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Apresentacao.Slide
{
    /// <summary>
    /// Interaction logic for Slide8.xaml
    /// </summary>
    public partial class Slide8 : System.Windows.Controls.UserControl
    {
        public Slide8()
        {
            InitializeComponent();
        }

        private void botaoSGEA_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Program Files (x86)\AtomTech\SGEA\SGEA.exe");
        }

        private void botaoMobile_Click(object sender, RoutedEventArgs e)
        {
            SlideFrame.SlideMobile(14);
        }

        private void botaoAtom_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start((Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
            //"\\AtomTech\\SGEA\\Site\\index.html"));
            SlideFrame.SlideMobile(11);
        }
    }
}
