using System.Collections.Generic;
using UnityEngine;
namespace PERM.Player
{
    public class LineInfo : MonoBehaviour
    {
        [Header("LineData")]
        public float bpm;
        public List<Notes> notesAbove;
        public List<Notes> notesBelow;
        public List<SpeedEvent> speedEvents;
        public List<MoveEvent> judgeLineMoveEvents;
        public List<Event> judgeLineRotateEvents;
        public List<Event> judgeLineDisappearEvents;
    }
}