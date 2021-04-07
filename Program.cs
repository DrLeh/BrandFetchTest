using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using ClosedXML;
using ClosedXML.Excel;
using System.Diagnostics;
using Svg;
using System.Drawing.Imaging;

namespace BrandFetchTest
{
    class Program
    {
        private const int CompanyNameColumn = 2;
        private const int CompanyDomainColumn = 3;
        //private const int IndustryColumn = 4;
        private const int VibrantColumn = 4;
        private const int DarkColumn = 5;
        private const int LightColumn = 6;
        private const int LogoImageColumn = 7;
        private const int LogoSvgColumn = 8;
        private const int IconImageColumn = 9;
        //private const int DescriptionColumn = 11;
        //private const int KeywordsColumn = 12;
        private const int FacebookColumn = 10;
        private const int YoutubeColumn = 11;
        //private const int CrunchbaseColumn = 15;
        private const int PinterestColumn = 12;
        private const int InstagramColumn = 13;
        private const int LinkedinColumn = 14;
        //private const int MediumColumn = 19;
        //private const int GithubColumn = 20;
        private const int TwitterColumn = 15;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            var apiKey = "l7FD1UGRzn1A4PRGKkGoL3mQbfciS4cb5fb2R8KM";

            var brandUrls = new List<string> { };
            var strs = File.ReadAllLines("Urls.txt");

            var filtered = strs.Where(x => !string.IsNullOrWhiteSpace(x));

            var output = @"c:\users\devon\desktop\brandFetch.xlsx";
            var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Sheet1");

            ws.Cell(1, 1).SetValue("ESP Domain");
            ws.Cell(1, CompanyNameColumn).SetValue("Company Name");
            ws.Cell(1, CompanyDomainColumn).SetValue("Brandfetch Domain");
            //ws.Cell(1, IndustryColumn).SetValue("Industry");
            ws.Cell(1, VibrantColumn).SetValue("Vibrant");
            ws.Cell(1, DarkColumn).SetValue("Dark");
            ws.Cell(1, LightColumn).SetValue("Light");
            ws.Cell(1, LogoImageColumn).SetValue("Logo image");
            ws.Cell(1, LogoSvgColumn).SetValue("Logo SVG");
            ws.Cell(1, IconImageColumn).SetValue("Icon image");
            //ws.Cell(1, DescriptionColumn).SetValue("Description");
            //ws.Cell(1, KeywordsColumn).SetValue("Keywords");
            ws.Cell(1, FacebookColumn).SetValue("Facebook");
            ws.Cell(1, YoutubeColumn).SetValue("Youtube");
            //ws.Cell(1, CrunchbaseColumn).SetValue("Crunchbase");
            ws.Cell(1, PinterestColumn).SetValue("pinterest");
            ws.Cell(1, InstagramColumn).SetValue("instagram");
            ws.Cell(1, LinkedinColumn).SetValue("linkedin");
            //ws.Cell(1, MediumColumn).SetValue("medium");
            //ws.Cell(1, GithubColumn).SetValue("github");
            ws.Cell(1, TwitterColumn).SetValue("twitter");



            ws.Column(LogoImageColumn).Width = 15;
            ws.Column(LogoSvgColumn).Width = 15;
            ws.Column(IconImageColumn).Width = 15;
            ws.Row(1).Style.Font.SetBold();


            var rowCounter = 2;

            foreach (var domain in filtered.Take(350))
            {
                ws.Cell(rowCounter, 1).SetValue(domain);
                ws.Row(rowCounter).Height = 80;

                await AddCompanyInfo(ws, rowCounter, domain);
                await AddColors(ws, rowCounter, domain);
                await AddImages(ws, rowCounter, domain);

                // Adjust row heights
                Console.WriteLine(rowCounter);


                rowCounter++;
            }
            ws.Column(1).AdjustToContents();
            for (int i = 12; i < 30; i++)
                ws.Column(i).AdjustToContents();

            wb.SaveAs(output);
            //Process.Start($@"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE", $@"/r ""{output}""");
        }

