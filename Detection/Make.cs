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
    public partial class Make : Form
    {
        private Image<Bgr, byte> image = null;
        public Make(Image<Bgr , byte> image)
          
        {
            this.image = image;

            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Make_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = image.Bitmap;

        
            
        }
    }
}
