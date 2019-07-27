using System;
using SpeechRecognitionNet;

namespace Sample
{
    class Program
    {
        private const string address = "localhost";
        private const int port = 8080;

        static void Main(string[] args)
        {
            var client = new SpeechRecognitionClient();
            client.Recognize += (s, e) => { Console.WriteLine(e.Recognized); };
            client.Connect(address, port);
            WaitConsoleReadLine();
        }

        private static void WaitConsoleReadLine()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "exit")
                {
                    return;
                }
            }
        }
    }
}
