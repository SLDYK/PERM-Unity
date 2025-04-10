using System.Collections.Generic;
using UnityEngine;
namespace PERM.Player
{
    public class LineInfo : MonoBehaviour
    {
        [Header("LineData")]
        public float bpm;
        public List<Note> notes;
        public List<MoveEvent> MoveEvents;
        public List<Event> FloorEvents;
        public List<Event> RotateEvents;
        public List<Event> DisappearEvents;
    }
}