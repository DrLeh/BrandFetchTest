namespace BrandFetchTest
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class LogoRoot
    {
        public int statusCode { get; set; }
        public LogoResponse response { get; set; }

        public class Logo
        {
            public bool safe { get; set; }
            public string image { get; set; }
            public string svg { get; set; }
        }

        public class Icon
        {
            public string image { get; set; }
            public object svg { get; set; }
        }

        public class LogoResponse
        {
            public Logo logo { get; set; }
            public Icon icon { get; set; }
        }

    }
}
