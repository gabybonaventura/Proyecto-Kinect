namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Ports;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Emgu.CV.Util;
    using Microsoft.Kinect;
    using Microsoft.Samples.Kinect.SkeletonBasics.App_inicial;
    using Microsoft.Samples.Kinect.SkeletonBasics.Helpers;

    public partial class MainWindow : Window
    {
        #region Properties


        Rect rect;
        private const float RenderWidth = 640.0f;

        private const float RenderHeight = 480.0f;

        private const double JointThickness = 3;

        private const double BodyCenterThickness = 10;

        private const double ClipBoundsThickness = 10;

        private readonly Brush centerPointBrush = Brushes.DarkRed;

        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        private readonly Brush inferredJointBrush = Brushes.White;

        private readonly Pen trackedBonePen = new Pen(Brushes.Red, 6);

        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private KinectSensor sensor;

        private DrawingGroup drawingGroup;

        private DrawingImage imageSource;

        private WriteableBitmap colorBitmap;

        private byte[] colorPixels;

        private readonly Pen HandHandPen = new Pen(Brushes.Red, 6);

        short[] datosDistancia = null;
        byte[] colorImagenDistancia = null;
        double ObjetoX;
        double ObjetoY;
        int ObjetoZ;
        SkeletonPoint skelObjeto;
        double[] angulos;
        
        bool flagSkeleton = false;
        
        bool flagObjeto = false;

        private SerialPort _serialPort = new SerialPort();
        private int _baudRate = 9600;
        private int _dataBits = 8;
        private Handshake _handshake = Handshake.None;
        private Parity _parity = Parity.None;
        private string _portName = "COM5";
        private StopBits _stopBits = StopBits.One;

        private bool flagTokenValidado, resultado=false;
        private string valorToken;
        private int nro_ejercicio;

        List<Angulos> desvios /*= new List<Angulos>()*/;


        Joint handRight;
        Joint codoDer;
        Joint shoulderRight;
        Joint hipRight;

        int contadorAuxiliarDepthFrame = 0;

        Hsv lowerLimit = new Hsv(40, 100, 100);
        Hsv upperLimit = new Hsv(80, 255, 255);

        //------------------------------------//

        // Intermediate storage for the depth data received from the sensor
        private DepthImagePixel[] depthPixels;
        // Intermediate storage for the color data received from the camera
        //private byte[] colorPixels;
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

        #endregion Properties


        //se inicializa con un flag de la ventana anterior
        //sirve para saber si se validó la sesión previamente.
        public MainWindow(bool flagToken, string token, int nroEj)
        {
            WindowState = WindowState.Maximized;
            flagTokenValidado = flagToken;
            this.valorToken = token;
            desvios = new List<Angulos>();
            nro_ejercicio = nroEj;
            nro_ejercicio++;
            InitializeComponent();

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


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
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
                //this.Image.Source = this.colorBitmap;

                this.colorCoordinates = new ColorImagePoint[this.sensor.DepthStream.FramePixelDataLength];
                this.depthWidth = this.sensor.DepthStream.FrameWidth;
                this.depthHeight = this.sensor.DepthStream.FrameHeight;
                int colorWidth = this.sensor.ColorStream.FrameWidth;
                int colorHeight = this.sensor.ColorStream.FrameHeight;
                this.colorToDepthDivisor = colorWidth / this.depthWidth;
                this.sensor.AllFramesReady += this.SensorAllFramesReady;


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
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                _serialPort.Open();
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write("*070090030050");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
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

                // Calculo la posicion del vector Depth Frame obteniendo la posicion donde esta
                // Alojada la profundidad del objeto.
                //if (ObjetoX > 0 && ObjetoY > 0)
                //{
                //    int posicionDelObjetoEnVector = Convert.ToInt32(640 * ObjetoY + ObjetoX - 1);
                //    ObjetoZ = datosDistancia[posicionDelObjetoEnVector] >> 3;
                //    if (contadorAuxiliarDepthFrame == 100)
                //    {
                //        Console.WriteLine($"Z del objeto: {ObjetoZ}");
                //        contadorAuxiliarDepthFrame = 0;
                //    }
                //    else
                //        contadorAuxiliarDepthFrame++;
                //}
                //Console.WriteLine($"Z del objeto: {ObjetoZ}");
                //Console.WriteLine($"Z del objeto SKEL: {skelObjeto.Z}");


            }

            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {

                if (colorFrame != null)
                {

                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    colorReceived = true;

                    System.Drawing.Bitmap bmp = Helper.ImageToBitmap(colorFrame);
                    Image<Hsv, Byte> currentFrameHSV = new Image<Hsv, byte>(bmp);
                    // Copy the pixel data from the image to a temporary array

                    Image<Gray, Byte> grayFrame = currentFrameHSV.Convert<Gray, Byte>();

                    Image<Gray, Byte> imageHSVDest = currentFrameHSV.InRange(lowerLimit, upperLimit);
                    imageHSVDest.Erode(100);
                    VectorOfVectorOfPoint vectorOfPoint = Helper.FindContours(imageHSVDest);

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


                                // scale color coordinates to depth resolution
                                int X = (int)ObjetoX / this.colorToDepthDivisor;
                                int Y = (int)ObjetoY / this.colorToDepthDivisor;

                                // depthPixel is the depth for the (X,Y) pixel in the color frame
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
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                        }

                        //Toma de mediciones de mano, hombro y codo derecho:
                        handRight = skel.Joints[JointType.HandRight];
                        //Joint munecaDer = skel.Joints[JointType.WristRight];
                        codoDer = skel.Joints[JointType.ElbowRight];
                        shoulderRight = skel.Joints[JointType.ShoulderRight];
                        hipRight = skel.Joints[JointType.HipRight];

                        //Dibujo un punto negro sobre el objeto detectado
                        Point objeto = new Point(this.ObjetoX, this.ObjetoY);
                        dc.DrawEllipse(Brushes.Black, new Pen(Brushes.Black, 5), objeto, 5, 5);

                        if ((shoulderRight.TrackingState == JointTrackingState.Tracked) &&
                                   (handRight.TrackingState == JointTrackingState.Tracked)
                                   && (codoDer.TrackingState == JointTrackingState.Tracked))
                        {
                            if (flagObjeto && !flagSkeleton)
                                CalcularAngulosFinales();

                            float distAux = DistanceHelper.ObtenerDistancia(handRight, skelObjeto);

                            //Console.WriteLine($"Mano X Y Z {handRight.Position.X} {handRight.Position.Y} {handRight.Position.Z}");
                            //Console.WriteLine($"Objeto X Y Z {skelObjeto.X} {skelObjeto.Y} {skelObjeto.Z}");


                            if (DistanceHelper.ObtenerDistancia(handRight, skelObjeto) < 0.1)
                            {
                                //significa que se llegó al objeto, por lo que se cierra la ventana y se envían
                                //los datos.
                                resultado = true;
                                this.Close();
                            }
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
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

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.WriteLine(indata);
            if (!string.IsNullOrEmpty(indata))
            {
                desvios.Add(new Angulos(indata));
            }
        }
        
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
            if (_serialPort.IsOpen)
                _serialPort.Close();
            Confirmacion win = new Confirmacion(flagTokenValidado, desvios, resultado, valorToken, nro_ejercicio);
            Console.WriteLine("cierra por acá");
            win.Show();
            
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
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        private void ConfirmacionButton_Click(object sender, RoutedEventArgs e)
        {
            ArduinoHelper.EscribirAngulosArduino(_serialPort, angulos);

        }
        private void CalcularButton_Click(object sender, RoutedEventArgs e)
        {
            this.CalcularAngulosFinales();
        }

        public void CalcularAngulosFinales()
        {
            
            angulos = AngleHelper.SetValorAngulos(shoulderRight,
            handRight, codoDer, hipRight, skelObjeto);
            if (angulos[0] == -1 || angulos[1] == -1 || angulos[2] == -1 || angulos[3] == -1)
            {
                //Console.WriteLine("no se puede alcanzar el objeto");
                /*dc.DrawText(
                new FormattedText("No se puede alcanzar el objeto",
                            CultureInfo.GetCultureInfo("en-us"),
                            FlowDirection.LeftToRight,
                            new Typeface("Verdana"),
                            25, System.Windows.Media.Brushes.Red),
                            new Point(0,0));*/
                flagSkeleton = false;
            }
            else
            {
                flagSkeleton = true;
                //si se puede alcanzar el objeto,
                try
                {
                    this.ConfirmacionButton.IsEnabled = true;
                    this.MensajesLabel.Content = "Angulos calculados";
                }
                catch (Exception)
                {
                    Console.WriteLine("Excepcion en escribir el angulo en arduino");
                }
            }
            
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