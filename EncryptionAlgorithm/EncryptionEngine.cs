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
            string output = DecryptImage("abstract.png");
            EncryptStringToImage(output);
        }

        // Base Methods
        
        public static Message EncryptString(string data)
        {
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] rno = new byte[8];
            random.GetNonZeroBytes(rno);
            long key1 = BitConverter.ToInt64(rno, 0);
            random.GetNonZeroBytes(rno);
            long key2 = BitConverter.ToInt64(rno, 0);

            string output = string.Join(" ", data.ToCharArray().Select(s => (Convert.ToInt64(s) + key1) * key2).ToArray());
            return new Message(output, key1, key2);
        }

        public static string DecryptString(string data, long key1, long key2)
        {
            string output = string.Join(null, data.Split(' ').Select(n => Convert.ToChar((Convert.ToInt64(n) / key2) - key1)).ToArray());
            return output;
        }

        public static string DecryptString(Message message)
        {
            string output = string.Join(null, message.data.Split(' ').Select(n => Convert.ToChar((Convert.ToInt64(n) / message.key2) - message.key1)).ToArray());
            return output;
        }

        // From File to Variable

        public static Message EncryptFromFile(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return new Message("encryption error", 0, 0); }
            string data = File.ReadAllText(filePath);
            return EncryptString(data);
        }

        public static string DecryptFromFile(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return "decryption error"; }
            string[] data = File.ReadAllText(filePath).Split('\n');
            Message message = new Message(data[0], Convert.ToInt64(data[1]), Convert.ToInt64(data[2]));
            return DecryptString(message);
        }

        public static string DecryptFromFile(string filePath, long key1, long key2)
        {
            if (!File.Exists(filePath)) { Console.WriteLine($"File {filePath} not found."); return "decryption error"; }
            string data = File.ReadAllText(filePath);
            Message message = new Message(data, key1, key2);
            return DecryptString(message);
        }

        // From Variable to File

        public static void EncryptToFile(string data, string filePath)
        {
            Message message = EncryptString(data);
            string output = message.ToString();
            File.WriteAllText(filePath, data);
        }

        public static void DecryptToFile(string data, long key1, long key2, string filePath)
        {
            string output = DecryptString(data, key1, key2);
            File.WriteAllText(filePath, output);
        }

        public static void DecryptToFile(Message message, string filePath)
        {
            string output = DecryptString(message);
            File.WriteAllText(filePath, output);
        }

        // From File to File

        public static void EncryptFile(string inputPath, string outputPath)
        {
            Message message = EncryptFromFile(inputPath);
            File.WriteAllText(outputPath, message.ToString());
        }

        public static void DecryptFile(string inputPath, string outputPath)
        {
            string output = DecryptFromFile(inputPath);
            File.WriteAllText(outputPath, output);
        }

        public static void DecryptFile(string inputPath, long key1, long key2, string outputPath)
        {
            string output = DecryptFromFile(inputPath, key1, key2);
            File.WriteAllText(outputPath, output);
        }

        // Image

        public static Bitmap EncryptStringToImage(string data)
        {
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] rno = new byte[2];
            random.GetNonZeroBytes(rno);
            uint key1 = BitConverter.ToUInt16(rno, 0);
            random.GetNonZeroBytes(rno);
            uint key2 = BitConverter.ToUInt16(rno, 0);

            uint[] output = data.ToCharArray().Select(s => (Convert.ToUInt32(s) + key1) * key2).ToArray();

            uint[] points = new uint[]{ key1, key2 }.Concat(output).ToArray();

            return ImageProcessor.ConvertToImage(points);
        }

        public static string DecryptImage(string filePath)
        {
            uint[] data = ImageProcessor.ConvertImageToArray(filePath);
            uint key1 = data[0];
            uint key2 = data[1];
            Console.WriteLine(key1);
            Console.WriteLine(key2);

            List<char> temp = new List<char>();
            for (int i = 2; i < data.Length; i++)
            {
                uint s = data[i] / key2 - key1;
                temp.Add(Convert.ToChar(s));
            }
            string message = string.Join(null, temp);
            return message;
        }

        // Extras

        public static Message ConvertToMessage(string data, long key1, long key2)
        {
            return new Message(data, key1, key2);
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