using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Helpers
{
    public static class ImageHelper
    {
        public static void  function()
        {
            //HSV 60 60.0 39.2
            //60 68.6 100.0
            Bitmap imagen = new Bitmap("gatito.png");
            


            Image<Rgb, Byte> img = new Image<Rgb, byte>(imagen);
            Image<Hsv, Byte> hsvimg = img.Convert<Hsv, Byte>();
            Image<Gray, Byte>[] channels = hsvimg.Split();
            Image<Gray, Byte> imghue = channels[0];
            Image<Gray, Byte> imgsat = channels[1];
            Image<Gray, Byte> imgval = channels[2];

            Image<Gray, byte> huefilter = imghue.InRange(new Gray(60), new Gray(60));
            Image<Gray, byte> satfilter = imgsat.InRange(new Gray(60.0), new Gray(68.6));
            Image<Gray, byte> valfilter = imgval.InRange(new Gray(39.2), new Gray(100.0));



        }
    }
}
