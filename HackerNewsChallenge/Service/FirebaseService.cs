using HackerNewsChallenge.Config;
using HackerNewsChallenge.Dto;
using HackerNewsChallenge.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace HackerNewsChallenge.Service
{
    public class FirebaseService : IFirebaseService
    {
        private readonly ILogger<FirebaseService> _logger;
        private readonly HackerNewsChallengeConfig _config;
        private readonly HttpClient _httpClient;

        public FirebaseService(ILogger<FirebaseService> logger,
                                IOptionsMonitor<HackerNewsChallengeConfig> config)
        {
            _logger = logger;
            _config = config.CurrentValue;
            _httpClient = new HttpClient();
        }

        public async Task<List<FirebaseStoryDto>> GetBestStories()
        {
            var bestStoriesIds = await GetBestStoriesIds();

            var storyTasks = bestStoriesIds.Select(GetStoryById);

            var stories = await Task.WhenAll(storyTasks);

            return stories.ToList();
        }

        private async Task<List<long>> GetBestStoriesIds()
        {
            _logger.LogDebug($"Fetching best story ids ...");

            var response = await _httpClient.GetAsync(_config.FirebaseBestStoriesIdsUrl);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<List<long>>();

            return responseBody!;
        }

        public async Task<FirebaseStoryDto> GetStoryById(long id)
        {
            _logger.LogDebug("Fetching story with id={id} ...", id);

            var url = _config.FirebaseBestStoriesByIdUrl.Replace("{id}", id.ToString());

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<FirebaseStoryDto>();

            return responseBody!;
        }
    }
}
