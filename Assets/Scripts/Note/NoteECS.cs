using Unity.Entities;

namespace PERM.Player
{
    public struct NoteECS : IComponentData
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
    }
}
