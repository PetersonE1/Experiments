using System.Speech.Synthesis;

namespace SpeechSynthesis
{
    internal class Runner
    {
        static void Main(string[] args)
        {
            //Run().Wait();
            //VoiceSamples();
            SpeakInOneMinute();
        }

        static void SpeakInOneMinute()
        {
            DateTime targetTime = DateTime.Now.AddMinutes(1);
            while (DateTime.Now < targetTime) { }
            using (SpeechSynthesizer synth = new())
            {
                synth.Speak($"It is currently {DateTime.Now.ToShortTimeString()} on {DateTime.Now.ToLongDateString()}");
            }
        }

        static void VoiceSamples()
        {
            using (SpeechSynthesizer synth = new())
            {
                foreach (var voice in synth.GetInstalledVoices())
                {
                    string name = voice.VoiceInfo.Name;
                    Console.WriteLine(name);
                    synth.SelectVoice(name);
                    synth.Speak(name);
                }
            }
        }

        static async Task Run()
        {
            //string message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            string message = "Test message, please ignore.";

            using (SpeechSynthesizer synthesizer = new())
            {
                synthesizer.SpeakAsync(message);
                Console.WriteLine("Time to count until the message is over.");
                int counter = 0;
                while (synthesizer.State == SynthesizerState.Speaking)
                {
                    Console.WriteLine(++counter);
                    await Task.Delay(1000);
                }
            }
        }
    }
}
