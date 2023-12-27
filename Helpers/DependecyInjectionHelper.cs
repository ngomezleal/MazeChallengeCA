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
            var apiParams = configuration.GetSection("Maze").Get<MazeApiParamsDto>();
            services.AddHttpClient(Constants.HttpClientConfigureName, client =>
            {
                client.BaseAddress = new Uri(apiParams.Url);
            });
            services.Configure<MazeApiParamsDto>(options => configuration.GetSection("Maze").Bind(options));
            services.AddSingleton<IMazeService, MazeService>();
            services.AddScoped<ISolveMazeService, SolveMazeService>();
            services.AddScoped<IMiscellaneous, Miscellaneous>();
        }
    }
}