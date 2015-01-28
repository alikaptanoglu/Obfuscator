using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Obfuscator
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dg = new OpenFileDialog();
            dg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dg.FileName;
                bmp = new Bitmap(dg.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {  
            if (bmp != null)
            {
                pictureBox1.Image = seventhRandom(bmp);
            }
        }

        private Bitmap fullRandom(Bitmap bmp)
        {
            Random rnd = new Random();
            int newX, newY;
            Color c1, c2;

            for (int xCount = 0; xCount < bmp.Width; xCount++)
            {
                for (int yCount = 0; yCount < bmp.Height; yCount++)
                {
                    c1 = bmp.GetPixel(xCount, yCount);
                    newX = rnd.Next(0, bmp.Width);
                    newY = rnd.Next(0, bmp.Height);
                    c2 = bmp.GetPixel(newX, newY);

                    bmp.SetPixel(xCount, yCount, c2);
                    bmp.SetPixel(newX, newY, c1);
                }
            }

            return bmp;
        }

        private Bitmap seventhRandom(Bitmap bmp)
        {
            Random rnd = new Random();
            int newX, newY;
            Color c1, c2;

            for (int xCount = 0; xCount < bmp.Width; xCount++)
            {
                for (int yCount = 0; yCount < bmp.Height; yCount++)
                {
                    if (xCount % 7 != 0)
                    {
                        c1 = bmp.GetPixel(xCount, yCount);
                        newX = rnd.Next(0, bmp.Width);
                        newY = rnd.Next(0, bmp.Height);
                        c2 = bmp.GetPixel(newX, newY);

                        bmp.SetPixel(xCount, yCount, c2);
                        bmp.SetPixel(newX, newY, c1);
                    }
                }
            }

            return bmp;
        }
    }
}
