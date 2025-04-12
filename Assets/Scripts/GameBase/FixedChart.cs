using System.Collections.Generic;

namespace PERM.Player
{
    // 自定义谱面格式
    [System.Serializable]
    public class Chart
    {
        public float offset;
        public List<JudgeLine> judgeLineList;
    }
    [System.Serializable]
    public class JudgeLine
    {
        public float bpm;
        public List<Note> notes;
        public List<MoveEvent> MoveEvents;
        public List<Event> FloorEvents;
        public List<Event> RotateEvents;
        public List<Event> DisappearEvents;
    }
    [System.Serializable]
    public class Event
    {
        public float startTime;
        public float endTime;
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
    public class Note
    {
        public int id;
        public int direction;
        public int type;
        public int startTime;
        public int endTime;
        public float positionX;
        public float speed;
        public float startFloor;
        public float endFloor;
        public bool isCreated = false;
    }

    // 官方谱面格式
    [System.Serializable]
    public class PhiChart
    {
        public int formatVersion;
        public float offset;
        public List<PhiJudgeLine> judgeLineList;
    }
    [System.Serializable]
    public class PhiJudgeLine
    {
        public float bpm;
        public List<PhiNote> notesAbove;
        public List<PhiNote> notesBelow;
        public List<PhiSpeedEvent> speedEvents;
        public List<PhiMoveEvent> judgeLineMoveEvents;
        public List<PhiEvent> judgeLineRotateEvents;
        public List<PhiEvent> judgeLineDisappearEvents;
    }
    [System.Serializable]
    public class PhiTimeRange
    {
        public float startTime;
        public float endTime;
    }
    [System.Serializable]
    public class PhiSpeedEvent : PhiTimeRange
    {
        public float value;
    }
    [System.Serializable]
    public class PhiEvent : PhiTimeRange
    {
        public float start;
        public float end;
    }
    [System.Serializable]
    public class PhiMoveEvent : PhiEvent
    {
        public float start2;
        public float end2;
    }
    [System.Serializable]
    public class PhiNote
    {
        public int type;
        public int time;
        public float positionX;
        public int holdTime;
        public float speed;
        public float floorPosition;
    }
}