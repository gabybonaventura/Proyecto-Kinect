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

        private static void EscribirArchivoDatosOffile(List<EjercicioViewModel> ejercicios)
        {
            EliminarArchivoDatosOffile();
            var datos = JsonConvert.SerializeObject(ejercicios);
            File.AppendAllText(ARCHIVO_OFFILE, datos);
        }

        private static List<EjercicioViewModel> DeserializarArchivoOffline()
        {
            string json = LeerArchivoDatosOffile();
            var ejercicios = JsonConvert.DeserializeObject<List<EjercicioViewModel>>(json);
            if (ejercicios == null)
                ejercicios = new List<EjercicioViewModel>();
            return ejercicios;
        }

        private static RespuestaServer EnviarGet(string api, string data)
        {
            RespuestaServer result = new RespuestaServer();
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
            result.RespuestaCode = Convert.ToInt32(status_code);
            if (result.RespuestaCode == SERVER_OK)
            {
                string isValid = rta.data.isValid;
                result.PropiedadIsValid = Convert.ToBoolean(isValid);
            }
            return result;
        }

        private static RespuestaServer EnviarPost(string api, string data)
        {
            RespuestaServer result = new RespuestaServer();
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
                string status_code = rta.head.status;
                result.RespuestaCode = Convert.ToInt32(status_code);
            }
            return result;
        }

        private static RespuestaServer Enviar(string api, string method, string data)
        {
            RespuestaServer resultado = new RespuestaServer();
            switch (method)
            {
                case METHOD_GET:
                    resultado = EnviarGet(api, data);
                    break;
                case METHOD_POST:
                    resultado = EnviarPost(api, data);
                    break;
                default:
                    break;
            }
            return resultado;
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
            var Envio = Enviar(API_SESSION,METHOD_POST,data);
            if (Envio.RespuestaCode == SERVER_OK)
            {
                EliminarArchivoDatosOffile();
                return ARCHIVOOFFLINE_SINCRONIZADO;
            }

            return ARCHIVOOFFLINE_NOSINCRONIZADO;
        }

        public static int ValidarToken(string token)
        {
            var resultGet = Enviar(API_TOKEN,METHOD_GET, token);
            if (resultGet.RespuestaCode == SERVER_OK)
            {
                if (resultGet.PropiedadIsValid)
                    return TOKEN_VALIDO;
                else
                    return TOKEN_INVALIDO;
            }
            return TOKEN_SINCONEXION;
        }

        public static int EnviarEjercicio(EjercicioViewModel ejercicio)
        {
            // El metodo POST necesita una lista.
            ejercicio.Fecha = DateTime.Now;
            List<EjercicioViewModel> ejercicios = new List<EjercicioViewModel>
            {
                ejercicio
            };
            var datos = JsonConvert.SerializeObject(ejercicios);
            var resultPost = Enviar(API_SESSION, METHOD_POST, datos);
            if (resultPost.RespuestaCode == SERVER_ERROR)
            {
                AgregarEjercicioDatosOffile(ejercicio);
                return SERVER_ERROR;
            }
            return SERVER_OK;
        }

        #region Test
        // Test para probar como guardar el archivo
        public static void TestInicializarArchivo()
        {
            EliminarArchivoDatosOffile();
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "38662776_1",
                Ejercicio = 1,
                FinalizoConExito = true,
                Desvios = 4,
                Fecha = DateTime.Now
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "38662776_1",
                Ejercicio = 2,
                FinalizoConExito = false,
                Desvios = 21,
                Fecha = DateTime.Now
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "38662776_2",
                Ejercicio = 1,
                FinalizoConExito = true,
                Desvios = 2,
                Fecha = DateTime.Now
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "38662776_2",
                Ejercicio = 2,
                FinalizoConExito = true,
                Desvios = 21,
                Fecha = DateTime.Now
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "38662776_2",
                Ejercicio = 3,
                FinalizoConExito = false,
                Desvios = 7,
                Fecha = DateTime.Now
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
