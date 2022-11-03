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
        // Constants
        public const uint X1 = 256u;
        public const uint X2 = 256u * 256u;
        public const uint X3 = 256u * 256u * 256u;

        // Item Conversions

        /// <summary>
        /// Converts a color into an unsigned integer using tiered steps.
        /// </summary>
        static uint ConvertToInt(this Color c)
        {
            uint result = 0;
            result += c.R;
            result += c.G * X1;
            result += c.B * X2;
            result += c.A * X3;
            return result;
        }

        /// <summary>
        /// Converts an unsigned integer into a color using tiered steps.
        /// </summary>
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

        // Data Conversions

        /// <summary>
        /// Converts an image to an array of unsigned integers from the given filepath.
        /// </summary>
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

        /// <summary>
        /// Converts an image to an array of unsigned integers from the given bitmap.
        /// </summary>
        public static uint[] ConvertImageToArray(Bitmap image)
        {
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

        /// <summary>
        /// Converts an unsigned integer array into a bitmap.
        /// </summary>
        public static Bitmap ConvertToImage(uint[] data) {
            List<int> heights = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                if (IsValid(i, data.Length)) heights.Add(i);
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

        /// <summary>
        /// Converts an unsigned integer array into a bitmap of the given height if possible. Otherwise, defaults to the minimum height.
        /// </summary>
        public static Bitmap ConvertToImage(uint[] data, int attemptHeight)
        {
            List<int> heights = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                if (IsValid(i, data.Length)) heights.Add(i);
            }

            if (!heights.Contains(attemptHeight)) attemptHeight = heights[0];
            int height = attemptHeight;

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

        // Wrapped Methods

        /// <summary>
        /// Generates a .png image from an unsigned integer array at the given filepath.
        /// </summary>
        public static void GenerateImageFromFile(string inputPath, string outputPath)
        {
            if (!File.Exists(inputPath)) return;
            string raw_data = File.ReadAllText(inputPath);
            uint[] array = raw_data.Split('\n').Select(n => Convert.ToUInt32(n)).ToArray();
            Bitmap map = ConvertToImage(array);
            map.Save(outputPath + $"-H{map.Height}.png");
        }

        /// <summary>
        /// Generates a .png image of the given height from an unsigned integer array at the given filepath, if possible. Otherwise, defaults to the minimum height.
        /// </summary>
        public static void GenerateImageFromFile(string inputPath, string outputPath, int targetHeight)
        {
            if (!File.Exists(inputPath)) return;
            string raw_data = File.ReadAllText(inputPath);
            uint[] array = raw_data.Split('\n').Select(n => Convert.ToUInt32(n)).ToArray();
            Bitmap map = ConvertToImage(array, targetHeight);
            map.Save(outputPath + $"-H{map.Height}.png");
        }

        /// <summary>
        /// Generates a .png image from a given unsigned integer array.
        /// </summary>
        public static void GenerateImageFromArray(uint[] array, string outputPath)
        {
            Bitmap map = ConvertToImage(array);
            map.Save(outputPath + $"-H{map.Height}.png");
        }

        /// <summary>
        /// Generates a .png image of the given height from a given unsigned integer array, if possible. Otherwise, defaults to the minimum height.
        /// </summary>
        public static void GenerateImageFromArray(uint[] array, string outputPath, int targetHeight)
        {
            Bitmap map = ConvertToImage(array, targetHeight);
            map.Save(outputPath + $"-H{map.Height}.png");
        }

        /// <summary>
        /// Saves an unsigned integer array to a file.
        /// </summary>
        public static void SaveArrayToFile(uint[] array, string outputPath)
        {
            string data = string.Join('\n', array);
            File.WriteAllText(outputPath, data);
        }

        // Data Processing

        /// <summary>
        /// Prompts the user for input. If the response matches a value, return 'true' and output the response.
        /// </summary>
        private static bool ReadIfContains(string[] values, out string n)
        {
            n = Console.ReadLine();
            return values.Contains(n);
        }

        /// <summary>
        /// Prompts the user to choose a valid height of a bitmap given an array.
        /// </summary>
        public static int AskHeight(int length)
        {
            List<int> heights = new List<int>();
            for (int i = 0; i < length; i++)
            {
                if (IsValid(i, length)) heights.Add(i);
            }

            Console.WriteLine($"The available heights are: {string.Join(", ", heights)}");
            Console.Write("Choose a height: ");
            string result;
            while (!ReadIfContains(heights.Select(s => s.ToString()).ToArray(), out result))
            {
                Console.Write("\n Please enter a valid height: ");
            }
            int height = Convert.ToInt32(result);
            return height;
        }

        // Bonus Methods

        /// <summary>
        /// Checks if 'a' is a factor of 'b'.
        /// </summary>
        public static bool IsFactor(int a, int b)
        {
            if (a != 0 && b % a == 0) return true;
            return false;
        }

        /// <summary>
        /// Checks if a given length and height will produce a valid bitmap.
        /// </summary>
        public static bool IsValid(int a, int b)
        {
            if (IsFactor(a, b) && a < 65536 && b / a < 65536) return true;
            return false;
        }

        /// <summary>
        /// Checks if there is an integer root of 'a'.
        /// </summary>
        public static bool IsRootable(int a)
        {
            int b = (int)Math.Sqrt(a);
            if (a == b * b) return true;
            return false;
        }

        /// <summary>
        /// Scales a given bitmap.
        /// </summary>
        public static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(sourceBMP, 0, 0, width, height);
            }
            return result;
        }

        /// <summary>
        /// Returns the integer root of a number if possible. Otherwise, returns '1'.
        /// </summary>
        public static int Square(int length)
        {
            if (IsRootable(length)) return (int)Math.Sqrt(length);
            Console.WriteLine($"Length {length} not divisible by two (2)");
            return 1;
        }
    }
}
