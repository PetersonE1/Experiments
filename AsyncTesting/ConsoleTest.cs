using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTesting
{
    public class ConsoleTest
    {
        List<List<char>> chars = new List<List<char>>();

        public async Task WriteToConsole()
        {
            Console.Clear();
            List<List<char>> toDelete = new List<List<char>>();
            foreach (List<char> item in chars)
            {
                if (item.LastOrDefault() == 13)
                {
                    Console.WriteLine($"Message: {string.Join("", item)}");
                    toDelete.Add(item);
                }
            }
            foreach (var item in toDelete)
                chars.Remove(item);
            toDelete.Clear();

            if (chars.Count > 0)
                Console.WriteLine(string.Join("", chars[0]));

            await Task.Delay(100);
        }

        public async Task ReadConsole()
        {
            if (chars.Count == 0)
                chars.Add(new List<char>());
            char c = await Task.Run(() => Console.ReadKey(true).KeyChar);
            chars.Last().Add(c);
            if (c == 13)
                chars.Add(new List<char>());
        }
    }
}
