using Microsoft.Samples.Kinect.SkeletonBasics.App_inicial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    /// <summary>
    /// Lógica de interacción para VentInicial.xaml
    /// </summary>
    public partial class VentInicial : Window
    {
        public VentInicial()
        {
            WindowStyle = WindowStyle.None;
            InitializeComponent();

           /* TaskEx.Delay(5000);

            Window1 win = new Window1();
            win.Show();*/
            //this.Close();
        }
    }
}
