using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



using Emgu;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

namespace Detection
{
    public partial class Form1 : Form
    {
        private VideoCapture capture = null;

        private double frames;

        private double framesCounter;

        private double fps;

        private bool play = false;



        public Form1()
        {
            InitializeComponent();
        }

       

           
        

        private Image<Bgr, byte> Find(Image<Bgr, byte> image)
        {
            

            MCvObjectDetection[] regions;

            using (HOGDescriptor descriptor = new HOGDescriptor())
            {
                descriptor.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
                regions = descriptor.DetectMultiScale(image);

            }

            foreach (MCvObjectDetection i in regions)
            {
                image.Draw(i.Rect, new Bgr(Color.Red), 3);
               
                
            }

            return image;
        }

        private async void ReadFrames()
        {
            Mat m = new Mat();

            while (play && framesCounter < frames)

            {
                framesCounter += 1;

                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, framesCounter);

                capture.Read(m);

                pictureBox1.Image = m.Bitmap;

                pictureBox2.Image = Find(m.ToImage<Bgr, byte>()).Bitmap;

                toolStripLabel1.Text = $"{framesCounter} / {frames}";

                await Task.Delay(1000 / Convert.ToInt16(fps));


            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult res = openFileDialog1.ShowDialog();

                if (res == DialogResult.OK)
                {
                    capture = new VideoCapture(openFileDialog1.FileName);

                    Mat m = new Mat();

                    capture.Read(m);

                    pictureBox1.Image = m.Bitmap;

                    fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                    frames = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);

                    framesCounter = 1;

                }
                else
                {
                    MessageBox.Show("Видео не выбрано!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void распознатьОбьектыToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                if (capture == null)

                    throw new Exception("Видео не выбрано !");

                play = true;

                ReadFrames();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                play = false;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            try
            {

                if (capture == null)

                    throw new Exception("Видео не выбрано !");

                play = true;

                ReadFrames();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {

                framesCounter -= Convert.ToDouble(numericUpDown1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {

                framesCounter += Convert.ToDouble(numericUpDown1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            play = false;

            Mat m = new Mat();

            capture.Read(m);

            Make screen = new Make(Find2(m.ToImage<Bgr, byte>()));

            screen.Show();


                Image<Bgr, byte> Find2(Image<Bgr, byte> image)
                {

                StreamWriter sw = new StreamWriter("C:\\Users\\User\\Desktop\\Отчет.txt");

                sw.WriteLine($"Отчет по обьектам из кадра № {framesCounter}");

                

                int number = 1;

                MCvObjectDetection[] regions;

                using (HOGDescriptor descriptor = new HOGDescriptor())
                {
                    descriptor.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
                    regions = descriptor.DetectMultiScale(image);

                }

                sw.WriteLine($"Количество обьектов на кадре {regions.Length}");

                foreach (MCvObjectDetection i in regions)
                {
                    image.Draw(i.Rect, new Bgr(Color.Red), 3);

                    sw.WriteLine($"Харакстеристика обьекта № {number}");
                    sw.WriteLine($"Высота обьекта = {i.Rect.Height} пикселя");
                    sw.WriteLine($"Ширина обьекта = {i.Rect.Width} пикселя ");
                    sw.WriteLine($"Координаты центра обьекта X={(i.Rect.X + i.Rect.Height) / 2}  Y= { (i.Rect.Y + i.Rect.Width)/2}");

                    CvInvoke.PutText(image, $"Object {number}", new Point(i.Rect.X, i.Rect.Y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, new MCvScalar(0, 255, 255), 2);
                    sw.WriteLine();
                    number++;
                    
                }
            
                 sw.Close();
                 return image;
        }

    }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    
}

