using MazeChallengeCA.Dtos;
using MazeChallengeCA.Models;

namespace MazeChallengeCA.Interfaces
{
    public interface IMazeService
    {
        Task<MazeAnswerDto> CreateNewRandomMaze(Maze objMaze);
        Task<GameAnswerDto> CreateGameWithNewMaze(Game objGame);
        Task<GameCurrentPositionAnswerDto> MoveLatitude(Game objGame);
        Task<GameCurrentPositionAnswerDto> GameCurrentPosition(CurrentPositionDto objCurrentPosition);
        Task<DebugingPurpousesAnswerDto> DebugingPurpouses(string url);
    }
}