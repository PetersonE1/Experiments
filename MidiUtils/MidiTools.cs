using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M;

namespace MidiUtils
{
    public static class MidiTools
    {
        private static readonly BitArray EmptyBits = new BitArray(Array.Empty<bool>());
        private static readonly BitArray SpaceBits = new BitArray(new bool[] { true, true, true, true, true, true, true, true });

        private static readonly Dictionary<char, BitArray> charToBits = new Dictionary<char, BitArray>()
        {
            { 'a', new BitArray(new bool[] {false, true}) },
            { 'b', new BitArray(new bool[] {true, false, false, false}) },
            { 'c', new BitArray(new bool[] {true, false, true, false}) },
            { 'd', new BitArray(new bool[] {true, false, false}) },
            { 'e', new BitArray(new bool[] {false}) },
            { 'f', new BitArray(new bool[] {false, false, true, false}) },
            { 'g', new BitArray(new bool[] {true, true, false}) },
            { 'h', new BitArray(new bool[] {false, false, false, false}) },
            { 'i', new BitArray(new bool[] {false, false}) },
            { 'j', new BitArray(new bool[] {false, true, true, true}) },
            { 'k', new BitArray(new bool[] {true, false, true}) },
            { 'l', new BitArray(new bool[] {false, true, false, false}) },
            { 'm', new BitArray(new bool[] {true, true}) },
            { 'n', new BitArray(new bool[] {true, false}) },
            { 'o', new BitArray(new bool[] {true, true, true}) },
            { 'p', new BitArray(new bool[] {false, true, true, false}) },
            { 'q', new BitArray(new bool[] {true, true, false, true}) },
            { 'r', new BitArray(new bool[] {false, true, false}) },
            { 's', new BitArray(new bool[] {false, false, false}) },
            { 't', new BitArray(new bool[] {true}) },
            { 'u', new BitArray(new bool[] {false, false, true}) },
            { 'v', new BitArray(new bool[] {false, false, false, true}) },
            { 'w', new BitArray(new bool[] {false, true, true}) },
            { 'x', new BitArray(new bool[] {true, false, false, true}) },
            { 'y', new BitArray(new bool[] {true, false, true, true}) },
            { 'z', new BitArray(new bool[] {true, true, false, false}) },
            { '1', new BitArray(new bool[] {false, true, true, true, true}) },
            { '2', new BitArray(new bool[] {false, false, true, true, true}) },
            { '3', new BitArray(new bool[] {false, false, false, true, true}) },
            { '4', new BitArray(new bool[] {false, false, false, false, true}) },
            { '5', new BitArray(new bool[] {false, false, false, false, false}) },
            { '6', new BitArray(new bool[] {true, false, false, false, false}) },
            { '7', new BitArray(new bool[] {true, true, false, false, false}) },
            { '8', new BitArray(new bool[] {true, true, true, false, false}) },
            { '9', new BitArray(new bool[] {true, true, true, true, false}) },
            { '0', new BitArray(new bool[] {true, true, true, true, true}) },
            { '.', new BitArray(new bool[] {false, true, false, true, false, true}) },
            { ',', new BitArray(new bool[] {true, true, false, false, true, true}) },
            { '?', new BitArray(new bool[] {false, false, true, true, false, false}) },
            { '!', new BitArray(new bool[] {true, false, true, false, true, true}) },
            { '\'', new BitArray(new bool[] {false, true, true, true, true, false}) },
            { '"', new BitArray(new bool[] {false, true, false, false, true, false}) },
            { '(', new BitArray(new bool[] {true, false, true, true, false}) },
            { ')', new BitArray(new bool[] {true, false, true, true, false, true}) },
            { '&', new BitArray(new bool[] {false, true, false, false, false}) },
            { ':', new BitArray(new bool[] {true, true, true, false, false, false}) },
            { ';', new BitArray(new bool[] {true, false, true, false, true, false}) },
            { '/', new BitArray(new bool[] {true, false, false, true, false}) },
            { '_', new BitArray(new bool[] {false, false, true, true, false, true}) },
            { '=', new BitArray(new bool[] {true, false, false, false, true}) },
            { '+', new BitArray(new bool[] {false, true, false, true, false}) },
            { '-', new BitArray(new bool[] {true, false, false, false, false, true}) },
            { '$', new BitArray(new bool[] {false, false, false, true, false, false, true}) },
            { '@', new BitArray(new bool[] {false, true, true, false, true, false}) },
            { ' ', SpaceBits}
        };

        public static BitArray CharToMorse(char c)
        {
            if (charToBits.ContainsKey(c))
                return charToBits[c];
            Console.WriteLine($"No morse code for character '{c}', skipping...");
            return EmptyBits;
        }

        public static BitArray[] StringToMorse(string s)
        {
            s = s.ToLower();
            List<BitArray> temp = new List<BitArray>();
            foreach (char c in s)
            {
                BitArray bits = CharToMorse(c);
                if (bits != EmptyBits)
                    temp.Add(bits);
            }
            return temp.ToArray();
        }

        public static void StringToMorseMidi(string s, int unitLength)
        {
            if (unitLength < 0) unitLength *= -1;
            BitArray[] bits = StringToMorse(s);
            using (MidiOutputDevice output = MidiDevice.Outputs[0])
            {
                MidiMessageNoteOn m_on = new MidiMessageNoteOn(72, 127, 1);
                MidiMessageNoteOff m_off = new MidiMessageNoteOff(72, 127, 1);
                output.Open();
                
                for (int i = 0; i < bits.Length; i++)
                {
                    BitArray bit = bits[i];
                    if (bit == SpaceBits)
                    {
                        Thread.Sleep((int)MorseTiming.WordGap * unitLength);
                        continue;
                    }
                    for (int j = 0; j < bit.Count; j++)
                    {
                        bool longer = bit.Get(j);
                        output.Send(m_on);
                        Thread.Sleep((longer ? (int)MorseTiming.Dah : (int)MorseTiming.Dit) * unitLength);
                        output.Send(m_off);
                        if (j != bit.Count - 1)
                            Thread.Sleep((int)MorseTiming.DitGap * unitLength);
                    }
                    if (i != bits.Length - 1 && bits[i+1] != SpaceBits)
                        Thread.Sleep((int)MorseTiming.CharGap * unitLength);
                }

                Thread.Sleep(250);
                output.Close();
            }
        }
    }

    public enum MorseTiming
    {
        Dit = 1,
        Dah = 3,
        DitGap = 1,
        CharGap = 3,
        WordGap = 7
    }
}
