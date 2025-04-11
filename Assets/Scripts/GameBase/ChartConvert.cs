using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace PERM.Player
{
    public static class ChartConvert
    {
        private static System.Random random = new System.Random();
        internal static Chart CreateChart(PhiChart PhiChart)
        {
            Chart chart = new Chart
            {
                offset = PhiChart.offset,
                judgeLineList = new List<JudgeLine>() // 初始化列表
            };

            // 使用 Parallel.ForEach 并行处理
            Parallel.ForEach(PhiChart.judgeLineList, phiJudgeLine =>
            {
                JudgeLine judgeLine = new JudgeLine
                {
                    bpm = phiJudgeLine.bpm,
                    notes = new List<Note>(), // 初始化音符列表
                    MoveEvents = new List<MoveEvent>(), // 初始化移动事件列表
                    FloorEvents = new List<Event>(), // 初始化地板事件列表
                    RotateEvents = new List<Event>(), // 初始化旋转事件列表
                    DisappearEvents = new List<Event>() // 初始化消失事件列表
                };

                // 处理速度事件
                ProcessSpeedEvents(phiJudgeLine.speedEvents, judgeLine);

                // 处理移动事件
                ProcessMoveEvents(phiJudgeLine.judgeLineMoveEvents, judgeLine);

                // 处理旋转事件
                ProcessGenericEvents(phiJudgeLine.judgeLineRotateEvents, judgeLine.RotateEvents);

                // 处理消失事件
                ProcessGenericEvents(phiJudgeLine.judgeLineDisappearEvents, judgeLine.DisappearEvents);

                // 处理音符
                ProcessNotes(phiJudgeLine.notesAbove, judgeLine, 1);
                ProcessNotes(phiJudgeLine.notesBelow, judgeLine, -1);

                //添加两个占位音符
                Add2Notes(judgeLine);

                // 按照 startFloor 对音符进行排序
                judgeLine.notes.Sort((note1, note2) => note1.startFloor.CompareTo(note2.startFloor));

                lock (chart.judgeLineList) // 确保线程安全
                {
                    chart.judgeLineList.Add(judgeLine);
                }
            });

            return chart;
        }

        private static void ProcessSpeedEvents(List<PhiSpeedEvent> speedEvents, JudgeLine judgeLine)
        {
            float recentFloor = 0;
            foreach (PhiSpeedEvent phiSpeedEvent in speedEvents)
            {
                Event floorEvent = new Event
                {
                    startTime = phiSpeedEvent.startTime,
                    endTime = phiSpeedEvent.endTime,
                    start = recentFloor
                };
                recentFloor += (phiSpeedEvent.endTime - phiSpeedEvent.startTime) / 32 / judgeLine.bpm * 60 * phiSpeedEvent.value;
                floorEvent.end = recentFloor;
                judgeLine.FloorEvents.Add(floorEvent);
            }
        }

        private static void ProcessMoveEvents(List<PhiMoveEvent> moveEvents, JudgeLine judgeLine)
        {
            foreach (PhiMoveEvent phiMoveEvent in moveEvents)
            {
                MoveEvent moveEvent = new MoveEvent
                {
                    startTime = phiMoveEvent.startTime,
                    endTime = phiMoveEvent.endTime,
                    start = phiMoveEvent.start,
                    end = phiMoveEvent.end,
                    start2 = phiMoveEvent.start2,
                    end2 = phiMoveEvent.end2
                };
                judgeLine.MoveEvents.Add(moveEvent);
            }
        }

        private static void ProcessGenericEvents(List<PhiEvent> phiEvents, List<Event> targetEvents)
        {
            foreach (PhiEvent phiEvent in phiEvents)
            {
                Event genericEvent = new Event
                {
                    startTime = phiEvent.startTime,
                    endTime = phiEvent.endTime,
                    start = phiEvent.start,
                    end = phiEvent.end
                };
                targetEvents.Add(genericEvent);
            }
        }

        private static void ProcessNotes(List<PhiNote> phiNotes, JudgeLine judgeLine, int direction)
        {
            int id = judgeLine.notes.Count; // 确保 ID 连续
            foreach (PhiNote phiNote in phiNotes)
            {
                Note note = new Note
                {
                    id = id++,
                    direction = direction,
                    type = phiNote.type,
                    startTime = phiNote.time,
                    endTime = phiNote.time + phiNote.holdTime,
                    positionX = phiNote.positionX,
                    speed = phiNote.speed,
                    startFloor = CalculateFloor(judgeLine.FloorEvents, phiNote.time),
                    endFloor = CalculateFloor(judgeLine.FloorEvents, phiNote.time + phiNote.holdTime)
                };
                judgeLine.notes.Add(note);
            }
        }

        private static void Add2Notes(JudgeLine judgeLine)
        {
            Note Left = new Note
            {
                startFloor = float.NegativeInfinity
            };
            judgeLine.notes.Add(Left);

            Note Right = new Note
            {
                startFloor = float.PositiveInfinity
            };
            judgeLine.notes.Add(Right);
        }

        private static float CalculateFloor(List<Event> floorEvents, float time)
        {
            foreach (Event floorEvent in floorEvents)
            {
                if (time >= floorEvent.startTime && time <= floorEvent.endTime)
                {
                    float t = (time - floorEvent.startTime) / (floorEvent.endTime - floorEvent.startTime);
                    return Mathf.Lerp(floorEvent.start, floorEvent.end, t);
                }
            }
            return floorEvents.Count > 0 ? floorEvents[^1].end : 0;
        }
    }
}