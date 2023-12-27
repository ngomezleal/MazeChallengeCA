using MazeChallengeCA.Dtos;
using MazeChallengeCA.Interfaces;
using MazeChallengeCA.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace MazeChallengeCA.Services
{
    public class MazeService: IMazeService
    {
        private readonly IOptions<GlobalConfigurationDto> config;
        private readonly IHttpClientFactory httpClientFactory;

        public MazeService(IOptions<GlobalConfigurationDto> options,
            IHttpClientFactory httpClientFactory)
        {
            this.config = options;
            this.httpClientFactory = httpClientFactory;
        }

        //Step 1
        public async Task<MazeAnswerDto> CreateNewRandomMaze(Maze maze)
        {
            var newMaze = new MazeAnswerDto();
            var client = httpClientFactory.CreateClient(config.Value.Maze.HttpClientConfigureName);
            var response = await client.PostAsJsonAsync(maze.Url, maze);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                newMaze = JsonSerializer.Deserialize<MazeAnswerDto>(body);
            }
            else
            {
                Console.WriteLine("The service 'CreateNewRandomMaze' has failed");
                Environment.Exit(0);
            }
            return newMaze;
        }

        //Step 2
        public async Task<GameAnswerDto> CreateGameWithNewMaze(Game game)
        {
            var gameAnswer = new GameAnswerDto();
            var client = httpClientFactory.CreateClient(config.Value.Maze.HttpClientConfigureName);
            var response = await client.PostAsJsonAsync(game.Uri, game);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                gameAnswer = JsonSerializer.Deserialize<GameAnswerDto>(body);
            }
            else
            {
                Console.WriteLine("The service 'CreateGameWithNewMaze' has failed");
                Environment.Exit(0);
            }
            return gameAnswer;
        }

        //Step 3
        public async Task<GameCurrentPositionAnswerDto> GameCurrentPosition(CurrentPositionDto currentPosition)
        {
            var currentPositionAnswer = new GameCurrentPositionAnswerDto();
            var client = httpClientFactory.CreateClient(config.Value.Maze.HttpClientConfigureName);
            var response = await client.GetAsync(currentPosition.Url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                currentPositionAnswer = JsonSerializer.Deserialize<GameCurrentPositionAnswerDto>(body);
            }
            else
            {
                Console.WriteLine("The service 'GameCurrentPosition' has failed");
                Environment.Exit(0);
            }
            return currentPositionAnswer;
        }

        //Step 4
        //It allow to move another latitude
        public async Task<GameCurrentPositionAnswerDto> MoveLatitude(Game game)
        {
            var latitudes = new GameCurrentPositionAnswerDto();
            var client = httpClientFactory.CreateClient(config.Value.Maze.HttpClientConfigureName);
            var response = await client.PostAsJsonAsync(game.Uri, game);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                latitudes = JsonSerializer.Deserialize<GameCurrentPositionAnswerDto>(body);
            }
            return latitudes;
        }

        //Intermediate
        public async Task<DebugingPurpousesAnswerDto> DebugingPurpouses(string uri)
        {
            var debuggingAnswer = new DebugingPurpousesAnswerDto();
            var client = httpClientFactory.CreateClient(config.Value.Maze.HttpClientConfigureName);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                debuggingAnswer = JsonSerializer.Deserialize<DebugingPurpousesAnswerDto>(body);
            }
            else
            {
                Console.WriteLine("The service 'DebugingPurpouses' has failed");
                Environment.Exit(0);
            }
            return debuggingAnswer;
        }
    }
}