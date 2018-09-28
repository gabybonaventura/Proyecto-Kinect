//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.Ports;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Media.Media3D;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Emgu.CV.Util;
    using Microsoft.Kinect;
    using Microsoft.Samples.Kinect.SkeletonBasics.Helpers;

    public partial class MainWindow : Window
    {
        
        Rect rect;
        private const float RenderWidth = 640.0f;

        private const float RenderHeight = 480.0f;

        private const double JointThickness = 3;

        private const double BodyCenterThickness = 10;

        private const double ClipBoundsThickness = 10;

        private readonly Brush centerPointBrush = Brushes.Blue;

        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        private readonly Brush inferredJointBrush = Brushes.Yellow;

        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private KinectSensor sensor;

        private DrawingGroup drawingGroup;

        private DrawingImage imageSource;

        private WriteableBitmap colorBitmap;

        private byte[] colorPixels;

        private readonly Pen HandHandPen = new Pen(Brushes.Red, 6);

        WriteableBitmap bitmapEficiente = null;

        short[] datosDistancia = null;
        byte[] colorImagenDistancia = null;
        WriteableBitmap bitmapImagenDistancia = null;
        double ObjetoX;
        double ObjetoY;
        int valorDistancia;
        int ObjetoZ;
        SkeletonPoint skelObjeto;


        private SerialPort _serialPort = new SerialPort();
        private int _baudRate = 9600;
        private int _dataBits = 8;
        private Handshake _handshake = Handshake.None;
        private Parity _parity = Parity.None;
        private string _portName = "COM5";
        private StopBits _stopBits = StopBits.One;

        double AnguloCodo = 0;
        double AnguloHombroArriba = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
       
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
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
                //this.Image.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
                this.sensor.ColorFrameReady += this.SensorColorFrameReady;
                this.sensor.DepthFrameReady += this.miKinect_DepthFrameReady;

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
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }


            try
            {
                _serialPort.BaudRate = _baudRate;
                _serialPort.DataBits = _dataBits;
                _serialPort.Handshake = _handshake;
                _serialPort.Parity = _parity;
                _serialPort.PortName = _portName;
                _serialPort.StopBits = _stopBits;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }



        /*private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {

            Hsv lowerLimit = new Hsv(40, 100, 100);
            Hsv upperLimit = new Hsv(80, 255, 255);

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {

                if(colorFrame != null)
                {
                    if (colorFrame != null)
                    {


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
                                rect = new Rect(p1, p2);
                                Console.WriteLine($"x: {(p1.X + p2.X) / 2} y: {(p1.Y + p2.Y) / 2}");
                                //currentFrame.Draw(rec, new Bgr(0, double.MaxValue, 0), 3);

                            }
                        }

                        Point3D point3D;


                        colorFrame.CopyPixelDataTo(this.colorPixels);
                        // Write the pixel data into our bitmap
                        this.colorBitmap.WritePixels(
                            new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                            this.colorPixels,
                            this.colorBitmap.PixelWidth * sizeof(int),
                            0);
                    }
                }
            }
        }*/

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {

            Hsv lowerLimit = new Hsv(40, 100, 100);
            Hsv upperLimit = new Hsv(80, 255, 255);

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {

                if (colorFrame != null)
                {
                    if (colorFrame != null)
                    {
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
                                rect = new Rect(p1, p2);
                                ObjetoX = (p1.X + p2.X) / 2;
                                ObjetoY = (p1.Y + p2.Y) / 2;

                                //Console.WriteLine($"objeto x: {(p1.X + p2.X) / 2} objeto y: {(p1.Y + p2.Y) / 2}");
                                //currentFrame.Draw(rec, new Bgr(0, double.MaxValue, 0), 3);

                                skelObjeto = DistanceHelper.ObtenerSkelPoint((int)ObjetoX, (int)ObjetoY,
                                    ObjetoZ, this.sensor);
                                /*DepthImagePoint dip1 = new DepthImagePoint();
                                dip1.X = (int)ObjetoX;
                                dip1.Y = (int)ObjetoY;
                                dip1.Depth = ObjetoZ;

                                skelObjeto = this.sensor.CoordinateMapper.MapDepthPointToSkeletonPoint(DepthImageFormat.Resolution640x480Fps30, dip1);
                                // SkeletonPoint sp1 = CoordinateMapper.MapDepthPointToSkeletonPoint(DepthImageFormat.Resolution640x480Fps30, dip1);
                                */
                                /*Console.WriteLine("obj x: " + ObjetoX + "obj y: " + ObjetoY 
                                    + "obj z: " + ObjetoZ);*/
                                /*Console.WriteLine("punto x: " + skelObjeto.X.ToString() + "punto y: "
                                    + skelObjeto.Y.ToString() + "punto z: " + skelObjeto.Z.ToString());
                            */
                                Console.WriteLine("pos y objeto:" + skelObjeto.Y + "pixel y:" + (int)ObjetoY);
                            }
                        }

                        colorFrame.CopyPixelDataTo(this.colorPixels);
                        // Write the pixel data into our bitmap
                        this.colorBitmap.WritePixels(
                            new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                            this.colorPixels,
                            this.colorBitmap.PixelWidth * sizeof(int),
                            0);

                        /*using (DrawingContext dc = this.drawingGroup.Open())
                        {
                            // Draw a transparent background to set the render size
                            //dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                            dc.DrawImage(this.colorBitmap, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                            dc.DrawText(new FormattedText
                                ("y:" + skelObjeto.Y,
                                CultureInfo.GetCultureInfo("en-us"),
                                FlowDirection.LeftToRight,
                                new Typeface("Verdana"),
                                25, System.Windows.Media.Brushes.BlueViolet),
                                new Point(skelObjeto.X, skelObjeto.Y));
                        }*/
                    }

                }
            }
        }

        void miKinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame framesDistancia = e.OpenDepthImageFrame())
            {
                if (framesDistancia == null) return;

                if (datosDistancia == null)
                    datosDistancia = new short[framesDistancia.PixelDataLength];

                if (colorImagenDistancia == null)
                    colorImagenDistancia = new byte[framesDistancia.PixelDataLength * 4];

                framesDistancia.CopyPixelDataTo(datosDistancia);

                int posColorImagenDistancia = 0;
                int x = 0;
                int y = 0;
                for (int i = 0; i < framesDistancia.PixelDataLength; i++)
                {
                    if (x < 639)
                        x++;
                    else
                    {
                        y++;
                        x = 0;
                    }
                    valorDistancia = datosDistancia[i] >> 3;

                    if (x == ObjetoX && y == ObjetoY)
                    {
                        ObjetoZ = valorDistancia;
                        if(_serialPort.IsOpen)
                            _serialPort.Write(ObjetoX.ToString() + ObjetoY.ToString() + valorDistancia.ToString());
                        //Console.WriteLine($"Z: {valorDistancia}");
                        //Console.WriteLine(ObjetoX.ToString() + ObjetoY.ToString() + valorDistancia.ToString());

                    }


                    if (y >= 470 && y <= 480)
                    {
                        //Console.WriteLine(valorDistancia);
                        colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                        colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                        colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                    }
                    else
                    {
                        if (x > 630 && x < 640)
                        {
                            colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                            colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                            colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                        }
                        else
                        {
                            if (valorDistancia == sensor.DepthStream.UnknownDepth)
                            {
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = 255; //Rojo
                            }
                            else if (valorDistancia == sensor.DepthStream.TooFarDepth)
                            {
                                colorImagenDistancia[posColorImagenDistancia++] = 255; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                            }
                            else if (valorDistancia == sensor.DepthStream.TooNearDepth)
                            {
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = 255; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = 0; //Rojo
                            }
                            else
                            {
                                byte byteDistancia = (byte)(255 - (valorDistancia >> 5));
                                colorImagenDistancia[posColorImagenDistancia++] = byteDistancia; //Azul
                                colorImagenDistancia[posColorImagenDistancia++] = byteDistancia; //Verde
                                colorImagenDistancia[posColorImagenDistancia++] = byteDistancia; //Rojo
                            }

                        }
                    }
                    posColorImagenDistancia++;
                }

                if (bitmapImagenDistancia == null)
                {
                    this.bitmapImagenDistancia = new WriteableBitmap(
                        framesDistancia.Width,
                        framesDistancia.Height,
                        96,
                        96,
                        PixelFormats.Bgr32,
                        null);
                    //DistanciaKinect.Source = bitmapImagenDistancia;
                }

                this.bitmapImagenDistancia.WritePixels(
                    new Int32Rect(0, 0, framesDistancia.Width, framesDistancia.Height),
                    colorImagenDistancia, //Datos de pixeles a color
                    framesDistancia.Width * 4,
                    0
                    );

            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

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
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }

                        //Toma de mediciones de mano, hombro y codo derecho:
                        Joint handRight = skel.Joints[JointType.HandRight];
                        Joint munecaDer = skel.Joints[JointType.WristRight];
                        Joint codoDer = skel.Joints[JointType.ElbowRight];
                        Joint shoulderRight = skel.Joints[JointType.ShoulderRight];
                        Joint hipRight = skel.Joints[JointType.HipRight];

                        if ((shoulderRight.TrackingState == JointTrackingState.Tracked) &&
                                   (handRight.TrackingState == JointTrackingState.Tracked)
                                   && (codoDer.TrackingState == JointTrackingState.Tracked))
                        {
                            //dibujo las líneas de los segmentos:
                             //mano hombro
                           /* dc.DrawLine(this.HandHandPen, this.SkeletonPointToScreen(handRight.Position),
                                this.SkeletonPointToScreen(shoulderRight.Position));
                            //mano codo
                            dc.DrawLine(this.HandHandPen, this.SkeletonPointToScreen(handRight.Position),
                                this.SkeletonPointToScreen(codoDer.Position));
                            //hombro codo
                            dc.DrawLine(this.HandHandPen, this.SkeletonPointToScreen(shoulderRight.Position),
                                this.SkeletonPointToScreen(codoDer.Position));
                            */
                            Point objeto = new Point(this.ObjetoX, this.ObjetoY);
                            //Console.WriteLine("punto obj: " + objeto.ToString());

                            //convierto los Joint en SkeletonPoint para que sea compatible el eje x y z.

                            

                            //Hombro
                           /* SkeletonPoint skelHombro = DistanceHelper.ObtenerSkelPoint((int)
                                shoulderRight.Position.X, (int)shoulderRight.Position.Y,
                                (int)shoulderRight.Position.Z, this.sensor);

                            SkeletonPoint skelCodo = DistanceHelper.ObtenerSkelPoint((int)
                                codoDer.Position.X, (int)codoDer.Position.Y,
                                (int)codoDer.Position.Z, this.sensor);

                            SkeletonPoint skelMano = DistanceHelper.ObtenerSkelPoint((int)
                                handRight.Position.X, (int)handRight.Position.Y,
                                (int)handRight.Position.Z, this.sensor);*/

                            dc.DrawLine(this.HandHandPen, this.SkeletonPointToScreen(shoulderRight.Position),
                                objeto);
                            /*
                            float distHombroMano = DistanceHelper.ObtenerDistancia(skelMano, skelHombro);
                            float distCodoMano = DistanceHelper.ObtenerDistancia(skelMano, skelCodo);
                            float distHombroCodo = DistanceHelper.ObtenerDistancia(skelCodo, skelHombro);*/

                            float distHombroMano = DistanceHelper.ObtenerDistancia(handRight, shoulderRight);
                            float distCodoMano = DistanceHelper.ObtenerDistancia(handRight, codoDer);
                            float distHombroCodo = DistanceHelper.ObtenerDistancia(codoDer, shoulderRight);

                            float distCadHombro = DistanceHelper.ObtenerDistancia(hipRight, shoulderRight);
                            float distCadMano = DistanceHelper.ObtenerDistancia(handRight, hipRight);

                            //Point3D objeto3d = new Point3D(this.ObjetoX, this.ObjetoY, this.ObjetoZ);
                            float distObjeto = DistanceHelper.ObtenerDistancia(shoulderRight, skelObjeto);

                            //el primer argumento es el segmento opuesto al angulo que queremos obtener.
                            double anguloCodo = DistanceHelper.CalcularAngulo(distHombroMano, distHombroCodo, distCodoMano);

                            double anguloHombroCad = DistanceHelper.CalcularAngulo(distCadMano, distHombroMano, distCadHombro);
                            //el siguiente angulo que obtenemos es el del hombro en relación con 
                            //el torso. 

                            Console.WriteLine("angulo hombro: " + anguloHombroCad);
                            //codo menos hombro (codo>hombro)
                            //Point puntoMedioHC = new Point((codoDer.Position.X + shoulderRight.Position.X) / 2, (codoDer.Position.Y + shoulderRight.Position.Y) / 2);

                           /* Point puntoMedioHC = new Point((codoDer.Position.X + shoulderRight.Position.X) / 2,
                                (codoDer.Position.Y + shoulderRight.Position.Y) / 2);
                                */
                            Point puntoMedioHO = new Point((skelObjeto.X + shoulderRight.Position.X) / 2,
                                (skelObjeto.Y + shoulderRight.Position.Y) / 2);

                            if(ObjetoY == 0)
                            {
                                Console.WriteLine("NO RECONOCE OBJETO");
                            }
                            else { 
                            dc.DrawText(
                            new FormattedText(distObjeto.ToString(),
                                              CultureInfo.GetCultureInfo("en-us"),
                                              FlowDirection.LeftToRight,
                                              new Typeface("Verdana"),
                                              25, System.Windows.Media.Brushes.BlueViolet),
                                              puntoMedioHO);
                            }

                            var distenMt = distObjeto * 100;
                            //Console.WriteLine("distancia hombro obj en cm: " + distenMt.ToString());
                            Console.WriteLine("hombro x: " + shoulderRight.Position.X + "hombro y: " + shoulderRight.Position.Y
                                + "hombro z: " + shoulderRight.Position.Z);

                            dc.DrawText(
                            new FormattedText("ang hombro: " + anguloHombroCad,
                                              CultureInfo.GetCultureInfo("en-us"),
                                              FlowDirection.LeftToRight,
                                              new Typeface("Verdana"),
                                              25, System.Windows.Media.Brushes.Red),
                                              this.SkeletonPointToScreen(shoulderRight.Position));

                            dc.DrawText(
                            new FormattedText("dist: " + distenMt,
                                              CultureInfo.GetCultureInfo("en-us"),
                                              FlowDirection.LeftToRight,
                                              new Typeface("Verdana"),
                                              40, System.Windows.Media.Brushes.Blue),
                                              objeto);


                            dc.DrawText(
                            new FormattedText(anguloCodo.ToString(),
                                              CultureInfo.GetCultureInfo("en-us"),
                                              FlowDirection.LeftToRight,
                                              new Typeface("Verdana"),
                                              25, System.Windows.Media.Brushes.BlueViolet),
                                              this.SkeletonPointToScreen(codoDer.Position));
                            /*string distHombroMano = DistanceHelper.ObtenerDistancia(handRight, shoulderRight).ToString();
                                                        string distCodoMano = DistanceHelper.ObtenerDistancia(handRight, codoDer).ToString();
                                                        string distHombroCodo = DistanceHelper.ObtenerDistancia(codoDer, shoulderRight).ToString();
                                                        */
                            // Double anguloCodo = DistanceHelper.CalcularAngulo(codoDer, handRight, shoulderRight);
                            /* Console.WriteLine("Distancia codo mano: " + distCodoMano);
                             Console.WriteLine("Distancia hombro codo: " + distHombroCodo); */
                            //Console.WriteLine("Angulo: " + anguloCodo);
                        }

                        /*      // Toma de Mediciones
                              Joint elbowLeft = skel.Joints[JointType.ElbowLeft];
                              Joint handLeft = skel.Joints[JointType.HandLeft];
                              Joint shoulderLeft = skel.Joints[JointType.ShoulderLeft];

                              Joint shoulderCenter = skel.Joints[JointType.ShoulderCenter];

                              Joint HipRight = skel.Joints[JointType.HipRight];
                              //Joint shoulderRight = skel.Joints[JointType.ShoulderRight];
                              Joint elbowRight = skel.Joints[JointType.ElbowRight];
                              //Joint handRight = skel.Joints[JointType.HandRight];

                              AnguloCodo = DistanceHelper.Angulos(shoulderRight, elbowRight, handRight);
                              AnguloHombroArriba = DistanceHelper.Angulos(HipRight, shoulderRight, elbowRight);
                              // Al generar el angulo del hombro mediante a los puntos de la cadera izquierda
                              // el angulo obtenido no es del todo preciso, ya que la cadera izquierda esta
                              // "dentro" del cuerpo y no es recto desde el hombro.
                              */
                        /*if (AnguloHombroArriba != 0 && AnguloHombroArriba != 0)
                            Console.WriteLine("Angulo Codo: " + AnguloCodo + "Angulo Hombre: " + AnguloHombroArriba);
                            */
                        //if ((handLeft.TrackingState == JointTrackingState.Tracked) &&
                        //           (handRight.TrackingState == JointTrackingState.Tracked))
                        //{
                        //    dc.DrawLine(this.HandHandPen, this.SkeletonPointToScreen(handRight.Position), this.SkeletonPointToScreen(handLeft.Position));
                        //    //DistanceHelper.Angulos(handLeft, shoulderCenter, handRight);

                        //    string distance = DistanceHelper.ObtenerDistancia(handLeft, handRight).ToString();
                        //    FormattedText formattedText = new FormattedText(distance, CultureInfo.CurrentCulture, FlowDirection.LeftToRight
                        //       ,new Typeface("Verdana") , 50, Brushes.Red );
                        //    double x = ((handLeft.Position.X + handRight.Position.X) / 2) * 320;
                        //    double y = 240 - (((handLeft.Position.Y + handRight.Position.Y) / 2) * 240);
                        //    //double x = 320;
                        //    //double y = 240;
                        //    //640x480

                        //    //dc.DrawText(formattedText, new Point(x,y));
                        //    Console.WriteLine("x" + x.ToString());
                        //    Console.WriteLine("y" + y.ToString());

                        //}
                        //string setDistanceHands;

                        //    dc.DrawRectangle(Brushes.Red, null, rect);

                        //setDistanceHands = "" + DistanceHelper.ObtenerDistancia(handLeft, handRight);
                        //if(setDistanceHands != "0")
                        //    DistanceHandHand.Text = setDistanceHands;
                        //Console.WriteLine("La distancia es:" + setDistanceHands);

                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                
            }
        }

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

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

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

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
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

    }
}