using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeChallengeCA.Dtos
{
    public class GameAnswerDto
    {
        public string MazeUid { get; set; } = string.Empty;
        public string GameUid { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public int CurrentPositionX { get; set; }
        public int CurrentPositionY { get; set; }
    }
}
