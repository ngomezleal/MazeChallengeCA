using MazeChallengeCA.Dtos;
using MazeChallengeCA.Helpers;
using MazeChallengeCA.Interfaces;
using System;

namespace MazeChallengeCA.Services
{
    public class Miscellaneous : IMiscellaneous
    {
        public char[,] InitializeVirtualMaze(char[,] virtualMaze)
        {
            for (int i = 0; i < virtualMaze.GetLength(0); i++)
            {
                for (int j = 0; j < virtualMaze.GetLength(1); j++)
                    virtualMaze[i, j] = ' ';
            }
            return virtualMaze;
        }

        public char[,] RecalculatePositions(GameCurrentPositionAnswerDto objGameCurrentPositionAnswer, char[,] virtualMaze)
        {
            var y = objGameCurrentPositionAnswer.MazeBlockView.CoordY;
            var x = objGameCurrentPositionAnswer.MazeBlockView.CoordX;

            //EastBlocked
            var vEastY = y;
            var vEastX = x + 1;

            //NorthBlocked
            var vNorthY = y - 1;
            var vNorthX = x;

            //WestBlocked
            var vWestY = y;
            var vWestX = x - 1;

            //SouthBlocked
            var vSouthY = y + 1;
            var vSouthX = x;

            if (((vEastY >= 0 && vEastY < virtualMaze.GetLength(0)) && (vEastX >= 0 && vEastX < virtualMaze.GetLength(1))))
            {
                if (virtualMaze[vEastY, vEastX] != '*')
                    virtualMaze[vEastY, vEastX] = objGameCurrentPositionAnswer.MazeBlockView.EastBlocked ? '#' : ' ';
            }

            if (((vNorthY >= 0 && vNorthY < virtualMaze.GetLength(0)) && (vNorthX >= 0 && vNorthX < virtualMaze.GetLength(1))))
            {
                if (virtualMaze[vNorthY, vNorthX] != '*')
                    virtualMaze[vNorthY, vNorthX] = objGameCurrentPositionAnswer.MazeBlockView.NorthBlocked ? '#' : ' ';
            }

            if (((vWestY >= 0 && vWestY < virtualMaze.GetLength(0)) && (vWestX >= 0 && vWestX < virtualMaze.GetLength(1))))
            {
                if (virtualMaze[vWestY, vWestX] != '*')
                    virtualMaze[vWestY, vWestX] = objGameCurrentPositionAnswer.MazeBlockView.WestBlocked ? '#' : ' ';
            }

            if (((vSouthY >= 0 && vSouthY < virtualMaze.GetLength(0)) && (vSouthX >= 0 && vSouthX < virtualMaze.GetLength(1))))
            {
                if (virtualMaze[vSouthY, vSouthX] != '*')
                    virtualMaze[vSouthY, vSouthX] = objGameCurrentPositionAnswer.MazeBlockView.SouthBlocked ? '#' : ' ';
            }

            return virtualMaze;
        }

        public char[,] Print(char[,] virtualMaze)
        {
            Console.Clear();
            for (int i = 0; i < virtualMaze.GetLength(0); i++)
            {
                for (int j = 0; j < virtualMaze.GetLength(1); j++)
                    Console.Write($"{virtualMaze[i, j]} ");

                Console.WriteLine();
            }
            return virtualMaze;
        }

        public GameCurrentPositionAnswerDto ReverseLatitude(GameCurrentPositionAnswerDto currentPosition, string latitude)
        {
            currentPosition.MazeBlockView.Latitude = latitude;
            switch (latitude)
            {
                case "GoEast":
                    currentPosition.MazeBlockView.ReverseLatitude = Constants.GoWest;
                    break;
                case "GoNorth":
                    currentPosition.MazeBlockView.ReverseLatitude = Constants.GoSouth;
                    break;
                case "GoWest":
                    currentPosition.MazeBlockView.ReverseLatitude = Constants.GoEast;
                    break;
                case "GoSouth":
                    currentPosition.MazeBlockView.ReverseLatitude = Constants.GoNorth;
                    break;
            }
            return currentPosition;
        }
    }
}