using HackerNewsChallenge.Dto;

namespace HackerNewsChallenge.Service.Interfaces
{
    public interface IFirebaseService
    {
        Task<List<FirebaseStoryDto>> GetBestStories();
    }
}
