using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Controllers
{
    public class VideoController
    {
        private long _inicioGrabacion;
        public List<System.Drawing.Bitmap> framesBmp { get; set; }

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
            framesBmp = new List<System.Drawing.Bitmap>();
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

        public void GuardarVideo(string nombreArchivo)
        {
            if(framesBmp.Count != 0)
            {
                int width = 640;
                int height = 480;
                //FinGrabacion = DateTime.Now.Ticks;

                int Duracion = (int)(new TimeSpan(FinGrabacion - InicioGrabacion)).TotalSeconds;


                int framRate = this.framesBmp.Count / Duracion;
                Console.WriteLine($"Frames por segundo {framRate}");

                // create instance of video writer
                using (var vFWriter = new VideoFileWriter())
                {
                    // create new video file
                    vFWriter.Open($"C://Users//Public/Videos//{nombreArchivo}.avi", width, height, framRate, VideoCodec.Raw);


                    //loop throught all images in the collection
                    foreach (var frame in framesBmp)
                    {
                        //what's the current image data?

                        var bmpReduced = ReduceBitmap(frame, width, height);

                        vFWriter.WriteVideoFrame(bmpReduced);
                    }
                    vFWriter.Close();
                }
                //this.framesBmp = new List<Bitmap>();
            }
        }
    }
}
