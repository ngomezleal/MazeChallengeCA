using System;

namespace MazeChallengeCA.Dtos
{
    public class CurrentPositionMazeBlockViewDto
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public bool NorthBlocked { get; set; }
        public bool SouthBlocked { get; set; }
        public bool WestBlocked { get; set; }
        public bool EastBlocked { get; set; }
        public string? Latitude { get; set; }
        public string? ReverseLatitude { get; set; }
    }
}
