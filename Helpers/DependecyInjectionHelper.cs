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
            var apiParams = configuration.GetSection("GlobalConfiguration").Get<GlobalConfigurationDto>();
            services.AddHttpClient(apiParams.Maze.HttpClientConfigureName, client =>
            {
                client.BaseAddress = new Uri(apiParams.Maze.MainUri);
            });
            services.Configure<GlobalConfigurationDto>(options => configuration.GetSection("GlobalConfiguration").Bind(options));
            services.AddSingleton<IMazeService, MazeService>();
            services.AddSingleton<ISolveMazeService, SolveMazeService>();
            services.AddSingleton<IVirtualMazeService, VirtualMazeService>();
        }
    }
}