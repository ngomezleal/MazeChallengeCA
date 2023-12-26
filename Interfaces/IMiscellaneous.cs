using MazeChallengeCA.Dtos;
using System;

namespace MazeChallengeCA.Interfaces
{
    public interface IMiscellaneous
    {
        char[,] InitializeVirtualMaze(char[,] virtualMaze);
        char[,] RecalculatePositions(GameCurrentPositionAnswerDto objGameCurrentPositionAnswer, char[,] virtualMaze);
        char[,] Print(char[,] virtualMaze);
        GameCurrentPositionAnswerDto ReverseLatitude(GameCurrentPositionAnswerDto currentPositionAnswerDto, string latitude);
    }
}