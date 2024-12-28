using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionAlgorithm
{
    public static class ImageEncryptionEngine
    {

        /// <summary>
        /// Converts a string to a Color[] array.
        /// </summary>
        /// <param name="s">string to be converted.</param>
        /// <returns>A Color[] array encoding the string data.</returns>
        public static Color[] StringToColors(string s)
        {
            // 7 bits per char (7 * s.Length)
            // Must be in multiples of 8 (bytes)
            // Total bits = 7 * s.Length
            // Bytes = t_bits / 8

            int bitCount = s.Length * 7;
            int byteCount = bitCount % 8 == 0 ? bitCount / 8 : (bitCount / 8) + 1;
            int extraBytes = 4 - (byteCount % 4);

            byte[] bytes = new byte[byteCount + extraBytes];
            char[] chars = s.ToCharArray();
            Color[] colors = new Color[bytes.Length / 4];

            // 11111112 22222233 33333444 44445555 55566666 66777777 78888888

            int index = 0;
            for (int i = 0; i < s.Length; i += 8)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i + j >= s.Length) break;
                    byte b = (byte)(chars[i + j] << (j + 1));
                    bytes[j + index * 7] = b;

                    if (i + j + 1 >= s.Length) break;
                    b += (byte)(chars[i + j + 1] >> (6 - j));
                    bytes[j + index * 7] = b;
                }

                index++;
            }

            for (int i = 0; i < bytes.Length; i += 4)
            {
                int argb = bytes[i];
                for (int j = 1; j < 4; j++)
                {
                    argb <<= 8;
                    argb += bytes[i + j];
                }
                Color color = Color.FromArgb(argb);
                colors[i / 4] = color;
            }

            return colors;
        }

        /// <summary>
        /// Converts a Color[] array to a string.
        /// </summary>
        /// <param name="colors">The Color[] array to parse.</param>
        /// <returns>A string containing the encoded data.</returns>
        public static string ColorsToString(Color[] colors)
        {
            byte[] bytes = new byte[colors.Length * 4];
            for (int i = 0; i < colors.Length * 4; i += 4)
            {
                bytes[i] = (byte)(colors[i / 4].ToArgb() >> 24);
                bytes[i + 1] = (byte)(colors[i / 4].ToArgb() >> 16);
                bytes[i + 2] = (byte)(colors[i / 4].ToArgb() >> 8);
                bytes[i + 3] = (byte)colors[i / 4].ToArgb();
            }

            // 11111112 22222233 33333444 44445555 55566666 66777777 78888888

            StringBuilder sb = new();

            for (int i = 0; i < bytes.Length; i += 7)
            {
                byte b = (byte)(bytes[i] >> 1);
                sb.Append((char)b);
                byte map = 0b00000000;

                for (int j = 1; j < 7; j++)
                {
                    if (i + j >= bytes.Length) return sb.ToString().TrimEnd();

                    map = (byte)((map << 1) | 0b00000001);
                    byte mask = (byte)(bytes[i + j - 1] & map);
                    b = (byte)((bytes[i + j] >> (j + 1)) + (mask << (7 - j)));
                    sb.Append((char)b);
                }

                b = (byte)(bytes[i + 6] & 0b01111111);
                sb.Append((char)b);
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Converts a Color array to a Bitmap. Makes the bitmap as square as possible.
        /// </summary>
        /// <param name="colors">The Color[] array containing pixel data.</param>
        /// <returns>A bitmap with pixels defined by the Color[] array.</returns>
        public static Bitmap ColorsToBitmap(Color[] colors)
        {
            int root = (int)Math.Sqrt(colors.Length);
            int width = 0;
            for (int i = 1; i <= root; i++)
            {
                if (colors.Length % i == 0)
                    width = i;
            }
            int height = colors.Length / width;

            Bitmap bitmap = new(width, height);

            int index = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, colors[index++]);
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Converts a Bitmap to a Color[] array.
        /// </summary>
        /// <param name="bitmap">The Bitmap to be converted.</param>
        /// <returns>A Color[] array of flattened pixel data.</returns>
        public static Color[] BitmapToColors(Bitmap bitmap)
        {
            Color[] colors = new Color[bitmap.Width * bitmap.Height];

            int index = 0;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    colors[index++] = bitmap.GetPixel(x, y);
                }
            }

            return colors;
        }
    }
}
