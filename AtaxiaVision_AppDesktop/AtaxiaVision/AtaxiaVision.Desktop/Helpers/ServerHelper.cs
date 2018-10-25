using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Helpers
{
    class ServerHelper
    {
        private static string archivoDatosOffile = @"\Archivos\ArchivoSinc.txt";
        private static string url = "https://ataxia-services-project.herokuapp.com/token/";

        public static bool ExisteDatosOffline()
        {
            return File.Exists(archivoDatosOffile);
        }

        private static string SerializarArchivoOffline()
        {
            // PREGUNTAR. Se puede hacer con la serializacion?
            string jsonAux = "{\"ejercicios\":[";
            //si existe, leo todos los datos para sincronizar:
            string[] lines = File.ReadAllLines(archivoDatosOffile);

            //genero el json para enviar todos los datos:
            foreach (string line in lines)
            {
                jsonAux += line + ",";
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            jsonAux = jsonAux + "]}";
            return jsonAux;
        }

        private static bool Enviar(string data)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
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
                        File.Delete(archivoDatosOffile);
                    }
                    else
                    {
                        //MessageBox.Show("No se ha podido sincronizar los datos.", "Error sincronización");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SincronizarDatosOffline()
        {
            var data = SerializarArchivoOffline();
            var Envio = Enviar(data);
            return Envio;
        }
    }
}
