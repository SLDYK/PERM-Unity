using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PERM.Player
{
    public class NoteCreator : MonoBehaviour
    {
        [SerializeField] private LineInfo LineInfo;
        [SerializeField] private EventHandler EventHandler;
        [SerializeField] private GameObject NotePrefab;
        [SerializeField] private Transform NoteStage;
        [SerializeField] private float FloorRange = 1;

        private float LoadRange;
        private List<int> CachedNoteIds = new List<int>();
        public NoteInfo[] NoteInfos = new NoteInfo[0];

        private void Start()
        {
            LoadRange = LineInfo.bpm * FloorRange;
            NoteStage = GameSet.Instance.NoteStage;
        }

        private void Update()
        {
            float currentFloor = EventHandler.LineFloor;
            float maxFloor = currentFloor + LoadRange;

            // �����ڷ�Χ�ڵ� Note �ƶ��� NoteStage
            foreach (NoteInfo noteInfo in NoteInfos)
            {
                if (noteInfo.startFloor >= maxFloor || noteInfo.endFloor <= currentFloor)
                {
                    RecycleNote(noteInfo.gameObject);
                    CachedNoteIds.Remove(noteInfo.id);
                    NoteInfos = NoteInfos.Where(n => n.id != noteInfo.id).ToArray(); // ���»���
                }
            }

            // �������� Note
            foreach (Note note in LineInfo.notes)
            {
                if (note.startFloor < maxFloor && note.endFloor > currentFloor && !CachedNoteIds.Contains(note.id))
                {
                    GameObject noteObject = GetOrCreateNote();
                    InitializeNote(noteObject, note);
                    CachedNoteIds.Add(note.id);
                    NoteInfos = NoteInfos.Append(noteObject.GetComponent<NoteInfo>()).ToArray(); // ���»���
                }
            }
        }

        private void RecycleNote(GameObject noteObject)
        {
            noteObject.SetActive(false);
            noteObject.transform.SetParent(NoteStage);
        }

        private GameObject GetOrCreateNote()
        {
            // �� NoteStage �в��ҿ��е� Note
            foreach (Transform child in NoteStage)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    child.SetParent(transform);
                    return child.gameObject;
                }
            }

            // ���û�п��е� Note���򴴽��µ�
            GameObject newNote = Instantiate(NotePrefab, transform);
            return newNote;
        }

        private void InitializeNote(GameObject noteObject, Note note)
        {
            NoteInfo noteInfo = noteObject.GetComponent<NoteInfo>();
            noteInfo.id = note.id;
            noteInfo.direction = note.direction;
            noteInfo.type = note.type;
            noteInfo.startTime = note.startTime;
            noteInfo.endTime = note.endTime;
            noteInfo.positionX = note.positionX;
            noteInfo.speed = note.speed;
            noteInfo.startFloor = note.startFloor;
            noteInfo.endFloor = note.endFloor;
        }
    }
}