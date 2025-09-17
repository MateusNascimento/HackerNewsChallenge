using HackerNewsChallenge.Config;
using HackerNewsChallenge.Service;
using HackerNewsChallenge.Service.Interfaces;

namespace HackerNewsChallenge 
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile($"Config/hacker-news-challenge.json", optional: false, reloadOnChange: false);
            builder.Services.Configure<HackerNewsChallengeConfig>(builder.Configuration.GetSection("HackerNewsChallenge"));

            builder.Services.AddSingleton<IFirebaseService, FirebaseService>();
            builder.Services.AddSingleton<IHackerNewsChallengeService, HackerNewsChallengeService>();
            builder.Services.AddControllers();

            // only used to initialize the cache at startup
            builder.Services.AddHostedService<HackerNewsChallengeHostedService>();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
