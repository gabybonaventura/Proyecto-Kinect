﻿namespace AtaxiaVision.Pantallas
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Ports;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Emgu.CV.Util;
    using Microsoft.Kinect;
    using Helpers;
    using AtaxiaVision.Models;
    using MaterialDesignThemes.Wpf;
    using AtaxiaVision.Controllers;
    using System.Windows.Input;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// Interaction logic for Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        #region Properties


        private const float RenderWidth = 640.0f;

        private const float RenderHeight = 480.0f;
        
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        private readonly Brush inferredJointBrush = Brushes.White;

        private readonly Pen trackedBonePen = new Pen(Brushes.Red, 6);

        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private KinectSensor sensor;

        private DrawingGroup drawingGroup;

        private DrawingImage imageSource;

        private WriteableBitmap colorBitmap;

        private byte[] colorPixels;

        short[] datosDistancia;
        byte[] colorImagenDistancia;
        double ObjetoX;
        double ObjetoY;
        int ObjetoZ;
        SkeletonPoint skelObjeto;
        Angulos angulos;
        Puntos puntos;

        bool flagSkeleton;

        bool flagObjeto;

        ArduinoController arduinoController;
        
        Joint ManoDerecha;
        Joint CodoDerecho;
        Joint HombroDerecho;

        Hsv lowerLimit = new Hsv(40, 100, 100);
        Hsv upperLimit = new Hsv(80, 255, 255);

        //------------------------------------//

        // Intermediate storage for the depth data received from the sensor
        private DepthImagePixel[] depthPixels;
        // Intermediate storage for the depth to color mapping
        private ColorImagePoint[] colorCoordinates;
        private int depthWidth;
        private int depthHeight;

        // Inverse scaling factor between color and depth
        private int colorToDepthDivisor;
        // Format we will use for the depth stream
        private const DepthImageFormat DepthFormat = DepthImageFormat.Resolution640x480Fps30;
        // Format we will use for the color stream
        private const ColorImageFormat ColorFormat = ColorImageFormat.RgbResolution640x480Fps30;

        //------------------------------------//

        private EjercicioViewModel Ejercicio { get; set; }
        private SesionViewModel Sesion { get; set; }

        #endregion Properties

        //Delegates
        public delegate void ImagenDelegate(DrawingImage drawingImage);
        public ImagenDelegate imagenDelegate;
        public delegate void ConsumoHombroArribaAbajoDelegate(int consumo);
        public ConsumoHombroArribaAbajoDelegate consumoHombroArribaAbajoDelegate;
        public delegate void ConsumoHombroAdelanteAtras(int consumo);
        public ConsumoHombroAdelanteAtras consumoHombroAdelanteAtrasDelegate;
        public delegate void ConsumoCodoArribaAbajo(int consumo);
        public ConsumoCodoArribaAbajo consumoCodoArribaAbajoDelegate;
        public delegate void ConsumoCodoDerechaIzquierda(int consumo);
        public ConsumoCodoDerechaIzquierda consumoCodoDerechaIzquierdaDelegate;

        //Para trabajar en background
        private BackgroundWorker TensionesServosBackgroundWorker = new BackgroundWorker();

        VideoController videoController;
        List<System.Drawing.Bitmap> framesBmp;

        #region Metodos Delegates
        private void SetConsumoHombroArribaAbajo(int consumo)
        {
            ConsumoHombroArribaAbajoLabel.Foreground = CalcularColor(consumo);
            ConsumoHombroArribaAbajoLabel.Content = consumo + " mA";
        }
        private void SetConsumoHombroAdelanteAtras(int consumo)
        {
            ConsumoHombroAdelanteAtrasLabel.Foreground = CalcularColor(consumo);
            ConsumoHombroAdelanteAtrasLabel.Content = consumo + " mA";
        }
        private void SetConsumoCodoArribaAbajo(int consumo)
        {
            ConsumoCodoArribaAbajoLabel.Foreground = CalcularColor(consumo);
            ConsumoCodoArribaAbajoLabel.Content = consumo + " mA";
        }
        private void SetConsumoCodoDerechaIzquierda(int consumo)
        {
            ConsumoCodoDerechaIzquierdaLabel.Foreground = CalcularColor(consumo);
            ConsumoCodoDerechaIzquierdaLabel.Content = consumo + " mA";
        }

        private Brush CalcularColor(int c)
        {
            if (c < 100)
                // Azul
                return new SolidColorBrush(Color.FromRgb(29, 22, 182));
            if (c < 200)
                // Verde
                return new SolidColorBrush(Color.FromRgb(22,182,51));
            if (c < 500)
                // Naranja
                return new SolidColorBrush(Color.FromRgb(226, 107, 25));
            // Rojo
            return new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }
        #endregion

        #region Metodos Background worker
        private void TensionesServosBG()
        {
            TensionesServosBackgroundWorker.DoWork += (s, e) =>
            {
                while (true)
                {   
                    int consumoHombroArribaAbajo = 20;
                    int consumoHombroAdelanteAtras = 100;
                    int consumoCodoArribaAbajo = 205;
                    int consumoCodoDerechaIzquierda = 550;
                    if (arduinoController.UltimaTension != null)
                    {
                        consumoHombroArribaAbajo = arduinoController.UltimaTension.HombroArribaAbajo;
                        consumoHombroAdelanteAtras = arduinoController.UltimaTension.HombroAdelanteAtras;
                        consumoCodoArribaAbajo = arduinoController.UltimaTension.CodoArribaAbajo;
                        consumoCodoDerechaIzquierda = arduinoController.UltimaTension.CodoIzquierdaDerecha;
                    }
                    ConsumoHombroArribaAbajoLabel.Dispatcher.Invoke(consumoHombroArribaAbajoDelegate, consumoHombroArribaAbajo);
                    ConsumoHombroAdelanteAtrasLabel.Dispatcher.Invoke(consumoHombroAdelanteAtrasDelegate, consumoHombroAdelanteAtras);
                    ConsumoCodoArribaAbajoLabel.Dispatcher.Invoke(consumoCodoArribaAbajoDelegate, consumoCodoArribaAbajo);
                    ConsumoCodoDerechaIzquierdaLabel.Dispatcher.Invoke(consumoCodoDerechaIzquierdaDelegate, consumoCodoDerechaIzquierda);
                    Thread.Sleep(1000);
                }
            };
            if (!TensionesServosBackgroundWorker.IsBusy)
                TensionesServosBackgroundWorker.RunWorkerAsync();
        }
        #endregion

        public Principal(SesionViewModel sesionVM, EjercicioViewModel ejercicioVM)
        {
            InitializeComponent();
            flagSkeleton = false;
            flagObjeto = false;
            datosDistancia = null;
            colorImagenDistancia = null;
            Sesion = sesionVM;
            Ejercicio = ejercicioVM;
            angulos = new Angulos();
            puntos = new Puntos();
            Ejercicio.Duracion = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            arduinoController = new ArduinoController();
            videoController = new VideoController();
            framesBmp = new List<System.Drawing.Bitmap>();
        }

        private void EstadoSnackBar(string mensaje)
        {
            Snackbar.IsActive = false;
            Snackbar.Message = new SnackbarMessage() { Content = mensaje };
            Snackbar.IsActive = true;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //Asigno lo delegates
            consumoHombroArribaAbajoDelegate = new ConsumoHombroArribaAbajoDelegate(SetConsumoHombroArribaAbajo);
            consumoHombroAdelanteAtrasDelegate = new ConsumoHombroAdelanteAtras(SetConsumoHombroAdelanteAtras);
            consumoCodoArribaAbajoDelegate = new ConsumoCodoArribaAbajo(SetConsumoCodoArribaAbajo);
            consumoCodoDerechaIzquierdaDelegate = new ConsumoCodoDerechaIzquierda(SetConsumoCodoDerechaIzquierda);

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                this.sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
                this.colorCoordinates = new ColorImagePoint[this.sensor.DepthStream.FramePixelDataLength];
                this.depthWidth = this.sensor.DepthStream.FrameWidth;
                this.depthHeight = this.sensor.DepthStream.FrameHeight;
                int colorWidth = this.sensor.ColorStream.FrameWidth;
                int colorHeight = this.sensor.ColorStream.FrameHeight;
                this.colorToDepthDivisor = colorWidth / this.depthWidth;
                this.sensor.AllFramesReady += this.SensorAllFramesReady;
                this.colorToDepthDivisor = colorWidth / this.depthWidth;
                depthPixels = new DepthImagePixel[sensor.DepthStream.FramePixelDataLength];

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                Image.Source = new BitmapImage(
                    new Uri("pack://application:,,,/AtaxiaVision;component/Imagenes/KinectAV.png"));
                EstadoSnackBar("Kinect no lista.");
            }

            arduinoController.Inicializar();
            TensionesServosBG();
        }

        private void SensorAllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            bool depthReceived = false;
            bool colorReceived = false;

            using (DepthImageFrame framesDistancia = e.OpenDepthImageFrame())
            {
                if (framesDistancia == null) return;

                framesDistancia.CopyDepthImagePixelDataTo(this.depthPixels);

                depthReceived = true;


                if (datosDistancia == null)
                    datosDistancia = new short[framesDistancia.PixelDataLength];

                if (colorImagenDistancia == null)
                    colorImagenDistancia = new byte[framesDistancia.PixelDataLength * 4];

                framesDistancia.CopyPixelDataTo(datosDistancia);
            }

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {

                if (colorFrame != null)
                {
                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    colorReceived = true;

                    System.Drawing.Bitmap bmp = EmguCVHelper.ImageToBitmap(colorFrame);

                    //videoController.InicioGrabacion = DateTime.Now.Ticks;

                    framesBmp.Add(bmp);

                    Image<Hsv, Byte> currentFrameHSV = new Image<Hsv, byte>(bmp);

                    Image<Gray, Byte> grayFrame = currentFrameHSV.Convert<Gray, Byte>();

                    Image<Gray, Byte> imageHSVDest = currentFrameHSV.InRange(lowerLimit, upperLimit);
                    imageHSVDest.Erode(100);
                    VectorOfVectorOfPoint vectorOfPoint = EmguCVHelper.FindContours(imageHSVDest);

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

                            if (true == depthReceived)
                            {
                                this.sensor.CoordinateMapper.MapDepthFrameToColorFrame(
                                    DepthFormat,
                                    this.depthPixels,
                                    ColorFormat,
                                    this.colorCoordinates);


                                int depthIndex = (int)ObjetoX + ((int)ObjetoY * this.depthWidth);
                                DepthImagePixel depthPixel = this.depthPixels[depthIndex];


                                ObjetoZ = datosDistancia[depthIndex] >> 3;

                                int X = (int)ObjetoX / this.colorToDepthDivisor;
                                int Y = (int)ObjetoY / this.colorToDepthDivisor;

                            }


                            if (ObjetoZ > 0)
                            {
                                skelObjeto = DistanceHelper.ObtenerSkelPoint((int)ObjetoX, (int)ObjetoY,
                                ObjetoZ, this.sensor);

                                flagObjeto = true;
                            }

                        }
                    }

                    colorFrame.CopyPixelDataTo(this.colorPixels);
                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                //dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                dc.DrawImage(this.colorBitmap, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        //Skeleton skel = skeletons[0];
                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                        }

                        //Toma de mediciones de mano, hombro y codo derecho:
                        ManoDerecha = skel.Joints[JointType.HandRight];
                        //Joint munecaDer = skel.Joints[JointType.WristRight];
                        CodoDerecho = skel.Joints[JointType.ElbowRight];
                        HombroDerecho = skel.Joints[JointType.ShoulderRight];

                        //Dibujo un punto negro sobre el objeto detectado
                        Point objeto = new Point(this.ObjetoX, this.ObjetoY);
                        dc.DrawEllipse(Brushes.Black, new Pen(Brushes.Black, 5), objeto, 5, 5);

                        if ((HombroDerecho.TrackingState == JointTrackingState.Tracked) &&
                                   (ManoDerecha.TrackingState == JointTrackingState.Tracked)
                                   && (CodoDerecho.TrackingState == JointTrackingState.Tracked))
                        {
                            if (flagObjeto && !flagSkeleton)
                            {
                                puntos = new Puntos(HombroDerecho, CodoDerecho, ManoDerecha, skelObjeto);
                                CalcularAngulosFinales(puntos);
                            }
                                

                            //Console.WriteLine($"Mano X Y Z {handRight.Position.X} {handRight.Position.Y} {handRight.Position.Z}");
                            //Console.WriteLine($"Objeto X Y Z {skelObjeto.X} {skelObjeto.Y} {skelObjeto.Z}");

                            if (DistanceHelper.ObtenerDistancia(ManoDerecha, skelObjeto) < 0.2)
                            {
                                Ejercicio.FinalizoConExito = true;
                                Cerrar();
                            }
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cerrar();
        }

        private void Cerrar()
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
            arduinoController.CerrarPuerto();
            //Confirmacion win = new Confirmacion(flagTokenValidado, desvios, resultado, valorToken, nro_ejercicio);
            Confirmacion win = new Confirmacion(Sesion, Ejercicio, arduinoController.Tensiones);
            Console.WriteLine("cierra por acá");
            win.Show();

            // Comento linea porque no tengo kinect y se rompe. Despues fijate gabi. (Ari)
            //videoController.GuardarVideo(framesBmp, $"Paciente{Sesion.Token} {DateTime.Now.ToString("ddMMyyyy")}");
            framesBmp = new List<System.Drawing.Bitmap>();

            this.Close();
            this.Close();
            this.Close();
            this.Close();
        }

        private void ConfirmacionButton_Click(object sender, RoutedEventArgs e)
        {
            arduinoController.EscribirAngulosArduino(angulos);
        }

        private void CalcularButton_Click(object sender, RoutedEventArgs e)
        {
           this.CalcularAngulosFinales(puntos);
        }

        public void CalcularAngulosFinales(Puntos puntos)
        {
            // ARREGLAR CAMI CASU
            angulos = new AngleHelper().SetValorAngulos(puntos);
                //HombroDerecho, ManoDerecha, CodoDerecho, skelObjeto);
            if (angulos.CodoArribaAbajo == -1 
                || angulos.CodoIzquierdaDerecha == -1 
                || angulos.HombroAdelanteAtras == -1 
                || angulos.HombroArribaAbajo == -1)
            {
                flagSkeleton = false;
            }
            else
            {
                flagSkeleton = true;
                //si se puede alcanzar el objeto,
                try
                {
                    this.ConfirmacionButton.IsEnabled = true;
                    //this.MensajesLabel.Content = "Angulos calculados";
                }
                catch (Exception)
                {
                    Console.WriteLine("Excepcion en escribir el angulo en arduino");
                }
            }
        }

        #region DibujarHuesos

        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.ShoulderRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

            }
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }
        #endregion DibujarHuesos

        private void FinEjercicioBtn_Click(object sender, RoutedEventArgs e)
        {
            //videoController.GuardarVideo(framesBmp, $"Paciente{Sesion.Token} {DateTime.Now.ToString("ddMMyyyy")}");
            framesBmp = new List<System.Drawing.Bitmap>();
            Confirmacion win = new Confirmacion(Sesion, Ejercicio, arduinoController.Tensiones);
            win.Show();
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FinEjercicioBtn_Click(sender,e);
            }
            if(ConfirmacionButton.IsEnabled && (e.Key == Key.I || e.Key == Key.Enter))
            {
                ConfirmacionButton_Click(sender, e);
            }
            if(e.Key == Key.A)
            {
               CalcularAngulosFinales(puntos);
            }
        }
    }
}
