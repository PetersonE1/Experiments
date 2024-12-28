using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Wintellect.Interop.Sound;
using System.Xml;
using System.IO;
using System.Reflection.Emit;

namespace User32_Testing
{
    public static class Caller
    {
        public static void Run()
        {
            DialogResult result = Message.DisplayMessageBox("Title", "Message", MessageBoxType.Ok, MessageBoxIcon.Error);
            Console.WriteLine(result);
        }

        public static void GenerateEnum()
        {
            Stream stream = File.OpenRead(@"C:\Users\Peter\Source\Repos\Experiments\User32_Testing\KeycodeTable.xml");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;

            List<(string, string, string)> codes = new();

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                string obj = string.Empty;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "tr")
                                obj = "<tr>";
                            else
                                obj += $"<{reader.Name}>";
                            break;
                        case XmlNodeType.EndElement:
                            obj += $"</{reader.Name}>";
                            if (reader.Name == "tr")
                                codes.Add(ParseElement(obj));
                            break;
                        case XmlNodeType.Text:
                            obj += reader.Value;
                            break;
                        default:
                            break;
                    }
                }
            }

            List<(string, string, string)> buffer = new();
            foreach (var code in codes)
            {
                if (code.Item1 == "-")
                    buffer.Add(code);
            }

            foreach (var code in buffer)
            {
                codes.Remove(code);
            }

            string enumCode = string.Empty;
            foreach (var code in codes)
                enumCode += $"{code.Item1} = {code.Item2},\n";
            Console.WriteLine(enumCode);
        }

        private static (string, string, string) ParseElement(string input)
        {
            Console.WriteLine(input);
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream sr = new MemoryStream(byteArray);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;

            using (XmlReader reader = XmlReader.Create(sr, settings))
            {
                string[] parsedElement = new string[3];
                int index = 0;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text:
                            Console.Write(index.ToString() + " ");
                            parsedElement[index] = reader.Value;
                            index++;
                            break;
                        default:
                            break;
                    }
                }
                if (parsedElement[2] == null)
                {
                    parsedElement[2] = parsedElement[1];
                    parsedElement[1] = parsedElement[0];
                    parsedElement[0] = parsedElement[2];
                }
                parsedElement[0] = parsedElement[0].Replace("\n", "");
                parsedElement[0] = parsedElement[0].Remove(0, parsedElement[0]
                    .IndexOf(parsedElement[0].First(c => (int)c != 32)))
                    .Replace(' ', '_');
                while (parsedElement[0].Last() == '_')
                    parsedElement[0] = parsedElement[0].Remove(parsedElement[0].Length - 1);
                return (parsedElement[0], parsedElement[1], parsedElement[2]);
            }
        }
    }
}
