using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
                pictureBox1.Image = jigsaw(bmp, 8);
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

        private Bitmap jigsaw(Bitmap bmp, int parts)
        {
            Random rnd = new Random();
            int xStep = bmp.Width / parts;
            int yStep = bmp.Height / parts;

            List<Point> pList = new List<Point>();

            for (int xCount = 0; xCount < bmp.Width; xCount += xStep)
            {
                for (int yCount = 0; yCount < bmp.Height; yCount += yStep)
                {
                    pList.Add(new Point(xCount, yCount));
                }
            }

            Point current, destination;
            /*for (int i = 0; i < pList.Count; i++)
            {
                selector = rnd.Next(0, pList.Count);
                CopyBmpRegion(bmp, new Rectangle(pList[i].X, pList[i].Y, xStep, yStep), pList[selector]);
            }*/
            while (pList.Count > 1)
            {
                current = pList[rnd.Next(0, pList.Count)];
                destination = pList[rnd.Next(0, pList.Count)];
                while (current.Equals(destination))
                {
                    current = pList[rnd.Next(0, pList.Count)];
                }
                CopyBmpRegion(bmp, new Rectangle(current.X, current.Y, xStep, yStep), destination);
                // TODO: This only makes sense if we're really swapping, not just copying one over the other.
                //       This way we're only touching a random amount more than half of the rectangles.
                pList.Remove(current);
                pList.Remove(destination);
            }

            return bmp;
        }

        private static void CopyBmpRegion(Bitmap image, Rectangle srcRect, Point destLocation)
        {
            //do some argument sanitising.
            if (!((srcRect.X >= 0 && srcRect.Y >= 0) && ((srcRect.X + srcRect.Width) <= image.Width) && ((srcRect.Y + srcRect.Height) <= image.Height)))
                throw new ArgumentException("Source rectangle isn't within the image bounds.");

            if ((destLocation.X < 0 || destLocation.X > image.Width) || (destLocation.Y < 0 || destLocation.Y > image.Height))
                throw new ArgumentException("Destination must be within the image.");

            // Lock the bits into memory
            BitmapData bmpData = image.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.ReadWrite, image.PixelFormat);
            int pxlSize = (bmpData.Stride / bmpData.Width); //calculate the pixel width (in bytes) of the current image.
            int src = 0; int dest = 0; //source/destination pixels.

            //account for the fact that not all of the source rectangle may be able to copy into the destination:
            int width = (destLocation.X + srcRect.Width) <= image.Width ? srcRect.Width : (image.Width - (destLocation.X + srcRect.Width));
            int height = (destLocation.Y + srcRect.Height) <= image.Height ? srcRect.Height : (image.Height - (destLocation.Y + srcRect.Height));

            //managed buffer to hold the current pixel data.
            byte[] buffer = new byte[pxlSize];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //calculate the start of the current source pixel and destination pixel.
                    src = ((srcRect.Y + y) * bmpData.Stride) + ((srcRect.X + x) * pxlSize);
                    dest = ((destLocation.Y + y) * bmpData.Stride) + ((destLocation.X + x) * pxlSize);

                    // Can replace this with unsafe code, but that's up to you.
                    Marshal.Copy(new IntPtr(bmpData.Scan0.ToInt32() + src), buffer, 0, pxlSize);
                    Marshal.Copy(buffer, 0, new IntPtr(bmpData.Scan0.ToInt32() + dest), pxlSize);
                }
            }

            image.UnlockBits(bmpData); //unlock the data.
        }
    }
}
