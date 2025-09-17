namespace HackerNewsChallenge.Dto
{
    public class FirebaseStoryDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string By { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Descendants { get; set; }
        public long Time { get; set; }

        public override string ToString()
        {
            return $"Id={Id}, Title={Title}, By={By}, Score={Score}, Url={Url}, Descendants={Descendants}, Time={Time}";
        }

    }
}
