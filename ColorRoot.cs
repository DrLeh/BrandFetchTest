using System.Collections.Generic;

namespace BrandFetchTest
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ColorRoot
    {
        public int statusCode { get; set; }
        public ColorResponse response { get; set; }
        public class Filtered
        {
            public string vibrant { get; set; }
            public string dark { get; set; }
            public string light { get; set; }
        }

        public class Suggested
        {
            public string color { get; set; }
        }

        public class ColorResponse
        {
            public Filtered filtered { get; set; }
            public List<Suggested> suggested { get; set; }
        }

    }
}
