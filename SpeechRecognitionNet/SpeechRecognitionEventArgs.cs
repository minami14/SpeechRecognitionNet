using System;

namespace SpeechRecognitionNet
{
    public class SpeechRecognitionEventArgs:EventArgs
    {
        public string Recognized { get; set; }

        public SpeechRecognitionEventArgs(string recognized)
        {
            Recognized = recognized;
        }
    }
}
