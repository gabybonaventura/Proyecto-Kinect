using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AtaxiaVision.Pantallas
{
    /// <summary>
    /// Interaction logic for Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        public Principal(bool flagToken, string token, int nroEj)
        {
            InitializeComponent();
        }

        private void FinEjercicioBtn_Click(object sender, RoutedEventArgs e)
        {
            Confirmacion win = new Confirmacion();
            win.Show();
            Close();
        }
    }
}
