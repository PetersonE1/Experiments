using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ImageDataStorage;
using System.Drawing;

namespace EncryptionAlgorithm
{
    public static class EncryptionEngine
    {
        public static void Main(string[] args)
        {
            EncryptFileToImageFile("input.txt", "abstract.png");
            DecryptImageFileToFile("abstract.png", "output.txt");
        }

        // Base Methods

        /// <summary>
        /// Encrypts a string as a series of random numbers.
        /// </summary>
        /// <returns>
        /// A Message containing the encrypted data and both keys.
        /// </returns>
        public static Message EncryptString(string data)
        {
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] rno = new byte[8];
            random.GetNonZeroBytes(rno);
            long key1 = BitConverter.ToInt64(rno, 0);
            random.GetNonZeroBytes(rno);
            long key2 = BitConverter.ToInt64(rno, 0);

            string output = string.Join(" ", data.ToCharArray().Select(s => (Convert.ToInt64(s) * key1) + key2).ToArray());
            return new Message(output, key1, key2);
        }

        /// <summary>
        /// Decrypts a string of encrypted numbers with the given keys.
        /// </summary>
        /// <returns>
        /// A decrypted string.
        /// </returns>
        public static string DecryptString(string data, long key1, long key2)
        {
            string output = string.Join(null, data.Split(' ').Select(n => Convert.ToChar((Convert.ToInt64(n) - key2) / key1)).ToArray());
            return output;
        }

        /// <summary>
        /// Decrypts a Message.
        /// </summary>
        /// <returns>
        /// A decrypted string.
        /// </returns>
        public static string DecryptString(Message message)
        {
            string output = string.Join(null, message.data.Split(' ').Select(n => Convert.ToChar((Convert.ToInt64(n) - message.key2) / message.key1)).ToArray());
            return output;
        }

        // From File to Variable

