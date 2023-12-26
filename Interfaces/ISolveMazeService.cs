using MazeChallengeCA.Dtos;

namespace MazeChallengeCA.Interfaces
{
    internal interface ISolveMazeService
    {
        Task<bool> SolveMaze(GameAnswerDto game, string latitude = "", int y = 0, int x = 0);
    }
}