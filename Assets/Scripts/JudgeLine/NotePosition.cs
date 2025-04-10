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
                noteInfo.transform.localPosition = new Vector3(noteInfo.positionX, (noteInfo.startFloor - EventHandler.LineFloor) * LineInfo.bpm / 5000, 0);
                noteInfo.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
