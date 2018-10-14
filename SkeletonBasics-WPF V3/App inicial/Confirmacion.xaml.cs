using Microsoft.Samples.Kinect.SkeletonBasics.Helpers;
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
    /// Lógica de interacción para Confirmacion.xaml
    /// </summary>
    public partial class Confirmacion : Window
    {
        bool flagToken, result;
        List<Angulos> listDesvio;
        int desvios = 0, contProm=0;
        float prom = 0;
        string tokenID, json;
        string[] lines;
        List<DatosSesion> listaDatos;

        public Confirmacion(bool flagToken, List<Angulos> desvios, bool resultado, string token)
        {
            InitializeComponent();
            this.listDesvio = desvios;
            this.flagToken = flagToken;
            this.result = resultado;
            this.tokenID = token;
            lines = null;
            listaDatos = new List<DatosSesion>();
            CalcularDesviosYResultado();
            EnviarDatos();  
        }

        private void GenerarJSON()
        {
            
            //reviso que el archivo no exista.
<<<<<<< HEAD
<<<<<<< HEAD
            if (File.Exists("..\\Archivo\\ArchivoSinc.txt"))
=======
            /*if (File.Exists(@"\Archivo\ArchivoSinc.txt"))
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
=======
            /*if (File.Exists(@"\Archivo\ArchivoSinc.txt"))
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
            {
                //si existe, leo todos los datos para sincronizar:
                lines = System.IO.File.ReadAllLines(@"\Archivo\ArchivoSinc.txt");

                //genero el json para enviar todos los datos:
                foreach (string line in lines)
                {
                    DatosSesion d = (DatosSesion)JsonConvert.DeserializeObject(line);
                    listaDatos.Add(d);
                }
<<<<<<< HEAD
<<<<<<< HEAD
            }
=======
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
            }*/
            jsonAux = jsonAux + json + "]}";
            return jsonAux;
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
        }

        private void EnviarDatos()
        {
            int nroEj = 1;
            DatosSesion datos = new DatosSesion("546", 1, true, 5, 2);
            //DatosSesion datos = new DatosSesion(tokenID, nroEj, result, desvios, prom);

            GenerarJSON();
            string jsonAux = JsonConvert.SerializeObject(datos);
            listaDatos.Add(datos);

            /*List<DatosSesion> list = new List<DatosSesion>();

            list.Add(datos);
            list.Add(new DatosSesion("789", 1, false, 5, 2));*/


            json = JsonConvert.SerializeObject(listaDatos);

            //la url es otra.
<<<<<<< HEAD
<<<<<<< HEAD
            string url = "https://ataxia-services-project.herokuapp.com/session";
=======
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
            //es un path de mock server
            //string url = "https://5f504b6e-15a3-4946-87de-56cdfdccf0ca.mock.pstmn.io/pathgabi";

            string url = "https://ataxia-services-project.herokuapp.com/sessions";

<<<<<<< HEAD
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    

                    //si todo sale exitosamente Y existe el archivo, debo eliminar el archivo: 
<<<<<<< HEAD
<<<<<<< HEAD

                    if (File.Exists("..\\Archivo\\ArchivoSinc.txt"))
                        File.Delete("..\\Archivo\\ArchivoSinc.txt");
                    
                    
=======
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
                   /* if (1 == 1)
                    {
                  /*      if (File.Exists(@"\ArchivoSinc.txt"))
                            File.Delete(@"\ArchivoSinc.txt");
                    }
                    else
                    {
                        File.AppendAllText(@"\Archivo\ArchivoSinc.txt", json);
                        MessageBox.Show("No se ha podido sincronizar los datos.", "Error sincronización");
                    }*/
<<<<<<< HEAD
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
                }
            }
            catch (Exception d)
            {
                //si no se puede enviar, agrego a archivo. si el archivo no existe, lo creo.
<<<<<<< HEAD
<<<<<<< HEAD
                File.AppendAllText("..\\Archivo\\ArchivoSinc.txt", jsonAux);
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
=======
>>>>>>> cd0ab7993a3be4cfde6fd5850e0f6aaefe730300
                //File.AppendAllText(@"\ArchivoSinc.txt", json);
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
