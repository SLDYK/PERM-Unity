using System.Collections.Generic;
namespace PERM.Player
{
    public class Chart
    {
        public int formatVersion { get; set; }
        public float offset { get; set; }
        public List<JudgeLineList> judgeLineList { get; set; }
    }
    public class JudgeLineList
    {
        public float bpm { get; set; }
        public List<Notes> notesAbove { get; set; }
        public List<Notes> notesBelow { get; set; }
        public List<SpeedEvent> speedEvents { get; set; }
        public List<MoveEvent> judgeLineMoveEvents { get; set; }
        public List<Event> judgeLineRotateEvents { get; set; }
        public List<Event> judgeLineDisappearEvents { get; set; }
    }
    public class TimeRange
    {
        public float startTime { get; set; }
        public float endTime { get; set; }
    }
    public class SpeedEvent : TimeRange
    {
        public float value { get; set; }
    }
    public class Event : TimeRange
    {
        public float start { get; set; }
        public float end { get; set; }
    }
    public class MoveEvent : Event
    {
        public float start2 { get; set; }
        public float end2 { get; set; }
    }
    public class Notes
    {
        public int type { get; set; }
        public int time { get; set; }
        public double positionX { get; set; }
        public double holdTime { get; set; }
        public double speed { get; set; }
        public double floorPosition { get; set; }
    }
}
