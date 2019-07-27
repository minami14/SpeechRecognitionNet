using SpeechRecognitionNet;
using System;
using Google.Cloud.TextToSpeech.V1;
using Google.Cloud.Translation.V2;
using NAudio.Wave;
using System.IO;
using System.Text;
using System.Threading;

namespace SpeechTranslator
{
    class Program
    {
        private const string address = "localhost";
        private const int port = 8080;
        
        private static readonly TranslationClient _translation = TranslationClient.Create();
        private static readonly TextToSpeechClient _tts = TextToSpeechClient.Create();
        private static Language _language = Language.English;
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var recognition = new SpeechRecognitionClient();
            recognition.Recognize += Recognized;
            recognition.Connect(address, port);
            WaitConsoleReadLine();
        }

        private static void Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            switch (e.Recognized)
            {
                case "英語":
                    _language = Language.English;
                    return;
                case "韓国語":
                    _language = Language.Korean;
                    return;
                case "スペイン語":
                    _language = Language.Spanish;
                    return;
                case "フランス語":
                    _language = Language.French;
                    return;
                case "ロシア語":
                    _language = Language.Russian;
                    return;
            }

            Console.WriteLine(e.Recognized);
            var translated = Translate(e.Recognized, _language);
            Console.WriteLine(translated);
            var mp3 = TextToSpeech(translated, _language);
            Play(mp3);
        }

        private static string Translate(string text, Language lang)
        {
            return _translation.TranslateText(text, lang.Translate).TranslatedText;
        }

        private static byte[] TextToSpeech(string text, Language lang)
        {
            var input = new SynthesisInput
            {
                Text = text
            };

            var voice = new VoiceSelectionParams
            {
                LanguageCode = lang.Speech,
                Name = lang.Name
            };

            var config = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            return _tts.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = input,
                Voice = voice,
                AudioConfig = config
            }).AudioContent.ToByteArray();
        }

        private static void Play(byte[] mp3)
        {
            var reader = new Mp3FileReader(new MemoryStream(mp3));
            var waveOut = new WaveOut();
            waveOut.Init(reader);
            waveOut.Play();
            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(1000);
            }
        }

        private static void WaitConsoleReadLine()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input.Length < 1)
                {
                    continue;
                }

                var args = input.Split(' ');
                switch (args[0])
                {
                    case "exit":
                        return;
                    case "lang":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("no language");
                            continue;
                        }
                        SetLanguage(args[1]);
                        break;
                }
            }
        }

        private static void SetLanguage(string lang)
        {
            switch (lang)
            {
                case "en":
                    _language = Language.English;
                    break;
                case "ko":
                    _language = Language.Korean;
                    break;
                case "ru":
                    _language = Language.Russian;
                    break;
                case "es":
                    _language = Language.Spanish;
                    break;
                case "fr":
                    _language = Language.French;
                    break;
                default:
                    Console.WriteLine("invalid language code");
                    break;
            }
        }
    }
}