        private static async Task AddCompanyInfo(IXLWorksheet ws, int rowCounter, string domain)
        {
            try
            {
                Console.WriteLine($"Getting Company Info for {domain}");
                var companyRoot = await GetCompanyInfo(domain);
                var company = companyRoot.response;
                if (company == null)
                    return;

                ws.Cell(rowCounter, CompanyNameColumn).SetValue(company.name);
                ws.Cell(rowCounter, CompanyDomainColumn).SetValue(company.domain);
                //ws.Cell(rowCounter, DescriptionColumn).SetValue(company.description);
                //ws.Cell(rowCounter, IndustryColumn).SetValue(string.Join(",", company.industry.OrEmptyIfNull().Select(x => x?.label)));
                //ws.Cell(rowCounter, KeywordsColumn).SetValue(company.keywords);
                ws.Cell(rowCounter, FacebookColumn).SetValue(company.facebook?.url);
                ws.Cell(rowCounter, YoutubeColumn).SetValue(company.youtube?.url);
                //ws.Cell(rowCounter, CrunchbaseColumn).SetValue(company.crunchbase?.url);
                ws.Cell(rowCounter, PinterestColumn).SetValue(company.pinterest?.url);
                ws.Cell(rowCounter, InstagramColumn).SetValue(company.instagram?.url);
                ws.Cell(rowCounter, LinkedinColumn).SetValue(company.linkedin?.url);
                //ws.Cell(rowCounter, MediumColumn).SetValue(company.medium?.url);
                //ws.Cell(rowCounter, GithubColumn).SetValue(company.github?.url);
                ws.Cell(rowCounter, TwitterColumn).SetValue(company.twitter?.url);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error getting image for {domain}: {e.Message}");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine($"Error getting image for {domain}: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting color for {domain}: {e.Message}");
            }
        }

        private static async Task AddImages(IXLWorksheet ws, int rowCounter, string domain)
        {
            try
            {
                Console.WriteLine($"Getting Logo for {domain}");
                var logoRoot = await GetLogo(domain);
                var response = logoRoot.response;
                var logo = response.logo;
                var icon = response.icon;
                //var imageUrl = "https://assets.brandfetch.io/5d83ad933bff44f.png";


                if (logo != null)
                {
                    await AddImage(ws, logo.image, rowCounter, LogoImageColumn);
                    await AddImage(ws, logo.svg, rowCounter, LogoSvgColumn);
                }
                if (icon != null)
                {
                    await AddImage(ws, icon.image, rowCounter, IconImageColumn);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error getting image for {domain}: {e.Message}");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine($"Error getting image for {domain}: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting color for {domain}: {e.Message}");
            }
        }

        private static async Task AddColors(IXLWorksheet ws, int rowCounter, string domain)
        {
            try
            {
                Console.WriteLine($"Getting Color for {domain}");
                var colorRoot = await GetColor(domain);
                var colors = colorRoot.response.filtered;
                ws.Cell(rowCounter, VibrantColumn).Style.Fill.BackgroundColor = XLColor.FromHtml(colors.vibrant);
                ws.Cell(rowCounter, DarkColumn).Style.Fill.BackgroundColor = XLColor.FromHtml(colors.dark);
                ws.Cell(rowCounter, LightColumn).Style.Fill.BackgroundColor = XLColor.FromHtml(colors.light);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error getting color for {domain}: {e.Message}");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine($"Error getting color for {domain}: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting color for {domain}: {e.Message}");
            }
        }

        public static async Task<CompanyRoot> GetCompanyInfo(string domain)
        {
            var url = "https://api.brandfetch.io/v1/company";
            var res = await HttpHelper.HttpPostAsync<CompanyRoot>(url, new { domain = domain });
            return res;
        }
        public static async Task<LogoRoot> GetLogo(string domain)
        {
            var url = "https://api.brandfetch.io/v1/logo";
            var res = await HttpHelper.HttpPostAsync<LogoRoot>(url, new { domain = domain });
            return res;
        }
        public static async Task<ColorRoot> GetColor(string domain)
        {
            var url = "https://api.brandfetch.io/v1/color";
            var res = await HttpHelper.HttpPostAsync<ColorRoot>(url, new { domain = domain });
            return res;
        }

        public static async Task AddImage(IXLWorksheet ws, string imageUrl, int row, int col)
        {
            if (imageUrl == null)
                return;
            try
            {
                using var strm = await GetImageStream(imageUrl);

                ws.AddPicture(strm)
                    .MoveTo(ws.Cell(row, col))
                    .WithSize(100, 100)
                    ;
            }
            catch
            {
                Console.WriteLine($"Error finding image: {imageUrl}");
            }
        }

        public static async Task<Stream> GetImageStream(string imageUrl)
        {
            var strm = await HttpHelper.HttpGetStreamAsync(imageUrl);

            if (imageUrl.EndsWith("svg"))
            {
                var svgDocument = SvgDocument.Open<SvgDocument>(strm);
                var bitmap = svgDocument.Draw();

                var bitmapStrm = new MemoryStream();

                bitmap.Save(bitmapStrm, ImageFormat.Png);
                return bitmapStrm;
            }
            return strm;
        }
    }



