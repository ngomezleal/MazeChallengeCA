using System;

namespace MazeChallengeCA.Dtos
{
    public class CurrentPositionLatitudeAnswerDto
    {
        public string Latitude { get; set; } = string.Empty;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}