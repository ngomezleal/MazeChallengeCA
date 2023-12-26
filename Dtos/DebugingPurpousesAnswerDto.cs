using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeChallengeCA.Dtos
{
    public class DebugingPurpousesAnswerDto
    {
        public string MazeUid { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public List<CurrentPositionMazeBlockViewDto>? Blocks { get; set; }
    }
}
