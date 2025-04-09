using System.Collections.Generic;
namespace PERM.Player
{
    [System.Serializable]
    public class Chart
    {
        public int formatVersion;
        public float offset;
        public List<JudgeLine> judgeLineList;
    }
    [System.Serializable]
    public class JudgeLine
    {
        public float bpm;
        public List<Notes> notesAbove;
        public List<Notes> notesBelow;
        public List<SpeedEvent> speedEvents;
        public List<MoveEvent> judgeLineMoveEvents;
        public List<Event> judgeLineRotateEvents;
        public List<Event> judgeLineDisappearEvents;
    }
    [System.Serializable]
    public class TimeRange
    {
        public float startTime;
        public float endTime;
    }
    [System.Serializable]
    public class SpeedEvent : TimeRange
    {
        public float value;
    }
    [System.Serializable]
    public class Event : TimeRange
    {
        public float start;
        public float end;
    }
    [System.Serializable]
    public class MoveEvent : Event
    {
        public float start2;
        public float end2;
    }
    [System.Serializable]
    public class Notes
    {
        public int type;
        public int time;
        public double positionX;
        public double holdTime;
        public double speed;
        public double floorPosition;
    }
}
