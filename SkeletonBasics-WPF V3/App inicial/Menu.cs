using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Microsoft.Samples.Kinect.SkeletonBasics.App_inicial
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://ataxia-services-project.herokuapp.com/token";
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            //request.UserAgent = RequestConstants.UserAgentValue;
            //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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
            Console.WriteLine(rta.body.token);
            //eturn content;
        
        }
    }
}

