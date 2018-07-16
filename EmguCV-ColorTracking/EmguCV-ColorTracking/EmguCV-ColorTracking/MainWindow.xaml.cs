using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.Drawing;

namespace EmguCV_ColorTracking
{

    public partial class MainWindow : Window
    {
        private VideoCapture _capture;
        DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hsv lowerLimit = new Hsv(40, 100, 100);
            Hsv upperLimit = new Hsv(80, 255, 255);



            _capture = new VideoCapture();

            _timer = new DispatcherTimer();
            _timer.Tick += (_, __) =>
            {
                Image<Bgr, Byte> currentFrame = _capture.QueryFrame().ToImage<Bgr, Byte>();
                Image<Hsv, Byte> currentFrameHSV = currentFrame.Convert<Hsv, Byte>();

                if (currentFrame != null)
                {
                    Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();

                    Image<Gray, Byte> imageHSVDest = currentFrameHSV.InRange(lowerLimit, upperLimit);
                    //imageHSVDest.Erode(200);
                    VectorOfVectorOfPoint vectorOfPoint = Helper.FindContours(imageHSVDest);
                    for (int i = 0; i < vectorOfPoint.Size; i++)
                    {
                        var contour = vectorOfPoint[i];
                        var area = CvInvoke.ContourArea(contour);
                        if (area > 100)
                        {
                            Rectangle rec = CvInvoke.BoundingRectangle(contour);
                            currentFrame.Draw(rec, new Bgr(0, double.MaxValue, 0), 3);

                        }
                    }

                    image1.Source = ToBitmapSource(currentFrame);
                    image2.Source = ToBitmapSource(imageHSVDest);
                }
            };
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _timer.Start();

        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
    }
}
