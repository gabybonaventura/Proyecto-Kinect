using AtaxiaVision.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Helpers
{
    public class ServerHelper
    {
        private const string ARCHIVO_OFFILE = @"C:\Users\Public\Documents\AV_ArchivoDatosOffline.txt";
        private const string URL = "http://api.ataxiavision.com/";
        private const string API_SESSION = "session/";
        private const string API_TOKEN = "token/";
        private const string API_EJERCICIOS = "exercise/";
        private const string METHOD_POST = "POST";
        private const string METHOD_GET = "GET";
        public const int TOKEN_SINCONEXION = -1;
        public const int TOKEN_VALIDO = 1;
        public const int TOKEN_INVALIDO = 0;
        public const int ARCHIVOOFFLINE_NOEXISTE = -1;
        public const int ARCHIVOOFFLINE_SINCRONIZADO = 1;
        public const int ARCHIVOOFFLINE_NOSINCRONIZADO = 0;
        public const int SERVER_OK = 200;
        public const int SERVER_ERROR = 500;
        
        #region Metodos privados
        private static bool ExisteArchivoDatosOffile()
        {
            return File.Exists(ARCHIVO_OFFILE);
        }

        private static void EliminarArchivoDatosOffile()
        {
            File.Delete(ARCHIVO_OFFILE);
        }

        private static string LeerArchivoDatosOffile()
        {
            if (ExisteArchivoDatosOffile())
                return File.ReadAllText(ARCHIVO_OFFILE);
            else
                return "";
        }

        private static void EscribirArchivoDatosOffile(List<RepeticionViewModel> ejercicios)
        {
            EliminarArchivoDatosOffile();
            var datos = JsonConvert.SerializeObject(ejercicios);
            File.AppendAllText(ARCHIVO_OFFILE, datos);
        }

        private static List<RepeticionViewModel> DeserializarArchivoOffline()
        {
            string json = LeerArchivoDatosOffile();
            var ejercicios = JsonConvert.DeserializeObject<List<RepeticionViewModel>>(json);
            if (ejercicios == null)
                ejercicios = new List<RepeticionViewModel>();
            return ejercicios;
        }

        private static dynamic EnviarGet(string api, string data)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(URL + api + data);
                var content = string.Empty;
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
                if (Convert.ToInt32(status_code) == SERVER_OK)
                    return rta.data;
            }
            catch (Exception)
            {
            }
            return SERVER_ERROR;
        }

        private static dynamic EnviarPost(string api, string data)
        {
            try
            {

                var request = (HttpWebRequest)WebRequest.Create(URL + api);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var rtaSr = streamReader.ReadToEnd();
                    var rta = JsonConvert.DeserializeObject<dynamic>(rtaSr);
                    string status_code = rta.head.status_code;
                    if (Convert.ToInt32(status_code) == SERVER_OK)
                        return rta.data;
                }
            }
            catch (Exception)
            {
            }
            return SERVER_ERROR;
        }

        private static dynamic Enviar(string api, string method, string data)
        {
            switch (method)
            {
                case METHOD_GET:
                    return EnviarGet(api, data);
                case METHOD_POST:
                    return EnviarPost(api, data);
                default:
                    break;
            }
            return null;
        }

        private static bool RequestNoValida(dynamic result)
        {
            return result.GetType().Name == "Int32" && result == SERVER_ERROR;
        }
        #endregion
        
        public static void AgregarEjercicioDatosOffile(RepeticionViewModel ejercicio)
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
            var result = Enviar(API_SESSION,METHOD_POST,data);

            if (RequestNoValida(result))
                return ARCHIVOOFFLINE_NOSINCRONIZADO;

            EliminarArchivoDatosOffile();
            return ARCHIVOOFFLINE_SINCRONIZADO;
        }

        public static RespuestaToken ValidarToken(string token)
        {
            var respuestaToken = new RespuestaToken();
            var result = Enviar(API_TOKEN,METHOD_GET, token);
            if (RequestNoValida(result))
                return respuestaToken;
            if (Convert.ToBoolean(result.isValid))
            {
                respuestaToken.Patient = new Patient(result.patient);
                respuestaToken.CodigoTokenValid = TOKEN_VALIDO;
            }
            else
                respuestaToken.CodigoTokenValid = TOKEN_INVALIDO;
            return respuestaToken;
        }

        public static int EnviarEjercicio(RepeticionViewModel ejercicio)
        {
            // El metodo POST necesita una lista.
            ejercicio.Fecha = DateTime.Now;
            List<RepeticionViewModel> ejercicios = new List<RepeticionViewModel>
            {
                ejercicio
            };
            var datos = JsonConvert.SerializeObject(ejercicios);
            var result = Enviar(API_SESSION, METHOD_POST, datos);
            if (RequestNoValida(result))
            {
                AgregarEjercicioDatosOffile(ejercicio);
                return SERVER_ERROR;
            }
            return SERVER_OK;
        }

        public static List<EjercicioViewModel> ObtenerEjercicios()
        {
            List<EjercicioViewModel> result = new List<EjercicioViewModel>();
            var resultGet = Enviar(API_EJERCICIOS, METHOD_GET, null);

            return result;
        }

        #region Test
        // Test para probar como guardar el archivo
        public static void TestInicializarArchivo()
        {
            EliminarArchivoDatosOffile();
            ServerHelper.AgregarEjercicioDatosOffile(new RepeticionViewModel
            {
                Token = "38662776_1",
                Ejercicio = 1,
                FinalizoConExito = true,
                Desvios = 4,
                Fecha = DateTime.Now,
                Duracion = new TimeSpan(0, 0, 32)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new RepeticionViewModel
            {
                Token = "38662776_1",
                Ejercicio = 2,
                FinalizoConExito = false,
                Desvios = 21,
                Fecha = DateTime.Now,
                Duracion = new TimeSpan(0, 1, 4)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new RepeticionViewModel
            {
                Token = "38662776_2",
                Ejercicio = 1,
                FinalizoConExito = true,
                Desvios = 2,
                Fecha = DateTime.Now,
                Duracion = new TimeSpan(0, 0, 24)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new RepeticionViewModel
            {
                Token = "38662776_2",
                Ejercicio = 2,
                FinalizoConExito = true,
                Desvios = 21,
                Fecha = DateTime.Now,
                Duracion = new TimeSpan(0, 0, 21)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new RepeticionViewModel
            {
                Token = "38662776_2",
                Ejercicio = 3,
                FinalizoConExito = false,
                Desvios = 7,
                Fecha = DateTime.Now,
                Duracion = new TimeSpan(0, 1, 32)
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
