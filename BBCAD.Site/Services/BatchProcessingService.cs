using System.Web;
using System.Text.Json;
using Microsoft.Extensions.Options;

using BBCAD.API.DTO;
using BBCAD.Site.Settings;

namespace BBCAD.Site.Services
{
    public class BatchProcessingService : IBatchProcessingService
    {
        // private readonly ILogger _logger;   
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<BoardAPIOptions> _boardAPIOptions;

        public BatchProcessingService(IHttpClientFactory clientFactory, IOptions<BoardAPIOptions> boardAPIOptions)
        {
            _clientFactory = clientFactory;
            _boardAPIOptions = boardAPIOptions;
        }

        public async Task<BatchProcessingResponce> CallBoardAPI(HttpMethod method, string requestStr, string? script = null)
        {
            string scriptTail = script == null ? string.Empty : $"?script={HttpUtility.UrlEncode(script)}";
            string requestUrl = $"{_boardAPIOptions.Value.Connection}/{requestStr}{scriptTail}";

            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(method, requestUrl);

            var response = await client.SendAsync(request) ??
                throw new Exception("Empty responce");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}: {response.ReasonPhrase}");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();

            string? rawContent = await response.Content.ReadAsStringAsync() ??
                throw new Exception("The responce content is empty");

            BatchProcessingResponce? responce =
                JsonSerializer.Deserialize<BatchProcessingResponce>(rawContent) ??
                throw new Exception("Can't deserialize responce");

            if (!string.IsNullOrWhiteSpace(responce?.Error))
            {
                throw new Exception(responce?.Error);
            }

            return responce;
        }
    }
}
