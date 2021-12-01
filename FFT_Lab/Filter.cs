using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;

namespace FFT_Lab
{
    public class Filter
    {
        public static Complex w(int k, int N)
        {
            if (k % N == 0) return 1;
            double arg = -2 * Math.PI * k / N;
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }
        public static Complex[] FFT1D(Complex[] x1, int option)
        {
            Complex[] X;
            int N = x1.Length;
            if (N == 2)
            {
                X = new Complex[2];
                X[0] = x1[0] + x1[1];
                X[1] = x1[0] - x1[1];
            }
            else
            {
                Complex[] x_even = new Complex[N / 2];
                Complex[] x_odd = new Complex[N / 2];
                for (int i = 0; i < N / 2; i++)
                {
                    x_even[i] = x1[2 * i];
                    x_odd[i] = x1[2 * i + 1];
                }
                Complex[] X_even = FFT1D(x_even, option);
                Complex[] X_odd = FFT1D(x_odd, option);
                X = new Complex[N];
                for (int i = 0; i < N / 2; i++)
                {
                    X[i] = X_even[i] + w(i * option, N) * X_odd[i];
                    X[i + N / 2] = X_even[i] - w(i * option, N) * X_odd[i];
                }
            }
            return X;
        }
        public static Complex[,] FFT2D(Complex[,] x, int option)
        {
            Complex[,] result = new Complex[Program.width, Program.height];
            // строки
            Complex[] x1 = new Complex[Program.width];
            for (int j = 0; j < Program.height; j++)
            {
                for (int i = 0; i < Program.width; i++)
                {
                    x1[i] = x[i, j];

                }
                Complex[] X = FFT1D(x1, option);
                for (int i = 0; i < Program.width; i++)
                {
                    result[i, j] = X[i];

                }
            }
            // столбцы
            x1 = new Complex[Program.height];
            for (int i = 0; i < Program.width; i++)
            {
                for (int j = 0; j < Program.height; j++)
                {
                    x1[j] = result[i, j];

                }
                Complex[] X = FFT1D(x1, option);
                for (int k = 0; k < Program.height; k++)
                {
                    result[i, k] = X[k];

                }
            }
            return result;
        }
        public static Bitmap FFTInverse(Complex[,] Arr)// обратный БПФ
        {
            var originalpicture = new Bitmap(Program.originalPic);
            Arr = FFT2D(Arr, -1);
            Bitmap rendered = new Bitmap(originalpicture.Width, originalpicture.Height);
            int C;
            int MN = Program.width * Program.height;
            for (int x = 0; x < originalpicture.Width; x++)
            {
                for (int y = 0; y < originalpicture.Height; y++)
                {

                    C = (int)((Arr[x, y].Real / MN) * Math.Pow(-1, x + y));
                    if (C > 255)
                    {
                        C = 255;
                    }
                    if (C < 0)
                    {
                        C = 0;
                    }
                    Color newColor = Color.FromArgb(C, C, C);
                    rendered.SetPixel(x, y, newColor);
                }
            }
            return rendered;
        }
        public static Complex[,] FFT(ref Bitmap original)
        {
            Program.width = original.Width;
            Program.height = original.Height;
            int powerW = (int)Math.Ceiling(Math.Log((double)Program.width, 2));
            int powerH = (int)Math.Ceiling(Math.Log((double)Program.height, 2));
            Program.height = (int)Math.Pow(2, powerH);
            Program.width = (int)Math.Pow(2, powerW);
            Complex[,] x = new Complex[Program.width, Program.height];
            for (int i = 0; i < Program.width; i++)
            {
                for (int j = 0; j < Program.height; j++)
                {
                    if (i < original.Width && j < original.Height)
                    {
                        x[i, j] = original.GetPixel(i, j).R * Math.Pow(-1, i + j);
                    }
                    else
                    {
                        x[i, j] = 0;
                    }
                }
            }
            Complex[,] res = FFT2D(x, 1);
            return res;
        }
    }
}