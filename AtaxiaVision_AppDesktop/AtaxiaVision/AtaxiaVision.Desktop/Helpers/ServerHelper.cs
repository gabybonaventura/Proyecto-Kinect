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
        private const string URL = "https://ataxia-services-project.herokuapp.com/";
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

        private static RespuestaGET EnviarGet(string api, string data)
        {
            RespuestaGET result = new RespuestaGET();
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
            result.RespuestaCode = Convert.ToInt16(status_code);
            if (result.RespuestaCode == SERVER_OK)
            {
                string isValid = rta.data.isValid;
                result.PropiedadIsValid = Convert.ToBoolean(isValid);
            }
            return result;
        }

        private static int EnviarPost(string api, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL + api);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            var dataByte = Encoding.ASCII.GetBytes(data);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(dataByte, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return Convert.ToInt32(responseString);
        }

        private static int Enviar(string api, string method, string data)
        {
            int resultado = SERVER_ERROR;
            switch (method)
            {
                case METHOD_GET:
                    EnviarGet(api, data);
                    break;
                case METHOD_POST:
                    EnviarPost(api, data);
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
            if (Envio == SERVER_OK)
                return ARCHIVOOFFLINE_SINCRONIZADO;
            else
                return ARCHIVOOFFLINE_NOSINCRONIZADO;
        }

        public static int ValidarToken(string token)
        {
            var resultGet = EnviarGet(API_TOKEN, token);
            if (resultGet.RespuestaCode == SERVER_OK)
            {
                if (resultGet.PropiedadIsValid)
                    return TOKEN_VALIDO;
                else
                    return TOKEN_INVALIDO;
            }
            return TOKEN_SINCONEXION;
        }

        #region Test
        // Test para probar como guardar el archivo
        public static void TestInicializarArchivo()
        {
            EliminarArchivoDatosOffile();
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "22",
                Ejercicio = 1,
                Resultado = true,
                Desvios = 4,
                Promedio = Convert.ToDecimal(14.33)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "22",
                Ejercicio = 2,
                Resultado = false,
                Desvios = 21,
                Promedio = Convert.ToDecimal(12)
            });
            ServerHelper.AgregarEjercicioDatosOffile(new EjercicioViewModel
            {
                Token = "233",
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
