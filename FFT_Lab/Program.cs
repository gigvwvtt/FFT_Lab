using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace FFT_Lab
{
    class Program
    {
        public static int width, height;
        private static int FilterOption; //опция выбора частного фильтра
        public static Bitmap originalPic ;
        //public static Bitmap originalPic = new Bitmap(Image.FromFile($@"{SaveExt.Desktop}\1.jpg"));


        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                originalPic = new Bitmap(Image.FromFile(args[0]));
            }
            width = originalPic.Width;
            height = originalPic.Height;

            SaveExt.Image(LowFrequency(), "LowFrequency");
            SaveExt.Image(HighFrequency(), "HighFrequency");
        }

        private static Bitmap LowFrequency()
        {
            FilterOption = 0;
            return Job();
        }
        private static Bitmap HighFrequency()
        {
            FilterOption = 1;
            return Job();
        }
        private static Bitmap Job()
        {
            Complex[,] FFT = Filter.FFT(ref originalPic);
            int P = width / 2;
            int Q = height / 2;
            double D;
            const int D0 = 30; //сила фильтрации

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    D = Math.Sqrt(Math.Pow(i - P, 2) + Math.Pow(j - Q, 2));
                    if (D > D0 && FilterOption == 0)
                    {
                        FFT[i, j] = 0;
                    }
                    else if (D <= D0 && FilterOption == 1)
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