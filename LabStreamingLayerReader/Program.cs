using EmotionalCities.Lsl;

namespace LabStreamingLayerReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //StreamInlet inlet = new StreamInlet() { ChannelCount = 4, ChannelFormat = ChannelFormat.Float32, ContentType = "EEG", /*Name = "MuseS-97A4"*/ };
            
            ReceiveData.Run(args);
        }
    }
}
