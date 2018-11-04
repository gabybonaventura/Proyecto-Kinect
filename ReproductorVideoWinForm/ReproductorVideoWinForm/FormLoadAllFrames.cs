using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace ReproductorVideoWinForm
{
    public partial class FormLoadAllFrames : Form
    {
        VideoCapture videocapture;
        bool IsPlaying = false;
        int TotalFrames;
        int CurrentFrameNo;
        Mat CurrentFrame;
        int FPS;
        bool Adelante = true;

        public FormLoadAllFrames()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video Files (*.mp4, *.flv, *.avi)| *.mp4;*.flv;*.avi";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                videocapture = new VideoCapture(ofd.FileName);
                TotalFrames = Convert.ToInt32(videocapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount));
                FPS = Convert.ToInt32(videocapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
                IsPlaying = true;
                CurrentFrame = new Mat();
                CurrentFrameNo = 0;
                trackBar1.Minimum = 0;
                trackBar1.Maximum = TotalFrames - 1;
                trackBar1.Value = 0;
                PlayVideo();
            }
        }

        private async void PlayVideo()
        {
            if (videocapture == null)
            {
                return;
            }

            try
            {
                //while (IsPlaying == true && CurrentFrameNo < TotalFrames)
                //{
                //    videocapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, CurrentFrameNo);
                //    videocapture.Read(CurrentFrame);
                //    pictureBox1.Image = CurrentFrame.Bitmap;
                //    trackBar1.Value = CurrentFrameNo;
                //    CurrentFrameNo += 1;
                //    await Task.Delay(500 / FPS);
                //}
                CurrentFrameNo = 1;

                //while (IsPlaying == true && CurrentFrameNo < TotalFrames)
                //{
                //    videocapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, CurrentFrameNo);
                //    videocapture.Read(CurrentFrame);
                //    Bitmap bitmap = CurrentFrame.Bitmap;
                //}

                    while (IsPlaying == true && CurrentFrameNo < TotalFrames)
                {
                    videocapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, CurrentFrameNo);
                    videocapture.Read(CurrentFrame);
                    pictureBox1.Image = CurrentFrame.Bitmap;
                    trackBar1.Value = CurrentFrameNo;
                    if (CurrentFrameNo == 0)
                        Adelante = true;
                    if (CurrentFrameNo == 50)
                        Adelante = false;

                    if(Adelante)
                        CurrentFrameNo += 1;
                    else
                        CurrentFrameNo -= 1;

                    await Task.Delay(125 / FPS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (videocapture != null)
            {
                IsPlaying = true;
                PlayVideo();
            }

            else
            {
                IsPlaying = false;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            IsPlaying = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            IsPlaying = false;
            CurrentFrameNo = 0;
            trackBar1.Value = 0;
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (videocapture != null)
            {
                CurrentFrameNo = trackBar1.Value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videocapture != null && CurrentFrame != null)
            {
                CurrentFrame.Save("C:\\Users\\Public\\Videos\\" + CurrentFrameNo.ToString() + ".jpg");
            }
        }

        private void FormLoadAllFrames_Load(object sender, EventArgs e)
        {

        }
    }
}
