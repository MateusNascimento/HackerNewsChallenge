using HackerNewsChallenge.Service.Interfaces;

namespace HackerNewsChallenge
{
    public class HackerNewsChallengeHostedService : IHostedService
    {
        private readonly ILogger<HackerNewsChallengeHostedService> _logger;
        private readonly IHackerNewsChallengeService _hackerNewsChallengeService;

        public HackerNewsChallengeHostedService(ILogger<HackerNewsChallengeHostedService> logger, 
                                                    IHackerNewsChallengeService hackerNewsChallengeService)
        {
            _logger = logger;
            _hackerNewsChallengeService = hackerNewsChallengeService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("HackerNewsChallengeHostedService is starting ...");

            _hackerNewsChallengeService.UpdateCache();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("HackerNewsChallengeHostedService is stopping ...");

            return Task.CompletedTask;
        }
    }
}
