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
    public class ServerHelper
    {
        private const string archivoDatosOffile = @"C:\Users\Public\Documents\AV_ArchivoDatosOffline.txt";
        private const string url = "https://ataxia-services-project.herokuapp.com/token/";
        public const int TOKEN_SINCONEXION = -1;
        public const int TOKEN_VALIDO = 1;
        public const int TOKEN_INVALIDO = 0;
        public const int ARCHIVOOFFLINE_NOEXISTE = -1;
        public const int ARCHIVOOFFLINE_SINCRONIZADO = 1;
        public const int ARCHIVOOFFLINE_NOSINCRONIZADO = 0;
        
        #region Metodos privados
        private static bool ExisteArchivoDatosOffile()
        {
            return File.Exists(archivoDatosOffile);
        }

        private static void EliminarArchivoDatosOffile()
        {
            File.Delete(archivoDatosOffile);
        }

        private static string LeerArchivoDatosOffile()
        {
            if (ExisteArchivoDatosOffile())
                return File.ReadAllText(archivoDatosOffile);
            else
                return "";
        }

        private static void EscribirArchivoDatosOffile(List<EjercicioViewModel> ejercicios)
        {
            var datos = JsonConvert.SerializeObject(ejercicios);
            File.AppendAllText(archivoDatosOffile, datos);
        }

        private static List<EjercicioViewModel> DeserializarArchivoOffline()
        {
            string json = LeerArchivoDatosOffile();
            var ejercicios = JsonConvert.DeserializeObject<List<EjercicioViewModel>>(json);
            if (ejercicios == null)
                ejercicios = new List<EjercicioViewModel>();
            return ejercicios;
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
                    var result = streamReader.ReadToEnd();
                    //revisar que dice result. Si envia ok o no ok.
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
        #endregion
        
        public static void AgregarEjercicioDatosOffile(EjercicioViewModel ejercicio)
        {
            var lista = DeserializarArchivoOffline();
            lista.Add(ejercicio);
            EscribirArchivoDatosOffile(lista);
        }
        
        public static int SincronizarDatosOffline()
        {
            if (!ExisteArchivoDatosOffile())
                return ARCHIVOOFFLINE_NOEXISTE;
            var data = LeerArchivoDatosOffile();
            var Envio = Enviar(data);
            if (Envio)
                return ARCHIVOOFFLINE_SINCRONIZADO;
            else
                return ARCHIVOOFFLINE_NOSINCRONIZADO;
        }

        public static int ValidarToken(string token)
        {
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
                return TOKEN_SINCONEXION;
            }
            return TOKEN_INVALIDO;
        }

        #region Test
        // Test para probar como guardar el archivo
        public static void TestInicializarArchivo()
        {
            EliminarArchivoDatosOffile();
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "11",
                Ejercicio = 1,
                Resultado = true,
                Desvios = 4,
                Promedio = Convert.ToDecimal(14.33)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "11",
                Ejercicio = 2,
                Resultado = false,
                Desvios = 21,
                Promedio = Convert.ToDecimal(12)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "12",
                Ejercicio = 1,
                Resultado = true,
                Desvios = 2,
                Promedio = Convert.ToDecimal(1)
            });
        }

        public static void TestLeerArchivo()
        {
            var json = LeerArchivoDatosOffile();
            var deserializado = DeserializarArchivoOffline();
        } 
        #endregion
    }
}
