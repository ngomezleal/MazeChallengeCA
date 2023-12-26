using MazeChallengeCA.Dtos;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace MazeChallengeCA.Services
{
    public class MazeService: IMazeService
    {
        private readonly ILogger<MazeService> logger;
        public MazeService(ILogger<MazeService> logger)
        {
            this.logger = logger;
        }

        //Step 1
        public async Task<MazeAnswerDto> CreateNewRandomMaze(Maze objMaze)
        {
            var newMaze = new MazeAnswerDto();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(objMaze.Url, objMaze);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    newMaze = JsonSerializer.Deserialize<MazeAnswerDto>(body);
                }
            }
            return newMaze;
        }

        //Step 2
        public async Task<GameAnswerDto> CreateGameWithNewMaze(Game objGame)
        {
            var gameAnswer = new GameAnswerDto();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(objGame.Url, objGame);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    gameAnswer = JsonSerializer.Deserialize<GameAnswerDto>(body);
                }
            }
            return gameAnswer;
        }

        //Step 3
        public async Task<GameCurrentPositionAnswerDto> GameCurrentPosition(CurrentPositionDto objCurrentPosition)
        {
            var currentPositionAnswer = new GameCurrentPositionAnswerDto();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(objCurrentPosition.Url);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    currentPositionAnswer = JsonSerializer.Deserialize<GameCurrentPositionAnswerDto>(body);
                }
            }
            return currentPositionAnswer;
        }

        //Step 4
        //It allow to move another latitude
        public async Task<GameCurrentPositionAnswerDto> MoveLatitude(Game objGame)
        {
            var latitudes = new GameCurrentPositionAnswerDto();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(objGame.Url, objGame);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    latitudes = JsonSerializer.Deserialize<GameCurrentPositionAnswerDto>(body);
                }
            }
            return latitudes;
        }

        //Intermediate
        public async Task<DebugingPurpousesAnswerDto> DebugingPurpouses(string url)
        {
            var debuggingAnswer = new DebugingPurpousesAnswerDto();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    debuggingAnswer = JsonSerializer.Deserialize<DebugingPurpousesAnswerDto>(body);
                }
            }
            return debuggingAnswer;
        }
    }
}