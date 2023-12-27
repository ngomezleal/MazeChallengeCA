using MazeChallengeCA.Dtos;
using MazeChallengeCA.Interfaces;
using Microsoft.Extensions.Options;

namespace MazeChallengeCA.Services
{
    public class VirtualMazeService : IVirtualMazeService
    {
        private readonly IOptions<GlobalConfigurationDto> config;
        public VirtualMazeService(IOptions<GlobalConfigurationDto> options)
        {
            this.config = options;
        }
        public char[,] InitializeVirtualMaze(char[,] virtualMaze)
        {
            for (int i = 0; i < virtualMaze.GetLength(0); i++)
                for (int j = 0; j < virtualMaze.GetLength(1); j++)
                    virtualMaze[i, j] = ' ';
            return virtualMaze;
        }

        public char[,] RecalculatePositions(GameCurrentPositionAnswerDto objGameCurrentPositionAnswer, char[,] virtualMaze)
        {
            var y = objGameCurrentPositionAnswer.MazeBlockView.CoordY;
            var x = objGameCurrentPositionAnswer.MazeBlockView.CoordX;

            //EastBlocked
            var vEastY = y;
            var vEastX = x + 1;

            //NorthBlocked
            var vNorthY = y - 1;
            var vNorthX = x;

            //WestBlocked
            var vWestY = y;
            var vWestX = x - 1;

            //SouthBlocked
            var vSouthY = y + 1;
            var vSouthX = x;

            if (!PositionsOutOfRange(vEastY, vEastX, virtualMaze))
                if (virtualMaze[vEastY, vEastX] != config.Value.Path.Route)
                    virtualMaze[vEastY, vEastX] = objGameCurrentPositionAnswer.MazeBlockView.EastBlocked ? config.Value.Path.Wall : ' ';

            if (!PositionsOutOfRange(vNorthY, vNorthX, virtualMaze))
                if (virtualMaze[vNorthY, vNorthX] != config.Value.Path.Route)
                    virtualMaze[vNorthY, vNorthX] = objGameCurrentPositionAnswer.MazeBlockView.NorthBlocked ? config.Value.Path.Wall : ' ';

            if (!PositionsOutOfRange(vWestY, vWestX, virtualMaze))
                if (virtualMaze[vWestY, vWestX] != config.Value.Path.Route)
                    virtualMaze[vWestY, vWestX] = objGameCurrentPositionAnswer.MazeBlockView.WestBlocked ? config.Value.Path.Wall : ' ';

            if (!PositionsOutOfRange(vSouthY, vSouthX, virtualMaze))
                if (virtualMaze[vSouthY, vSouthX] != config.Value.Path.Route)
                    virtualMaze[vSouthY, vSouthX] = objGameCurrentPositionAnswer.MazeBlockView.SouthBlocked ? config.Value.Path.Wall : ' ';

            return virtualMaze;
        }

        public char[,] Print(char[,] virtualMaze)
        {
            Console.Clear();
            Console.WriteLine("Checking solutions...");
            for (int i = 0; i < virtualMaze.GetLength(0); i++)
            {
                for (int j = 0; j < virtualMaze.GetLength(1); j++)
                    Console.Write($"{virtualMaze[i, j]} ");

                Console.WriteLine();
            }
            return virtualMaze;
        }

        public bool PositionsOutOfRange(int positionY, int positionX, char[,] virtualMaze)
        {
            if (((positionY >= 0 && positionY < virtualMaze.GetLength(0)) && (positionX >= 0 && positionX < virtualMaze.GetLength(1))))
                return false;
            return true;
        }

        public GameCurrentPositionAnswerDto ReverseLatitude(GameCurrentPositionAnswerDto currentPosition, string latitude)
        {
            currentPosition.MazeBlockView.Latitude = latitude;
            switch (latitude)
            {
                case nameof(config.Value.Latitudes.GoEast):
                    currentPosition.MazeBlockView.ReverseLatitude = config.Value.Latitudes.GoWest;
                    break;
                case nameof(config.Value.Latitudes.GoNorth):
                    currentPosition.MazeBlockView.ReverseLatitude = config.Value.Latitudes.GoSouth;
                    break;
                case nameof(config.Value.Latitudes.GoWest):
                    currentPosition.MazeBlockView.ReverseLatitude = config.Value.Latitudes.GoEast;
                    break;
                case nameof(config.Value.Latitudes.GoSouth):
                    currentPosition.MazeBlockView.ReverseLatitude = config.Value.Latitudes.GoNorth;
                    break;
            }
            return currentPosition;
        }

        public CurrentPositionLatitudeAnswerDto ReversePositionsAndLatitude(int positionY, int positionX, string latitude)
        {
            CurrentPositionLatitudeAnswerDto currentPositionLatitude;
            switch (latitude)
            {
                case nameof(config.Value.Latitudes.GoEast):
                    positionX -= 1;
                    latitude = config.Value.Latitudes.GoWest;
                    break;
                case nameof(config.Value.Latitudes.GoNorth):
                    positionY += 1;
                    latitude = config.Value.Latitudes.GoSouth;
                    break;
                case nameof(config.Value.Latitudes.GoWest):
                    positionX += 1;
                    latitude = config.Value.Latitudes.GoEast;
                    break;
                case nameof(config.Value.Latitudes.GoSouth):
                    positionY -= 1;
                    latitude = config.Value.Latitudes.GoNorth;
                    break;
            }

            currentPositionLatitude = new CurrentPositionLatitudeAnswerDto()
            {
                Latitude = latitude,
                PositionX = positionX,
                PositionY = positionY,
            };
            return currentPositionLatitude;
        }
    }
}