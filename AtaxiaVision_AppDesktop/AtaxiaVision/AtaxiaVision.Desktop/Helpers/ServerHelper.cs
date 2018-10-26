using AtaxiaVision.Models;
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

        public static bool ExisteArchivoDatosOffile()
        {
            return File.Exists(archivoDatosOffile);
        }

        public static string LeerArchivoDatosOffile()
        {
            return File.ReadAllText(archivoDatosOffile);
        }

        public static bool EscribirArchivoDatosOffile(List<EjercicioViewModel> ejercicios)
        {
            try
            {
                var datos = JsonConvert.SerializeObject(ejercicios);
                File.AppendAllText(archivoDatosOffile, datos);
                return true;
            }
            catch (Exception)
            {
                
            }
            return false;
        }

        public static List<EjercicioViewModel> DeserializarArchivoOffline()
        {
            string json = LeerArchivoDatosOffile();
            var ejercicios = JsonConvert.DeserializeObject<List<EjercicioViewModel>>(json);
            return ejercicios;
        }

        public static bool Enviar(string data)
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
                    // PREGUNTAR. Para que se asigna la variable si nunca se usa? -- no sabemos que devuelve result
                    var result = streamReader.ReadToEnd();
                    //revisar que dice result. Si envia ok o no ok.
                    // PREGUNTAR. Por que esta condicion? -- porque no puede comparar a result
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
            var data = LeerArchivoDatosOffile();
            var Envio = Enviar(data);
            return Envio;
        }

        public static int ValidarToken(string token)
        {
            // PREGUNTAR. Guardar en archivo Configuracion.txt
            // -- Hacerlo
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
