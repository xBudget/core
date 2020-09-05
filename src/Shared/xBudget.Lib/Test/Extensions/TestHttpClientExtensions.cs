using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace xBudget.Lib.Test.Extensions
{
    public static class TestHttpClientExtensions
    {
        public static async Task<T> ReadAsObject<T>(this HttpContent httpContent)
        {
            var responseString = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string path, T @object)
        {
            var objectJson = JsonConvert.SerializeObject(@object);
            var jsonContent = new StringContent(objectJson, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(path, jsonContent);
        }
    }
}
