#region Namespaces
using MazeChallengeCA.Dtos;
using MazeChallengeCA.Helpers;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
#endregion

//The “configuration” variable is created to get our settings.
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

/*Instantiate a variable ‘services’ with a ‘ServiceCollection’ instance.
 * It’s used to retrieve our services via Dependency Injection.*/
var services = new ServiceCollection();

//An extension method created to configure depencency injection.
services.ConfigureMazeDependecyInjection(configuration);
using var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILoggerFactory>();
var buildMaze = serviceProvider.GetService<IMazeService>();
var solveMaze = serviceProvider.GetService<ISolveMazeService>();
var miscellaneous = serviceProvider.GetService<IMiscellaneous>();
var apiParams = configuration.GetSection("Maze").Get<MazeApiParamsDto>();

//Important parameters to create new Maze
Maze objMaze = new Maze()
{
    Width = Constants.Width,
    Height = Constants.Height,
    Url = $"{apiParams.Url}/Maze?code={apiParams.Code}"
};

//Step 1: "Create a New Random Maze"
var newMaze = await buildMaze.CreateNewRandomMaze(objMaze);

//Step 2; "Start the game"
Game objGame = new Game()
{
    Operation = Constants.Operation,
    Url = $"{apiParams.Url}/Game/{newMaze.MazeUid}?code={apiParams.Code}"
};
var game = await buildMaze.CreateGameWithNewMaze(objGame);

//Step 3 "Start to solve"
char[,] virtualMaze = new char[objMaze.Height, objMaze.Width];
virtualMaze = miscellaneous.InitializeVirtualMaze(virtualMaze);

//Call method to solve maze
await solveMaze.SolveMaze(game);
Console.WriteLine("Finished");