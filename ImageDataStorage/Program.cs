using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;

namespace ImageDataStorage
{
    public static class ImageProcessor
    {
        static readonly uint X1 = 256u;
        static readonly uint X2 = 256u * 256u;
        static readonly uint X3 = 256u * 256u * 256u;

        static void Main(string[] args)
        {
            uint[] array = ConvertImageToArray(@"gradient.png");

            Bitmap map = ConvertToImage(array);

            map.Save(@"image2.png");
        }

        static uint ConvertToInt(this Color c)
        {
            uint result = 0;
            result += c.R;
            result += c.G * X1;
            result += c.B * X2;
            result += c.A * X3;
            return result;
        }

        static Color ConvertToColor(this uint i)
        {
            int R, G, B, A = 0;

            A = (int)(i / X3);
            i -= X3 * (uint)A;
            B = (int)(i / X2);
            i -= X2 * (uint)B;
            G = (int)(i / X1);
            i -= X1 * (uint)G;
            R = (int)(i);

            Color c = Color.FromArgb(A, R, G, B);
            return c;
        }

        public static uint[] ConvertImageToArray(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            Bitmap image = new Bitmap(filePath);
            Console.WriteLine(image.Height);
            int imageSize = image.Height * image.Width;
            uint[] imageData = new uint[imageSize];

            int i = 0;
            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    Color c = image.GetPixel(w, h);
                    imageData[i] = image.GetPixel(w, h).ConvertToInt();
                    i++;
                }
            }
            image.Dispose();

            return imageData;
        }

        public static Bitmap ConvertToImage(uint[] data) {
            List<int> heights = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                if (IsFactor(i, data.Length)) heights.Add(i);
            }

            Console.WriteLine($"The available heights are: {string.Join(", ", heights)}");
            Console.Write("Choose a height: ");
            string result;
            while (!ReadIfContains(heights.Select(s => s.ToString()).ToArray(), out result)) {
                Console.Write("\n Please enter a valid height: ");
            }
            int height = Convert.ToInt32(result);

            int width = data.Length / height;
            Bitmap map = new Bitmap(width, height);

            int index = 0;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    map.SetPixel(w, h, data[index].ConvertToColor());
                    index++;
                }
            }

            return map;
        }

        public static bool IsFactor(int a, int b)
        {
            if (a != 0 && b % a == 0) return true;
            return false;
        }

        private static bool ReadIfContains(string[] values, out string n)
        {
            n = Console.ReadLine();
            return values.Contains(n);
        }
    }
}
