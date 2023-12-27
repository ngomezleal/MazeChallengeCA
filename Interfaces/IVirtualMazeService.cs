using MazeChallengeCA.Dtos;

namespace MazeChallengeCA.Interfaces
{
    public interface IVirtualMazeService
    {
        char[,] InitializeVirtualMaze(char[,] virtualMaze);
        char[,] RecalculatePositions(GameCurrentPositionAnswerDto objGameCurrentPositionAnswer, char[,] virtualMaze);
        char[,] Print(char[,] virtualMaze);
        bool PositionsOutOfRange(int positionY, int positionX, char[,] virtualMaze);
        GameCurrentPositionAnswerDto ReverseLatitude(GameCurrentPositionAnswerDto currentPositionAnswerDto, string latitude);
        CurrentPositionLatitudeAnswerDto ReversePositionsAndLatitude(int positionY, int positionX, string latitude);
    }
}