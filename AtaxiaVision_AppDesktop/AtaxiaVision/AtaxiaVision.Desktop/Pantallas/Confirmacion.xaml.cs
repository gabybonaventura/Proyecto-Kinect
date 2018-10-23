using AtaxiaVision.Desktop.Pantallas;
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
    /// Interaction logic for Confirmacion.xaml
    /// </summary>
    public partial class Confirmacion : Window
    {
        public Confirmacion()
        {
            InitializeComponent();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            Close();
        }
    }
}
