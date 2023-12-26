using System;

namespace MazeChallengeCA.Dtos
{
    public class MazeAnswerDto
    {
        public string MazeUid { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
    }
}