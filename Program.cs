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
var virtualMazeService = serviceProvider.GetService<IVirtualMazeService>();
var globalConfiguration = configuration.GetSection("GlobalConfiguration").Get<GlobalConfigurationDto>();

//Important parameters to create new Maze
Maze objMaze = new Maze()
{
    Width = globalConfiguration.MazeSize.Width,
    Height = globalConfiguration.MazeSize.Height,
    Url = $"{globalConfiguration.Maze.NewMazeApiUri}?code={globalConfiguration.Maze.Code}"
};

//Step 1: "Create a New Random Maze"
var newMaze = await buildMaze.CreateNewRandomMaze(objMaze);
Console.WriteLine("Maze created!");

//Step 2; "Start the game"
Game objGame = new Game()
{
    Operation = globalConfiguration.Maze.Operation,
    Uri = $"{globalConfiguration.Maze.GameApiUri}{newMaze.MazeUid}?code={globalConfiguration.Maze.Code}"
};
Console.WriteLine("Creating game...");
var game = await buildMaze.CreateGameWithNewMaze(objGame);

//Step 3 "Start to solve"
char[,] virtualMaze = new char[objMaze.Height, objMaze.Width];
virtualMaze = virtualMazeService.InitializeVirtualMaze(virtualMaze);

//Call method to solve maze
await solveMaze.SolveMaze(game);
Console.WriteLine("Finished");
Console.ReadKey();