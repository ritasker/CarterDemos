namespace SampleTests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpContentExtensions
    {
        public static async Task<T> ToModel<T>(this HttpContent content)
        {
            var contentAsStr = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contentAsStr);
        }
    }
}