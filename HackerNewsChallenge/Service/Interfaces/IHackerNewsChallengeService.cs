using HackerNewsChallenge.Model;

namespace HackerNewsChallenge.Service.Interfaces
{
    public interface IHackerNewsChallengeService
    {
        List<HackerNewsChallengeStory> GetNBestStories(int numberOfStories, bool invalidateCache = false);

        void UpdateCache();
    }
}
