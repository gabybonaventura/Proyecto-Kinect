using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using camaraProfundidad.Helpers;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Kinect;

namespace camaraProfundidad
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor miKinect;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (KinectSensor.KinectSensors.Count == 0) {
                MessageBox.Show("No se detecta ningun kinect", "Visor de Camara");
                Application.Current.Shutdown();
            }

            try
            {
                miKinect = KinectSensor.KinectSensors.FirstOrDefault();
                miKinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                miKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                miKinect.Start();
                miKinect.DepthFrameReady += miKinect_DepthFrameReady;
                miKinect.ColorFrameReady += SensorColorFrameReady;
            }
            catch {
                MessageBox.Show("Fallo al inicializar kinect", "Visor de KInect");
                Application.Current.Shutdown();
            }
        }
        WriteableBitmap bitmapEficiente = null;

        short[] datosDistancia = null;
        byte[] colorImagenDistancia = null;
        WriteableBitmap bitmapImagenDistancia = null;
        double ObjetoX;
        double ObjetoY;

        void miKinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame framesDistancia = e.OpenDepthImageFrame()) {
                if (framesDistancia == null) return;

                if (datosDistancia == null)
                    datosDistancia = new short[framesDistancia.PixelDataLength];

                if (colorImagenDistancia == null)
                    colorImagenDistancia = new byte[framesDistancia.PixelDataLength * 4];

                framesDistancia.CopyPixelDataTo(datosDistancia);

                int posColorImagenDistancia = 0;
                int x = 0;
                int y = 0;
                for (int i = 0; i < framesDistancia.PixelDataLength; i++ )
                {
                    if (x < 639)
                        x++;
                    else
                    {
                        y++;
                        x = 0;
                    }
                    int valorDistancia = datosDistancia[i] >> 3;

                    if (x == ObjetoX && y == ObjetoY)
                        Console.WriteLine($"Z: {valorDistancia}");


                    if (y >= 470 && y <= 480)
                    {
                        //Console.WriteLine(valorDistancia);
                        colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                        colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                        colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                    }
                    else
                    {
                        if(x > 630 && x < 640 )
                        {
                            colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                            colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                            colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                        }
                        else
                        {
                            if (valorDistancia == miKinect.DepthStream.UnknownDepth) {
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = 255; //Rojo
                            }
                            else if (valorDistancia == miKinect.DepthStream.TooFarDepth) {
                                colorImagenDistancia[posColorImagenDistancia++] = 255; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                            }
                            else if (valorDistancia == miKinect.DepthStream.TooNearDepth)
                            {
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = 255; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                            }
                            else {
                                byte byteDistancia = (byte)(255 - (valorDistancia >> 5));
                                colorImagenDistancia[posColorImagenDistancia++] = byteDistancia; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = byteDistancia; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = byteDistancia; //Rojo
                            }

                        }
                    }
                        posColorImagenDistancia++;
                }

                if (bitmapImagenDistancia == null) {
                    this.bitmapImagenDistancia = new WriteableBitmap(
                        framesDistancia.Width,
                        framesDistancia.Height,
                        96,
                        96,
                        PixelFormats.Bgr32,
                        null);
                    DistanciaKinect.Source = bitmapImagenDistancia;
                }

                this.bitmapImagenDistancia.WritePixels(
                    new Int32Rect(0, 0, framesDistancia.Width, framesDistancia.Height),
                    colorImagenDistancia, //Datos de pixeles a color
                    framesDistancia.Width * 4,
                    0
                    );
                
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {

            Hsv lowerLimit = new Hsv(40, 100, 100);
            Hsv upperLimit = new Hsv(80, 255, 255);

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
  
                if (colorFrame != null)
                {
                    byte[] datosColor = new byte[colorFrame.PixelDataLength];

                    colorFrame.CopyPixelDataTo(datosColor);

                    System.Drawing.Bitmap bmp = Helper.ImageToBitmap(colorFrame);
                    Image<Hsv, Byte> currentFrameHSV = new Image<Hsv, byte>(bmp);
                    // Copy the pixel data from the image to a temporary array

                    Image<Gray, Byte> grayFrame = currentFrameHSV.Convert<Gray, Byte>();

                    Image<Gray, Byte> imageHSVDest = currentFrameHSV.InRange(lowerLimit, upperLimit);
                    //imageHSVDest.Erode(200);
                    VectorOfVectorOfPoint vectorOfPoint = Helper.FindContours(imageHSVDest);
                    //VectorOfPointF vf = new VectorOfPointF();
                    for (int i = 0; i < vectorOfPoint.Size; i++)
                    {
                        var contour = vectorOfPoint[i];
                        var area = CvInvoke.ContourArea(contour);
                        if (area > 100)
                        {

                            System.Drawing.Rectangle rec = CvInvoke.BoundingRectangle(contour);
                            Point p1 = new Point(rec.X, rec.Y);
                            Point p2 = new Point(rec.X + rec.Width, rec.Y + rec.Height);
                            ObjetoX = (p1.X + p2.X) / 2;
                            ObjetoY = (p1.Y + p2.Y) / 2;
                            //rect = new Rect(p1, p2);

                            Console.WriteLine($"x: {ObjetoX} y: {ObjetoY}");
                            //currentFrame.Draw(rec, new Bgr(0, double.MaxValue, 0), 3);
                        }
                    }
                    colorStream.Source = BitmapSource.Create(
                        colorFrame.Width, colorFrame.Height,
                        96,
                        96,
                        PixelFormats.Bgr32,
                        null,
                        datosColor,
                        colorFrame.Width * colorFrame.BytesPerPixel
                        );


                    //colorFrame.CopyPixelDataTo(this.colorPixels);
                    //// Write the pixel data into our bitmap
                    //this.colorBitmap.WritePixels(
                    //    new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                    //    this.colorPixels,
                    //    this.colorBitmap.PixelWidth * sizeof(int),
                    //    0);
                }
                
            }
        }

    }
}
