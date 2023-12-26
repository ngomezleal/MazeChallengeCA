using System;

namespace MazeChallengeCA.Dtos
{
    public class GameCurrentPositionAnswerDto
    {
        public GameAnswerDto? Game { get; set; }
        public CurrentPositionMazeBlockViewDto? MazeBlockView { get; set; }
    }
}
