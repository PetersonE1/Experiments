using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Drawing;

namespace ImageDataStorage
{
    public static class ImageProcessor
    {
        static readonly uint X1 = 256u;
        static readonly uint X2 = 256u * 256u;
        static readonly uint X3 = 256u * 256u * 256u;

        static void Main(string[] args)
        {
            
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

            Color c = Color.FromArgb(R, G, B, A);
            return c;
        }

        static uint[] ConvertImageToArray(string filePath)
        {
            Bitmap image = new Bitmap(@"image.png");
            int imageSize = image.Height * image.Width;
            uint[] imageData = new uint[imageSize];

            int i = 0;
            for (int h = 0; h < image.Width; h++)
            {
                for (int w = 0; w < image.Height; w++)
                {
                    imageData[i] = image.GetPixel(w, h).ConvertToInt();
                }
            }
            image.Dispose();

            return imageData;
        }

        static Bitmap ConvertToImage(uint[] data, int height) { 
            if (data.Length / height != (double)data.Length / (double)height)
            {
                Console.Write("(Pixels are not evenly divisible by height. Add blank pixels and continue, or adjust height automatically? Y/N) ");
                switch (Console.ReadLine())
                {
                    case "Y": height = Adjust(data.Length, height); break;
                    case "y": height = Adjust(data.Length, height); break;
                    case "N": break;
                    case "n": break;
                    default: Console.WriteLine("Invalid input"); return null;
                }

            }
            int width = data.Length / height + 1;
            Bitmap map = new Bitmap(width, height);
            return map;
        }

        static int Adjust(int length, int height)
        {
            int width = length / height;
            return 0;
        }
    }
}
