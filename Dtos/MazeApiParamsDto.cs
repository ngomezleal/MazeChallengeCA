using System;

namespace MazeChallengeCA.Dtos
{
    public class MazeApiParamsDto
    {
        public string? MainUri { get; set; }
        public string? NewMazeApiUri { get; set; }
        public string? GameApiUri { get; set; }
        public string? Code { get; set; }
        public string? Operation { get; set; }
        public string? HttpClientConfigureName { get; set; }
    }
}