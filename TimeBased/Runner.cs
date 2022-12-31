using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace TimeBased
{
    internal static class Runner
    {
        private static void Main()
        {
            DateTime timeToCheck = DateTime.Parse(Console.ReadLine());
            string s = ChristmasEve(DateOnly.FromDateTime(timeToCheck));
            Console.WriteLine(s);
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(s);
        }

        private static string ChristmasEve(DateOnly date)
        {
            DateOnly christmas = new DateOnly(date.Year, 12, 25);
            int days = christmas.DayOfYear - date.DayOfYear;
            Console.WriteLine($"Difference pre-adjustment: {days}");
            if (days < 0)
                days += 365;
            Console.WriteLine($"Difference post-adjustment: {days}");

            string output = "Christmas";
            for (int i = 0; i < days; i++)
                output += " Eve";

            return output;
        }
    }
}