using MazeChallengeCA.Dtos;
using MazeChallengeCA.Helpers;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Models;
using Microsoft.Extensions.Options;

namespace MazeChallengeCA.Services
{
    public class SolveMazeService : ISolveMazeService
    {
        private readonly IMazeService mazeService;
        private readonly IOptions<MazeApiParamsDto> config;
        private readonly IMiscellaneous miscellaneous;
        Game objGame = new Game();
        List<CurrentPositionMazeBlockViewDto> listCurrentPosition;
        char[,] virtualMaze;
        public SolveMazeService(IOptions<MazeApiParamsDto> options, IMiscellaneous miscellaneous, IMazeService mazeService)
        {
            this.mazeService = mazeService;
            this.config = options;
            this.miscellaneous = miscellaneous;
            virtualMaze = new char[Constants.Height, Constants.Width];
            listCurrentPosition = new List<CurrentPositionMazeBlockViewDto>();
            virtualMaze = miscellaneous.InitializeVirtualMaze(virtualMaze);
        }

        public async Task<bool> SolveMaze(GameAnswerDto game, string latitude = "", int y = 0, int x = 0)
        {
            var currentPosition = new GameCurrentPositionAnswerDto();
            objGame = new Game()
            {
                Operation = latitude,
                Url = $"{config.Value.Url}/Game/{game.MazeUid}/{game.GameUid}?code={config.Value.Code}"
            };

            if (!string.IsNullOrEmpty(latitude))
            {
                currentPosition = await mazeService.MoveLatitude(objGame);
                if (currentPosition.MazeBlockView is not null)
                {
                    currentPosition = miscellaneous.ReverseLatitude(currentPosition, latitude);
                    var indexPositionByLatitude = listCurrentPosition.FindIndex(coord => coord.CoordY == y && coord.CoordX == x && coord.Latitude == latitude);
                    if (indexPositionByLatitude == -1)
                        listCurrentPosition.Add(currentPosition.MazeBlockView);
                    virtualMaze = miscellaneous.RecalculatePositions(currentPosition, virtualMaze);
                    virtualMaze = miscellaneous.Print(virtualMaze);
                }
            }

            if ((y >= 0 && y < virtualMaze.GetLength(0)) && (x >= 0 && x < virtualMaze.GetLength(1)))
            {
                if (currentPosition.MazeBlockView is not null)
                {
                    if ((currentPosition.MazeBlockView.CoordX == currentPosition.MazeBlockView.CoordY) &&
                    (currentPosition.MazeBlockView.CoordX == (Constants.Height - 1) && currentPosition.MazeBlockView.CoordY == (Constants.Width - 1)))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Congratulations, you won!");
                        return true;
                    }
                }
            }
            else
                return false;

            if ((y >= 0 && y < virtualMaze.GetLength(0)) && (x >= 0 && x < virtualMaze.GetLength(1)))
            {
                //If it arrive to the wall or at the same point, It continue to search some solutions.
                //Si llegamos a una pared o al mismo punto, no se puede resolver.
                if (virtualMaze[y, x] == '#' || virtualMaze[y, x] == '*')
                {
                    if (currentPosition.MazeBlockView is not null)
                    {
                        var indexPositionByLatitude = listCurrentPosition.FindIndex(coord => coord.CoordY == y && coord.CoordX == x && coord.Latitude == latitude);
                        try
                        {
                            objGame.Operation = listCurrentPosition[indexPositionByLatitude].ReverseLatitude;
                            currentPosition = await mazeService.MoveLatitude(objGame);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    return false;
                }
            }
            else
                return false;

            /*Intermadiate Case 
             * It start to go foreach latitude*/
            if ((y >= 0 && y < virtualMaze.GetLength(0)) && (x >= 0 && x < virtualMaze.GetLength(1)))
                virtualMaze[y, x] = '*'; //Line or cell visited!
            else
                return false;

            bool path = false;
            virtualMaze = miscellaneous.Print(virtualMaze);

            /*Recursives calls (Algorithm recursive applied)
             * //Due to it didn't find the exit in the four movements, it comes back
             */
            path = await SolveMaze(game, Constants.GoEast, y, (x + 1)); //Go to GoEast
            if (path)
                return true;

            path = await SolveMaze(game, Constants.GoNorth, (y - 1), x); //Go to GoNorth
            if (path)
                return true;

            path = await SolveMaze(game, Constants.GoWest, y, (x - 1)); //Go to GoWest
            if (path)
                return true;

            path = await SolveMaze(game, Constants.GoSouth, (y + 1), x); //Go to GoSouth
            if (path)
                return true;

            //Otherwise, if it isn't the result expected, then the current position(path) is unchecked.
            virtualMaze[y, x] = ' ';
            var indexPositionByCoordnates = listCurrentPosition.FindIndex(coord => coord.CoordY == y && coord.CoordX == x);
            objGame.Operation = listCurrentPosition[indexPositionByCoordnates].ReverseLatitude;
            currentPosition = await mazeService.MoveLatitude(objGame);
            virtualMaze = miscellaneous.RecalculatePositions(currentPosition, virtualMaze);
            virtualMaze = miscellaneous.Print(virtualMaze);
            return false;
        }
    }
}