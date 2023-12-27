using MazeChallengeCA.Dtos;
using MazeChallengeCA.Helpers;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace MazeChallengeCA.Services
{
    public class MazeService: IMazeService
    {
        private readonly ILogger<MazeService> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public MazeService(ILogger<MazeService> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        //Step 1
        public async Task<MazeAnswerDto> CreateNewRandomMaze(Maze objMaze)
        {
            var newMaze = new MazeAnswerDto();
            var client = httpClientFactory.CreateClient(Constants.HttpClientConfigureName);
            var response = await client.PostAsJsonAsync(objMaze.Url, objMaze);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                newMaze = JsonSerializer.Deserialize<MazeAnswerDto>(body);
            }
            return newMaze;
        }

        //Step 2
        public async Task<GameAnswerDto> CreateGameWithNewMaze(Game objGame)
        {
            var gameAnswer = new GameAnswerDto();
            var client = httpClientFactory.CreateClient(Constants.HttpClientConfigureName);
            var response = await client.PostAsJsonAsync(objGame.Url, objGame);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                gameAnswer = JsonSerializer.Deserialize<GameAnswerDto>(body);
            }
            return gameAnswer;
        }

        //Step 3
        public async Task<GameCurrentPositionAnswerDto> GameCurrentPosition(CurrentPositionDto objCurrentPosition)
        {
            var currentPositionAnswer = new GameCurrentPositionAnswerDto();
            var client = httpClientFactory.CreateClient(Constants.HttpClientConfigureName);
            var response = await client.GetAsync(objCurrentPosition.Url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                currentPositionAnswer = JsonSerializer.Deserialize<GameCurrentPositionAnswerDto>(body);
            }
            return currentPositionAnswer;
        }

        //Step 4
        //It allow to move another latitude
        public async Task<GameCurrentPositionAnswerDto> MoveLatitude(Game objGame)
        {
            var latitudes = new GameCurrentPositionAnswerDto();
            var client = httpClientFactory.CreateClient(Constants.HttpClientConfigureName);
            var response = await client.PostAsJsonAsync(objGame.Url, objGame);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                latitudes = JsonSerializer.Deserialize<GameCurrentPositionAnswerDto>(body);
            }
            return latitudes;
        }

        //Intermediate
        public async Task<DebugingPurpousesAnswerDto> DebugingPurpouses(string url)
        {
            var debuggingAnswer = new DebugingPurpousesAnswerDto();
            var client = httpClientFactory.CreateClient(Constants.HttpClientConfigureName);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                debuggingAnswer = JsonSerializer.Deserialize<DebugingPurpousesAnswerDto>(body);
            }
            return debuggingAnswer;
        }
    }
}