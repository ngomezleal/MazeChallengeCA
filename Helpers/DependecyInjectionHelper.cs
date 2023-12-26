using MazeChallengeCA.Dtos;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MazeChallengeCA.Helpers
{
    public static class DependecyInjectionHelper
    {
        public static void ConfigureMazeDependecyInjection(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddLogging();
            services.AddHttpClient();
            services.Configure<MazeApiParamsDto>(options => configuration.GetSection("Maze").Bind(options));
            services.AddSingleton<IMazeService, MazeService>();
            services.AddSingleton<ISolveMazeService, SolveMazeService>();
            services.AddSingleton<IMiscellaneous, Miscellaneous>();
        }
    }
}