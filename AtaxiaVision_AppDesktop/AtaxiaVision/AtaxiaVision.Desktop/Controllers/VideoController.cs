using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Controllers
{
    class VideoController
    {
        private long _inicioGrabacion;

        public long InicioGrabacion
        {
            get { return _inicioGrabacion; }
            set
            {
                if (_inicioGrabacion == 0)
                    _inicioGrabacion = value;
            }
        }
        public long FinGrabacion { get; set; }

        public VideoController()
        {
            _inicioGrabacion = 0;
        }
        public Bitmap ReduceBitmap(Bitmap original, int reducedWidth, int reducedHeight)
        {
            var reduced = new Bitmap(reducedWidth, reducedHeight);
            using (var dc = Graphics.FromImage(reduced))
            {
                // you might want to change properties like
                dc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                dc.DrawImage(original, new Rectangle(0, 0, reducedWidth, reducedHeight), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            }

            return reduced;
        }

        public void GuardarVideo(List<Bitmap> frames, string nombreArchivo)
        {
            int width = 640;
            int height = 480;
            FinGrabacion = DateTime.Now.Ticks;

            int Duracion = (int)(new TimeSpan(FinGrabacion - InicioGrabacion)).TotalSeconds;


            int framRate = frames.Count / Duracion;

            // create instance of video writer
            using (var vFWriter = new VideoFileWriter())
            {
                // create new video file
                vFWriter.Open($"C://Users//Public/Videos//{nombreArchivo}.avi", width, height, framRate, VideoCodec.Raw);


                //loop throught all images in the collection
                foreach (var frameBmp in frames)
                {
                    //what's the current image data?

                    var bmpReduced = ReduceBitmap(frameBmp, width, height);

                    vFWriter.WriteVideoFrame(bmpReduced);
                }
                vFWriter.Close();
            }


        }
    }
}
