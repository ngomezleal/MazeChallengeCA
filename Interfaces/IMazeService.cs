using MazeChallengeCA.Dtos;
using MazeChallengeCA.Models;

namespace MazeChallengeCA.Interfaces
{
    public interface IMazeService
    {
        Task<MazeAnswerDto> CreateNewRandomMaze(Maze maze);
        Task<GameAnswerDto> CreateGameWithNewMaze(Game game);
        Task<GameCurrentPositionAnswerDto> MoveLatitude(Game game);
        Task<GameCurrentPositionAnswerDto> GameCurrentPosition(CurrentPositionDto currentPosition);
        Task<DebugingPurpousesAnswerDto> DebugingPurpouses(string uri);
    }
}