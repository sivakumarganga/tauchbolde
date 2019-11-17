using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tauchbolde.Mobile.Application.Services.Communication
{
    public class RestService : IRestService
    {
        readonly HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue ("application/json"));
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }
    }
}