        /// <summary>
        /// Encrypts the given file into a series of random numbers.
        /// </summary>
        /// <returns>
        /// A Message containing the encrypted data and both keys.
        /// </returns>
        public static Message EncryptFromFile(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return new Message("encryption error", 0, 0); }
            string data = File.ReadAllText(filePath);
            return EncryptString(data);
        }

        /// <summary>
        /// Decrypts the given file with keys included.
        /// </summary>
        /// <returns>
        /// A decrypted string.
        /// </returns>
        public static string DecryptFromFile(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return "decryption error"; }
            string[] data = File.ReadAllText(filePath).Split('\n');
            Message message = new Message(data[0], Convert.ToInt64(data[1]), Convert.ToInt64(data[2]));
            return DecryptString(message);
        }

        /// <summary>
        /// Decrypts the given file using the given keys.
        /// </summary>
        /// <returns>
        /// A decrypted string.
        /// </returns>
        public static string DecryptFromFile(string filePath, long key1, long key2)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return "decryption error"; }
            string data = File.ReadAllText(filePath);
            Message message = new Message(data, key1, key2);
            return DecryptString(message);
        }

        // From Variable to File

        /// <summary>
        /// Encrypts a string as a series of random numbers and saves it to a file.
        /// </summary>
        public static void EncryptToFile(string data, string filePath)
        {
            Message message = EncryptString(data);
            string output = message.ToString();
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// Decrypts a given string using the given keys and saves the output to a file.
        /// </summary>
        public static void DecryptToFile(string data, long key1, long key2, string filePath)
        {
            string output = DecryptString(data, key1, key2);
            File.WriteAllText(filePath, output);
        }

        /// <summary>
        /// Decrypts a given Message and saves the output to a file.
        /// </summary>
        public static void DecryptToFile(Message message, string filePath)
        {
            string output = DecryptString(message);
            File.WriteAllText(filePath, output);
        }

        // From File to File

        /// <summary>
        /// Encrypts the given file as a series of random numbers and saves the output to a file.
        /// </summary>
        public static void EncryptFile(string inputPath, string outputPath)
        {
            Message message = EncryptFromFile(inputPath);
            File.WriteAllText(outputPath, message.ToString());
        }

        /// <summary>
        /// Decrypts the given file with keys includes and saves the output to a file.
        /// </summary>
        public static void DecryptFile(string inputPath, string outputPath)
        {
            string output = DecryptFromFile(inputPath);
            File.WriteAllText(outputPath, output);
        }

        /// <summary>
        /// Decrypts the given file with the given keys and saves the output to a file.
        /// </summary>
        public static void DecryptFile(string inputPath, long key1, long key2, string outputPath)
        {
            string output = DecryptFromFile(inputPath, key1, key2);
            File.WriteAllText(outputPath, output);
        }

        // Base Methods - Image

        /// <summary>
        /// Encrypts a string into an image, storing the keys as the first two pixels.
        /// </summary>
        public static Bitmap EncryptStringToImage(string data)
        {
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] rno = new byte[2];
            random.GetNonZeroBytes(rno);
            uint key1 = BitConverter.ToUInt16(rno, 0);
            random.GetNonZeroBytes(rno);
            uint key2 = BitConverter.ToUInt16(rno, 0);

            uint[] output = data.ToCharArray().Select(s => (Convert.ToUInt32(s) * key1) + key2).ToArray();
            uint[] points = new uint[] { key1, key2 }.Concat(output).ToArray();

            return ImageProcessor.ConvertToImage(points);
        }

        /// <summary>
        /// Encrypts a string into an image, storing the keys in an external variable.
        /// </summary>
        public static Bitmap EncryptStringToImage(string data, out uint[] keys)
        {
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] rno = new byte[2];
            random.GetNonZeroBytes(rno);
            uint key1 = BitConverter.ToUInt16(rno, 0);
            random.GetNonZeroBytes(rno);
            uint key2 = BitConverter.ToUInt16(rno, 0);

            uint[] output = data.ToCharArray().Select(s => (Convert.ToUInt32(s) * key1) + key2).ToArray();

            keys = new uint[] { key1, key2 };
            return ImageProcessor.ConvertToImage(output);
        }

        /// <summary>
        /// Decrypts an image with keys included into a string.
        /// </summary>
        public static string DecryptImageToString(Bitmap map)
        {
            uint[] data = ImageProcessor.ConvertImageToArray(map);
            uint key1 = data[0];
            uint key2 = data[1];

            List<char> temp = new List<char>();
            for (int i = 2; i < data.Length; i++)
            {
                uint s = (data[i] - key2) / key1;
                temp.Add(Convert.ToChar(s));
            }
            string message = string.Join(null, temp);
            return message;
        }

        /// <summary>
        /// Decrypts an image into a string using the given keys.
        /// </summary>
        public static string DecryptImageToString(Bitmap map, uint key1, uint key2)
        {
            uint[] data = ImageProcessor.ConvertImageToArray(map);

            List<char> temp = new List<char>();
            for (int i = 0; i < data.Length; i++)
            {
                uint s = (data[i] - key2) / key1;
                temp.Add(Convert.ToChar(s));
            }
            string message = string.Join(null, temp);
            return message;
        }

        // From File to Variable - Image

        /// <summary>
        /// Encrypts the given file as an image, storing the keys in the first two pixels.
        /// </summary>
        public static Bitmap EncryptFileToImage(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return null; }
            string data = File.ReadAllText(filePath);
            return EncryptStringToImage(data);
        }

        /// <summary>
        /// Encrypts the given file as an image, storing the keys in an external variable.
        /// </summary>
        public static Bitmap EncryptFileToImage(string filePath, out uint[] keys)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); keys = null; return null; }
            string data = File.ReadAllText(filePath);
            return EncryptStringToImage(data, out keys);
        }

        /// <summary>
        /// Decrypts the given image with keys included into a string.
        /// </summary>
        public static string DecryptImageFileToString(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return "decryption error"; }
            Bitmap map = new Bitmap(filePath);
            return DecryptImageToString(map);
        }

        /// <summary>
        /// Decrypts the given image into a string using the given keys.
        /// </summary>
        public static string DecryptImageFileToString(string filePath, uint key1, uint key2)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return "decryption error"; }
            Bitmap map = new Bitmap(filePath);
            return DecryptImageToString(map, key1, key2);
        }

        // From Variable to File - Image

        /// <summary>
        /// Encrypts a string as an image saved to a file, storing the keys as the first two pixels.
        /// </summary>
        public static void EncryptStringToImageFile(string data, string filePath)
        {
            Bitmap map = EncryptStringToImage(data);
            map.Save(filePath);
        }

        /// <summary>
        /// Encrypts a string as an image saved to a file, storing the keys in a separate file.
        /// </summary>
        public static void EncryptStringToImageFile(string data, string filePath, string keyPath)
        {
            uint[] keys;
            Bitmap map = EncryptStringToImage(data, out keys);

            map.Save(filePath);
            File.WriteAllText(keyPath, string.Join('\n', keys));
        }

        /// <summary>
        /// Decrypts an image with keys included, saving the output to a file.
        /// </summary>
        public static void DecryptImageToFile(Bitmap map, string filePath)
        {
            string data = DecryptImageToString(map);
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// Decrypts an image using the given keys, saving the output to a file.
        /// </summary>
        public static void DecryptImageToFile(Bitmap map, uint key1, uint key2, string filePath)
        {
            string data = DecryptImageToString(map, key1, key2);
            File.WriteAllText(filePath, data);
        }

        // From File to File - Image

        /// <summary>
        /// Encrypts the given file to an image and saves the output to a file, storing the keys in the first two pixels.
        /// </summary>
        public static void EncryptFileToImageFile(string inputPath, string outputPath)
        {
            Bitmap map = EncryptFileToImage(inputPath);
            map.Save(outputPath);
        }

        /// <summary>
        /// Encrypts the given file to an image and saves the output to a file, storing the keys in a separate file.
        /// </summary>
        public static void EncryptFileToImageFile(string inputPath, string outputPath, string keyPath)
        {
            uint[] keys;
            Bitmap map = EncryptFileToImage(inputPath, out keys);

            map.Save(outputPath);
            File.WriteAllText(keyPath, string.Join('\n', keys));
        }

        /// <summary>
        /// Decrypts the given image file, keys included, and stores the output in a file.
        /// </summary>
        public static void DecryptImageFileToFile(string inputPath, string outputPath)
        {
            string data = DecryptImageFileToString(inputPath);
            File.WriteAllText(outputPath, data);
        }

        /// <summary>
        /// Decrypts the given image file using the given keys and stores the output in a file.
        /// </summary>
        public static void DecryptImageFileToFile(string inputPath, string outputPath, uint key1, uint key2)
        {
            string data = DecryptImageFileToString(inputPath, key1, key2);
            File.WriteAllText(outputPath, data);
        }
    }

    public struct Message
    {
        public string data;
        public long key1;
        public long key2;

        public Message(string data, long key1, long key2)
        {
            this.data = data;
            this.key1 = key1;
            this.key2 = key2;
        }

        public override string ToString()
        {
            return $"{data}\n{key1}\n{key2}";
        }
    }
}