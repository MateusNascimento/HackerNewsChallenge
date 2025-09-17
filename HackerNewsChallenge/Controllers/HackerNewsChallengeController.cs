using HackerNewsChallenge.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsChallenge.Controllers
{
    [Route("hacker-news-challenge")]
    public class HackerNewsChallengeController : Controller
    {
        private readonly ILogger<HackerNewsChallengeController> _logger;
        private readonly IHackerNewsChallengeService _hackerNewsChallengeService;

        public HackerNewsChallengeController(ILogger<HackerNewsChallengeController> logger, 
                                                IHackerNewsChallengeService hackerNewsChallengeService)
        {
            _logger = logger;
            _hackerNewsChallengeService = hackerNewsChallengeService;
        }

        [HttpGet]
        public IActionResult GetBestStories([FromQuery] int numberOfStories, [FromQuery] bool invalidateCache)
        {
            try
            {
                var stories = _hackerNewsChallengeService.GetNBestStories(numberOfStories, invalidateCache);

                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred on {className}:{functionName}: [{exception}]",
                                        nameof(HackerNewsChallengeController), nameof(GetBestStories), ex);

                return StatusCode(500, "Internal server error");
            }
        }
    }
}
