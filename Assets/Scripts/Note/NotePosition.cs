using UnityEngine;
namespace PERM.Player
{
    public class NotePosition : MonoBehaviour
    {
        [SerializeField] NoteCreator NoteCreator;
        [SerializeField] EventHandler EventHandler;
        [SerializeField] LineInfo LineInfo;

        private void Update()
        {
            foreach (NoteInfo noteInfo in NoteCreator.NoteInfos)
            {
                float PosY = (noteInfo.startFloor - EventHandler.LineFloor) * noteInfo.direction * 6;
                noteInfo.transform.localPosition = new Vector3(noteInfo.positionX, PosY, 0);
            }
        }
    }
}