namespace HackerNewsChallenge.Config
{
    public class HackerNewsChallengeConfig
    {
        public string FirebaseBestStoriesIdsUrl { get; set; } = string.Empty;

        public string FirebaseBestStoriesByIdUrl { get; set; } = string.Empty;

        public bool CacheExpirationEnabled { get; set; }

        public int CacheDurationInMilliseconds { get; set; }

    }
}
