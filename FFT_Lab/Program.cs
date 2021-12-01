using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FFT_Lab
{
    class Program
    {
        public static int width, height;
        public static int Radius = 30;
        public static int Chosen; //опция выбора частного фильтра
        public static Bitmap originalPic = new Bitmap(Image.FromFile($@"{SaveExt.Desktop}\1.jpg"));

        static void Main(string[] args)
        {
            width = originalPic.Width;
            height = originalPic.Height;
            Radius = originalPic.Width / 2;

            SaveExt.Image(LowFrequency(), "LowFrequency");
            SaveExt.Image(HighFrequency(), "HighFrequency");
        }

        private static Bitmap LowFrequency()
        {
            Chosen = 0;
            return Job();
        }
        private static Bitmap HighFrequency()
        {
            Chosen = 1;
            return Job();
        }
        private static Bitmap Job()
        {
            Complex[,] FFT = Filter.FFT(ref originalPic);
            int P = width / 2;
            int Q = height / 2;
            double D;
            int D0 = 30;
            int option = Chosen;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    D = Math.Sqrt(Math.Pow(i - P, 2) + Math.Pow(j - Q, 2));
                    if (D > D0 && option == 0)
                    {
                        FFT[i, j] = 0;
                    }
                    else if (D <= D0 && option == 1)
                    {
                        FFT[i, j] = 0;
                    }
                }
            }
            return Filter.FFTInverse(FFT);
        }

        public static class SaveExt
        {
            public static readonly string Desktop = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";
            public static void Image(Bitmap bitmap, string name)
            {
                bitmap.Save($@"{Desktop}\{name}.jpg", ImageFormat.Png);
            }
        }
    }
}