    internal class HttpHelper
    {
        private static HttpClient _httpClient;
        private static HttpClient HttpClient => _httpClient ?? (_httpClient = CreateClient());

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
        };

        private static HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", "l7FD1UGRzn1A4PRGKkGoL3mQbfciS4cb5fb2R8KM");
            return httpClient;
        }

        internal static void SetUrlBase(string url)
        {
            if (HttpClient.BaseAddress == null)
                HttpClient.BaseAddress = new Uri(url);
        }


        internal static void SetClient(HttpClient client)
        {
            _httpClient = client;
        }

        public static string BuildUrl(params string[] components)
        {
            return components.Select(x => x.Trim('/')).StringJoin("/");
        }

        public static async Task<Stream> HttpGetStreamAsync(string url)
        {
            var res = await HttpClient.GetAsync(url);
            await HandleStatusCode(url, res, "GET");
            return await res.Content.ReadAsStreamAsync();
        }

        public static async Task<T> HttpGetAsync<T>(string url)
        {
            try
            {
                var res = await HttpClient.GetAsync(url);
                await HandleStatusCode(url, res, "GET");
                var result = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result, _jsonSettings);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public static async Task<T> HttpPostAsync<T>(string url, object content)
        {
            var json = JsonConvert.SerializeObject(content);

            var res = await HttpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            await HandleStatusCode(url, res, "POST");
            var result = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result, _jsonSettings);
        }
        private static async Task HandleStatusCode(string url, HttpResponseMessage response, string method)
        {
            if (response.IsSuccessStatusCode)
                return;
            var content = await response.Content?.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                //case System.Net.HttpStatusCode.InternalServerError: throw new InternalServerErrorException(url, response.ReasonPhrase, content);
                case System.Net.HttpStatusCode.NotFound: throw new NotFoundException(url, response.ReasonPhrase, content);
                    //case System.Net.HttpStatusCode.BadRequest: throw new BadRequestException(url, response.ReasonPhrase, content);
                    //case System.Net.HttpStatusCode.BadGateway: throw new BadGatewayException(url, response.ReasonPhrase, content);
                    //case System.Net.HttpStatusCode.MethodNotAllowed: throw new MethodNotAllowedException(url, method, response.ReasonPhrase, content);
                    //case System.Net.HttpStatusCode.RequestTimeout: throw new RequestTimeoutException(url, response.ReasonPhrase, content);
            }
            throw new UnknownException(url, response.ReasonPhrase, content);
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string url, string reason, string content)
            : base($"NotFound. Reason: '{reason}' Content: '{content}' Url: {url}") { }
    }
    public class UnknownException : Exception
    {
        public UnknownException(string url, string reason, string content)
            : base($"Http Exception. Reason: '{reason}' Content: '{content}' Url: {url}") { }
    }

    public static class Extensions
    {
        public static string StringJoin(this IEnumerable<string> s, string separator = ",")
        {
            return string.Join(separator, s);
        }

        public static string StringJoin<T>(this IEnumerable<T> ob, Func<T, string> selector, string separator = ",")
        {
            return string.Join(separator, ob.Select(selector));
        }
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> ob)
        {
            return ob ?? Enumerable.Empty<T>();
        }
    }
}
