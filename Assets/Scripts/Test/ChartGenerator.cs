using System;
using UnityEngine;

namespace PERM.Player
{
    public class ChartGenerator
    {
        private static System.Random random = new System.Random();

        public static void GenerateRandomNotes(Chart chart, int noteCount)
        {
            for (int i = 0; i < noteCount; i++)
            {
                Note newNote = new Note
                {
                    id = i,
                    direction = 1,
                    type = random.Next(0, 3),
                    startTime = random.Next(0, 24600),
                    positionX = (float)random.Next(-4,4),
                    speed = 1
                };
                newNote.endTime = newNote.startTime + random.Next(100, 500);
                int judgeLineIndex = random.Next(0, chart.judgeLineList.Count);
                chart.judgeLineList[judgeLineIndex].notes.Add(newNote);
            }
        }
    }
}
