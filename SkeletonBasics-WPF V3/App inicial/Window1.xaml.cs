using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Samples.Kinect.SkeletonBasics.App_inicial
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string token;
        private bool flagTokenValidado;
        public Window1()
        {
            //debo revisar si existe archivo para sincronizar.
            SincronizarDatos();
            InitializeComponent();
        }

        private void SincronizarDatos()
        {
            
            //reviso que el archivo exista.
            if (File.Exists(@"\Archivo\ArchivoSinc.txt"))
            {
                string jsonAux = "{\"ejercicios\":[";
                //si existe, leo todos los datos para sincronizar:
                string[] lines = System.IO.File.ReadAllLines(@"\Archivo\ArchivoSinc.txt");

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
                        var result = streamReader.ReadToEnd();
                        //revisar que dice result. Si envia ok o no ok.
                        if (1 == 1)
                        {
                                File.Delete(@"\Archivo\ArchivoSinc.txt");
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido sincronizar los datos.", "Error sincronización");
                        }
                    }
                    //si todo salió ok, elimino el archivo.

                }catch(Exception d)
                {
                    MessageBox.Show("No se ha podido sincronizar los datos pendientes.", "Error sincronización");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
             if (String.IsNullOrEmpty(TokenTextBox.Text))
             {
                 this.ErrorLabel.Content = "Por favor ingrese un token";
                 return;
             }

             this.ErrorLabel.Content = "";
             token = this.TokenTextBox.Text;
             string url = "https://ataxia-services-project.herokuapp.com/token/"+token;
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
                        Console.WriteLine("token válido");
                        flagTokenValidado = true;
                        this.IniRehabBtn.IsEnabled = true;
                    }
                    else
                    {
                        Console.WriteLine("token inválido");
                    }
                    
                }
             }
             catch
             {
                Console.WriteLine("no hay conexión amea!");
                flagTokenValidado = false;
                this.IniRehabBtn.IsEnabled = true;
            }
        }

        private void IniRehabBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow(flagTokenValidado, token, 1);
            win.Show();
            this.Close();
        }
    }
}
