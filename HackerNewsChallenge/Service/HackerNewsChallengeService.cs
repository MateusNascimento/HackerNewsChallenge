using HackerNewsChallenge.Config;
using HackerNewsChallenge.Model;
using HackerNewsChallenge.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace HackerNewsChallenge.Service
{
    public class HackerNewsChallengeService : IHackerNewsChallengeService
    {
        private readonly ILogger<HackerNewsChallengeService> _logger;
        private readonly IFirebaseService _firebaseService;
        private readonly HackerNewsChallengeConfig _config;

        private readonly List<HackerNewsChallengeStory> _cachedStoriesSortedByScore = new List<HackerNewsChallengeStory>();
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private TimeOnly _lastCacheRequestTime = TimeOnly.MinValue;

        public HackerNewsChallengeService(ILogger<HackerNewsChallengeService> logger, 
                                            IFirebaseService firebaseService, 
                                            IOptionsMonitor<HackerNewsChallengeConfig> config)
        {
            _logger = logger;
            _firebaseService = firebaseService;
            _config = config.CurrentValue;
        }

        public List<HackerNewsChallengeStory> GetNBestStories(int numberOfStories, bool invalidateCache = false)
        {
            if (invalidateCache || IsCacheExpired())
            {
                UpdateCache();
            }

            return GetCachedNBestStories(numberOfStories);

        }

        private bool IsCacheExpired()
        {
            return _config.CacheExpirationEnabled &&
                   (TimeOnly.FromDateTime(DateTime.Now) - _lastCacheRequestTime).TotalMilliseconds > _config.CacheDurationInMilliseconds;
        }

        public void UpdateCache()
        {
            _logger.LogInformation("Updating cache ...");

            _cacheLock.EnterWriteLock();

            try
            {
                // given that "_cacheLock.EnterWriteLock()" blocks all reader threads 
                // there is no reason to await in here
                var firebaseStories = _firebaseService.GetBestStories().Result;

                _cachedStoriesSortedByScore.Clear();

                var firebaseStoriesSorted = firebaseStories.OrderByDescending(x => x.Score);

                foreach (var firebaseStory in firebaseStoriesSorted)
                {
                    var hackerNewsChallengeStory = HackerNewsChallengeStory.FromFirebaseStory(firebaseStory);
                    _cachedStoriesSortedByScore.Add(hackerNewsChallengeStory);
                }

                _lastCacheRequestTime = TimeOnly.FromDateTime(DateTime.Now);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        private List<HackerNewsChallengeStory> GetCachedNBestStories(int numberOfStories)
        {
            _cacheLock.EnterReadLock();

            try
            {
                if (numberOfStories <= 0)
                {
                    return _cachedStoriesSortedByScore;
                }

                return _cachedStoriesSortedByScore.Take(numberOfStories).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }
    }
}
