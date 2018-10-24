using AtaxiaVision.Models;
using AtaxiaVision.Pantallas;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace AtaxiaVision.Desktop.Pantallas
{
    /// <summary>
    /// Interaction logic for Inicio.xaml
    /// </summary>
    public partial class Inicio : Window
    {
        // PREGUNTAR. El flag de tokenValido no se usa nunca, si todo ya se hace al tocar click.
        private InicioViewModel Model = new InicioViewModel();

        public Inicio()
        {
            InitializeComponent();
            SincronizarDatos();
        }

        public Inicio(string token, int nroEjercicio)
        {
            Model.Token = token;
            Model.Ejercicio = nroEjercicio;
        }

        private void EstadoSnackBar(string mensaje)
        {
            EstadoSnackbar.IsActive = false;
            EstadoSnackbar.Message = new SnackbarMessage() { Content = mensaje };
            EstadoSnackbar.IsActive = true;
        }

        private void SincronizarDatos()
        {
            EstadoSnackBar("Sincronizando Datos...");
            //reviso que el archivo exista.
            // PREGUNTAR. Se puede guardar la ruta en un archivo de Configuracion?
            if (File.Exists(@"\Archivos\ArchivoSinc.txt"))
            {
                // PREGUNTAR. Se puede hacer con la serializacion?
                string jsonAux = "{\"ejercicios\":[";
                //si existe, leo todos los datos para sincronizar:
                string[] lines = System.IO.File.ReadAllLines(@"\Archivos\ArchivoSinc.txt");

                //genero el json para enviar todos los datos:
                foreach (string line in lines)
                {
                    jsonAux += line + ",";
                    // Use a tab to indent each line of the file.
                    Console.WriteLine("\t" + line);
                }
                jsonAux = jsonAux + "]}";

                //lo envío:
                string url = "https://ataxia-services-project.herokuapp.com/token/";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(jsonAux);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // PREGUNTAR. Para que se asigna la variable si nunca se usa?
                        var result = streamReader.ReadToEnd();
                        //revisar que dice result. Si envia ok o no ok.
                        // PREGUNTAR. Por que esta condicion?
                        if (1 == 1)
                        {
                            File.Delete(@"\Archivos\ArchivoSinc.txt");
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido sincronizar los datos.", "Error sincronización");
                        }
                    }
                    //si todo salió ok, elimino el archivo.
                    EstadoSnackBar("Datos sincronizados");
                }
                catch (Exception)
                {
                    EstadoSnackBar("Error al sincronizar");
                    MessageBox.Show("No se ha podido sincronizar los datos pendientes.", "Error sincronización");
                }
            }
            else
            {
                EstadoSnackBar("Sincronizacion finalizada");
            }   
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ValidarTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            Model.Token = TokenTextBox.Text;
            if (String.IsNullOrEmpty(Model.Token))
            {
                EstadoSnackBar("Por favor ingrese un token");
                return;
            }

            // PREGUNTAR. Guardar en archivo Configuracion.txt 
            string url = "https://ataxia-services-project.herokuapp.com/token/" + Model.Token;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            var content = string.Empty;

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
                var rta = JsonConvert.DeserializeObject<dynamic>(content);
                string status_code = rta.head.status_code;

                //Console.WriteLine(rta.head.status_code);
                if (status_code.Equals("200"))
                {
                    string isValid = rta.data.isValid;
                    if (isValid.Equals("True"))
                    {
                        EstadoSnackBar("Token válido");
                        Model.TokenValido = true;
                        IniRehabBtn.IsEnabled = true;
                    }
                    else
                    {
                        EstadoSnackBar("Token inválido");
                    }
                }
            }
            catch
            {
                EstadoSnackBar("No hay conexión");
                Model.TokenValido = false;
                IniRehabBtn.IsEnabled = true;
            }
        }

        private void IniRehabBtn_Click(object sender, RoutedEventArgs e)
        {
            // PREGUNTAR. Siempre manda numero de ejercicio 1.
            Principal win = new Principal(Model.TokenValido, Model.Token, 1);
            win.Show();
            Close();
        }
    }
}
