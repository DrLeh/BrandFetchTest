using System;
using System.Collections.Generic;
using System.Text;

namespace BrandFetchTest
{
    class CompanyRoot
    {
        public int statusCode { get; set; }
        public CompanyResponse response { get; set; }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Youtube
        {
            public object url { get; set; }
        }

        public class Github
        {
            public object url { get; set; }
        }

        public class Facebook
        {
            public string url { get; set; }
        }

        public class Industry
        {
            public double score { get; set; }
            public string label { get; set; }
        }

        public class Pinterest
        {
            public string url { get; set; }
        }

        public class Instagram
        {
            public string url { get; set; }
        }

        public class Linkedin
        {
            public string url { get; set; }
        }

        public class Medium
        {
            public string url { get; set; }
        }

        public class Crunchbase
        {
            public string url { get; set; }
        }

        public class Twitter
        {
            public string url { get; set; }
        }

        public class CompanyResponse
        {
            public Youtube youtube { get; set; }
            public Github github { get; set; }
            public string keywords { get; set; }
            public Facebook facebook { get; set; }
            public string description { get; set; }
            public string name { get; set; }
            public string banner { get; set; }
            public object language { get; set; }
            public List<Industry> industry { get; set; }
            public Pinterest pinterest { get; set; }
            public Instagram instagram { get; set; }
            public Linkedin linkedin { get; set; }
            public Medium medium { get; set; }
            public Crunchbase crunchbase { get; set; }
            public Twitter twitter { get; set; }
            public string domain { get; set; }
        }



    }
}
