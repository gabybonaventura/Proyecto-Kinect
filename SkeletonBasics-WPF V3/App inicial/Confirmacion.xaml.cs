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
    /// Lógica de interacción para Confirmacion.xaml
    /// </summary>
    public partial class Confirmacion : Window
    {
        bool flagToken, result;
        List<Angulos> listDesvio;
        int desvios = 0, contProm=0;
        float prom = 0;
        string tokenID;
        string[] lines;

        public Confirmacion(bool flagToken, List<Angulos> desvios, bool resultado, string token)
        {
            this.listDesvio = desvios;
            this.flagToken = flagToken;
            this.result = resultado;
            this.tokenID = token;
            lines = null;
            CalcularDesviosYResultado();
            EnviarDatos();
            InitializeComponent();
            
        }

        private string GenerarJSON(string json)
        {
            string jsonAux = "{\"ejercicios\":[";
            //reviso que el archivo no exista.
            if (File.Exists(@"\Archivo\ArchivoSinc.txt"))
            {
                //si existe, leo todos los datos para sincronizar:
                lines = System.IO.File.ReadAllLines(@"\Archivo\ArchivoSinc.txt");

                //genero el json para enviar todos los datos:
                foreach (string line in lines)
                {
                    jsonAux += line + ",";
                    // Use a tab to indent each line of the file.
                    Console.WriteLine("\t" + line);
                }
            }
            jsonAux = jsonAux + json + "]}";
            return jsonAux;
        }

        private void EnviarDatos()
        {
            //genero el json correspondiente a este ejercicio:
            string json = "{\"tokenID\":\"" + tokenID + "\"," +
                                  /* "\"nroEj\":\""+ nroEj + "\"}" +*/
                                  "\"resultado\":\"" + result + "\"" +
                                  "\"desvios\":\"" + desvios + "\"" +
                                  "\"promedio\":\"" + prom + "\"}";

            string jsonAux = GenerarJSON(json);

            //la url es otra.
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
                    //si todo sale exitosamente Y existe el archivo, debo eliminar el archivo: 
                    if (1 == 1)
                    {
                        if (File.Exists(@"\Archivo\ArchivoSinc.txt"))
                            File.Delete(@"\Archivo\ArchivoSinc.txt");
                    }
                    else
                    {
                        File.AppendAllText(@"\Archivo\ArchivoSinc.txt", json);
                        MessageBox.Show("No se ha podido sincronizar los datos.", "Error sincronización");
                    }
                }
            }
            catch (Exception d)
            {
                //si no se puede enviar, agrego a archivo. si el archivo no existe, lo creo.
                File.AppendAllText(@"\Archivo\ArchivoSinc.txt", json);
                MessageBox.Show("No se ha podido sincronizar los datos.", "Error sincronización");
            }

            

        }

        private void CalcularDesviosYResultado()
        {
            //acá contamos la cantidad de desvíos y deberíamos hacer un prom del valor
            //del desvío. Para eso deberíamos traducir la tensión a otro valor
            //ESO QUEDA PENDIENTE.[FALTA]
            foreach (Angulos ang in listDesvio)
            {
                //sumo para el promedio de desvios, en este caso debería contar además cantidad de desvios en particular.
                if (ang.CodoID > 200)
                {
                    prom += (float)ang.CodoID;
                    contProm++;
                }

                if (ang.CodoAA > 200)
                {
                    prom += (float)ang.CodoAA;
                    contProm++;
                }

                if (ang.HombroAA > 200)
                {
                    prom += (float)ang.HombroAA;
                    contProm++;
                }

                if (ang.HombroID > 200)
                {
                    prom += (float)ang.HombroID;
                    contProm++;
                }

                if (ang.CodoID > 200 || ang.CodoAA > 200 ||
                    ang.HombroAA > 200 || ang.HombroID > 200)
                    desvios++;
            }
            prom = prom / contProm;
        }

        private void SiBtn_Click(object sender, RoutedEventArgs e)
        {
            //en este botón abro de nuevo MainWindow para un nuevo ejercicio
            MainWindow win = new MainWindow(flagToken,tokenID);
            win.Show();
            this.Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            //en este vuelvo al menú principal:
            Window1 win = new Window1();
            win.Show();
            this.Close();
        }
    }
}
