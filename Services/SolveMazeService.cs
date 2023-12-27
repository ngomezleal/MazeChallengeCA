using MazeChallengeCA.Dtos;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Models;
using Microsoft.Extensions.Options;

namespace MazeChallengeCA.Services
{
    public class SolveMazeService : ISolveMazeService
    {
        private readonly IMazeService mazeService;
        private readonly IOptions<GlobalConfigurationDto> config;
        private readonly IVirtualMazeService virtualMazeService;
        List<CurrentPositionMazeBlockViewDto> listCurrentPosition;
        CurrentPositionLatitudeAnswerDto currentPositionLatitude;
        Game objGame = new Game();
        char[,] virtualMaze;
        public SolveMazeService(IOptions<GlobalConfigurationDto> options, IVirtualMazeService virtualMazeService, IMazeService mazeService)
        {
            this.mazeService = mazeService;
            this.config = options;
            this.virtualMazeService = virtualMazeService;
            virtualMaze = new char[config.Value.MazeSize.Height, config.Value.MazeSize.Width];
            listCurrentPosition = new List<CurrentPositionMazeBlockViewDto>();
            currentPositionLatitude = new CurrentPositionLatitudeAnswerDto();
            virtualMaze = virtualMazeService.InitializeVirtualMaze(virtualMaze);
        }

        public async Task<bool> SolveMaze(GameAnswerDto game, string latitude = "", int y = 0, int x = 0)
        {
            var currentPosition = new GameCurrentPositionAnswerDto();
            objGame = new Game()
            {
                Operation = latitude,
                Uri = $"{config.Value.Maze.GameApiUri}{game.MazeUid}/{game.GameUid}?code={config.Value.Maze.Code}"
            };

            if (!string.IsNullOrEmpty(latitude))
            {
                /* Doing movement to another latitud.
                 * If this one isn't null, then, save the movements, recalculate the obstacles and continue.*/
                currentPosition = await mazeService.MoveLatitude(objGame);
                if (currentPosition.MazeBlockView is not null)
                {
                    currentPosition = virtualMazeService.ReverseLatitude(currentPosition, latitude);
                    var indexPositionByLatitude = listCurrentPosition.FindIndex(coord => coord.CoordY == y && coord.CoordX == x && coord.Latitude == latitude);
                    if (indexPositionByLatitude == -1)
                        listCurrentPosition.Add(currentPosition.MazeBlockView);
                    virtualMaze = virtualMazeService.RecalculatePositions(currentPosition, virtualMaze);
                    virtualMaze = virtualMazeService.Print(virtualMaze);
                }
                else
                {
                    /* Foreach movement, the coordinates can change. Sometimes the service can return blocked values, either blocked coordinates or another situation. 
                     * It's necessary restore the positions and coordinates */
                    currentPositionLatitude = virtualMazeService.ReversePositionsAndLatitude(y, x, latitude);
                    y = currentPositionLatitude.PositionY;
                    x = currentPositionLatitude.PositionX;
                    latitude = currentPositionLatitude.Latitude;
                    return false;
                }
            }

            if(!virtualMazeService.PositionsOutOfRange(y, x, virtualMaze))
            {
                if (currentPosition.MazeBlockView is not null)
                {
                    //Checking if the game has reached the end of the maze.
                    if ((currentPosition.MazeBlockView.CoordX == currentPosition.MazeBlockView.CoordY) &&
                    (currentPosition.MazeBlockView.CoordX == (config.Value.MazeSize.Height - 1) && currentPosition.MazeBlockView.CoordY == (config.Value.MazeSize.Width - 1)))
                    {
                        //The end of the maze is always where the coord x and y are equal to the width-1 and height-1 of the created maze.
                        virtualMaze[currentPosition.MazeBlockView.CoordX, currentPosition.MazeBlockView.CoordY] = config.Value.Path.Goal;
                        virtualMaze = virtualMazeService.Print(virtualMaze);
                        Console.WriteLine();
                        Console.WriteLine("Congratulations, you won!");
                        return true;
                    }
                }
            }
            else
                return false;

            if (!virtualMazeService.PositionsOutOfRange(y, x, virtualMaze))
            {
                /*If it arrive to the wall or at the same point, It continue to search some solutions.
                 * It mean, it's necessary return to the old position */
                if (virtualMaze[y, x] == config.Value.Path.Wall || virtualMaze[y, x] == config.Value.Path.Route)
                {
                    if (currentPosition.MazeBlockView is not null)
                    {
                        var indexPositionByLatitude = listCurrentPosition.FindIndex(coord => coord.CoordY == y && coord.CoordX == x && coord.Latitude == latitude);
                        objGame.Operation = listCurrentPosition[indexPositionByLatitude].ReverseLatitude;
                        currentPosition = await mazeService.MoveLatitude(objGame);
                    }
                    return false;
                }
            }
            else
                return false;

            /*Intermadiate Case 
             * It start to go foreach latitude*/
            if (!virtualMazeService.PositionsOutOfRange(y, x, virtualMaze))
                virtualMaze[y, x] = config.Value.Path.Route; //Line or cell visited!
            else
                return false;

            bool path = false;
            virtualMaze = virtualMazeService.Print(virtualMaze);

            /*Recursives calls (Algorithm recursive applied)
             * //Due to it didn't find the exit in the four movements, it comes back */
            path = await SolveMaze(game, config.Value.Latitudes.GoEast, y, (x + 1)); //Go to GoEast
            if (path)
                return true;

            path = await SolveMaze(game, config.Value.Latitudes.GoNorth, (y - 1), x); //Go to GoNorth
            if (path)
                return true;

            path = await SolveMaze(game, config.Value.Latitudes.GoWest, y, (x - 1)); //Go to GoWest
            if (path)
                return true;

            path = await SolveMaze(game, config.Value.Latitudes.GoSouth, (y + 1), x); //Go to GoSouth
            if (path)
                return true;

            //Otherwise, if isn't the result expected, then the current position(path) is unchecked.
            virtualMaze[y, x] = ' ';
            var indexPositionByCoordnates = listCurrentPosition.FindIndex(coord => coord.CoordY == y && coord.CoordX == x);
            objGame.Operation = listCurrentPosition[indexPositionByCoordnates].ReverseLatitude;
            currentPosition = await mazeService.MoveLatitude(objGame);
            virtualMaze = virtualMazeService.RecalculatePositions(currentPosition, virtualMaze);
            virtualMaze = virtualMazeService.Print(virtualMaze);
            return false;
        }
    }
}