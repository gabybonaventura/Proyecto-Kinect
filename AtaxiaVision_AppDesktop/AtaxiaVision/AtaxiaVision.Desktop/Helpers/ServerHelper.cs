using Newtonsoft.Json;
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
        private const string archivoDatosOffile = @"\Archivos\ArchivoSinc.txt";
        private const string url = "https://ataxia-services-project.herokuapp.com/token/";
        public const int SIN_CONEXION = -1;
        public const int TOKEN_VALIDO = 1;
        public const int TOKEN_INVALIDO = 0;

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
            // return Enviar(SerializarArchivoOffline());
        }

        public static int ValidarToken(string token)
        {
            // PREGUNTAR. Guardar en archivo Configuracion.txt 
            string urlToken = url + token;
            var request = (HttpWebRequest)WebRequest.Create(urlToken);
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
                        //EstadoSnackBar("Token válido");
                        //Model.TokenValido = true;
                        //IniRehabBtn.IsEnabled = true;
                        return TOKEN_VALIDO;
                    }
                    //else
                    //{
                    //    //EstadoSnackBar("Token inválido");
                    //    return TOKEN_INVALIDO;
                    //}
                }
            }
            catch
            {
                //EstadoSnackBar("No hay conexión");
                //Model.TokenValido = false;
                //IniRehabBtn.IsEnabled = true;
                return SIN_CONEXION;
            }
            return TOKEN_INVALIDO;
        }
    }
}
