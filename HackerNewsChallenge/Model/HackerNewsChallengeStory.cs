using HackerNewsChallenge.Dto;

namespace HackerNewsChallenge.Model
{
    public class HackerNewsChallengeStory
    {
        public string Title { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        public string PostedBy { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }

        public override string ToString()
        {
            return $"Title={Title}, Uri={Uri}, PostedBy={PostedBy}, Time={Time}, Score={Score}, CommentCount={CommentCount}";
        }

        public static HackerNewsChallengeStory FromFirebaseStory(FirebaseStoryDto firebaseStory) 
        {
            return new HackerNewsChallengeStory
            {
                Title = firebaseStory.Title,
                Uri = firebaseStory.Url,
                PostedBy = firebaseStory.By,
                Time = DateTimeOffset.FromUnixTimeMilliseconds(firebaseStory.Time).UtcDateTime,
                Score = firebaseStory.Score,
                CommentCount = firebaseStory.Descendants
            };
        }
    }
}
