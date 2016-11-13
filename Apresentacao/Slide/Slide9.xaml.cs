using RegawMOD.Android;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Apresentacao.Slide
{
    /// <summary>
    /// Interaction logic for Slide9.xaml
    /// </summary>
    public partial class Slide9 : UserControl
    {
        private bool x = false;
        private Main slideMestre;

        public Slide9(Main m)
        {
            InitializeComponent();
            slideMestre = m;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!x)
            {
                slideMestre.Topmost = false;
                WindowHelper.bringToFront("Vysor");
                x = true;
            }
            else
            {
                slideMestre.Topmost = true;
                WindowHelper.bringToFront("Vysor");
                x = false;
            }
        }
    }
}
