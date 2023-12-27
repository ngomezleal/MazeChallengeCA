using System;

namespace MazeChallengeCA.Dtos
{
    public class GlobalConfigurationDto
    {
        public MazeApiParamsDto Maze { get; set; }
        public MazePathDto Path { get; set; }
        public MazeSizeDto MazeSize { get; set; }
        public LatitudesDto Latitudes { get; set; }
    }
}