using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;

namespace ImageDataStorage
{
    public static class Runner
    {
        const int length = 10000;

        static void Main(string[] args)
        {
            Fibonacci();
        }

        static private void Quadratic(bool askForHeight = false)
        {
            int height = ImageProcessor.Square(length);
            uint a = 123;
            uint b = 456;
            uint c = 789;


            if (!askForHeight && File.Exists($"outputs/quadratic-{length}-{a}-{b}-{c}-H{height}.png")) {Console.WriteLine("File already exists"); return; }

            uint[] array = new uint[length];
            for (uint x = 0; x < length; x++)
            {
                array[x] = a * (x * x) + b * x + c;
            }

            if (askForHeight) { height = ImageProcessor.AskHeight(length); if (File.Exists($"outputs/quadratic-{length}-{a}-{b}-{c}-H{height}.png")) { Console.WriteLine("File already exists"); return; } }

            if (!File.Exists($"inputs/arrays/quadratic-{length}-{a}-{b}-{c}")) ImageProcessor.SaveArrayToFile(array, $"inputs/arrays/quadratic-{length}-{a}-{b}-{c}");
            ImageProcessor.GenerateImageFromArray(array, $"outputs/quadratic-{length}-{a}-{b}-{c}", height);
        }

        static private void Cubic(bool askForHeight = false)
        {
            int height = ImageProcessor.Square(length);
            uint a = 12;
            uint b = 34;
            uint c = 56;
            uint d = 78;

            if (!askForHeight && File.Exists($"outputs/cubic-{length}-{a}-{b}-{c}-{d}-H{height}.png")) { Console.WriteLine("File already exists"); return; }

            uint[] array = new uint[length];
            for (uint x = 0; x < length; x++)
            {
                array[x] = a * (x * x * x) + b * (x * x) + c * x + d;
            }

            if (askForHeight) { height = ImageProcessor.AskHeight(length); if (File.Exists($"outputs/cubic-{length}-{a}-{b}-{c}-{d}-H{height}.png")) { Console.WriteLine("File already exists"); return; } }

            if (!File.Exists($"inputs/arrays/cubic-{length}-{a}-{b}-{c}-{d}")) ImageProcessor.SaveArrayToFile(array, $"inputs/arrays/cubic-{length}-{a}-{b}-{c}-{d}");
            ImageProcessor.GenerateImageFromArray(array, $"outputs/cubic-{length}-{a}-{b}-{c}-{d}", height);
        }

        static private void Fibonacci(bool askForHeight = false)
        {
            int height = ImageProcessor.Square(length);

            if (!askForHeight && File.Exists($"outputs/fibonacci-{length}-H{height}.png")) { Console.WriteLine("File already exists"); return; }

            uint num1 = 0;
            uint num2 = 1;
            uint[] array = new uint[length];
            for (uint i = 0; i < length; i++)
            {
                array[i] = num2;
                uint temp = num2;
                num2 += num1;
                num1 = temp;
            }

            if (askForHeight) { height = ImageProcessor.AskHeight(length); if (File.Exists($"outputs/fibonacci-{length}-H{height}.png")) { Console.WriteLine("File already exists"); return; } }

            if (!File.Exists($"inputs/arrays/fibonacci-{length}")) ImageProcessor.SaveArrayToFile(array, $"inputs/arrays/fibonacci-{length}");
            ImageProcessor.GenerateImageFromArray(array, $"outputs/fibonacci-{length}", height);
        }
    }
}
