namespace SpeechTranslator
{
    public class Language
    {
        public string Translate { get; set; }
        public string Speech { get; set; }
        public string Name { get; set; }

        public static readonly Language English = new Language("en", "en-US", "en-US-Wavenet-F");
        public static readonly Language Korean = new Language("ko", "ko-KR", "ko-KR-Wavenet-B");
        public static readonly Language Russian = new Language("ru", "ru-RU", "ru-RU-Wavenet-C");
        public static readonly Language Spanish = new Language("es", "es-ES", "es-ES-Standard-A");
        public static readonly Language French = new Language("fr", "fr-FR", "fr-FR-Wavenet-A");


        public Language(string translate, string speech, string name)
        {
            Translate = translate;
            Speech = speech;
            Name = name;
        }
    }
}