using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace PERM.Player
{
    public class NoteCreator : MonoBehaviour
    {
        [SerializeField] private LineInfo LineInfo;
        [SerializeField] private EventHandler EventHandler;
        [SerializeField] private GameObject NotePrefab;
        [SerializeField] private Transform NoteStage;
        [SerializeField] private float LoadRange = 0.5f;

        private List<int> CachedNoteIds = new List<int>();
        public NoteInfo[] NoteInfos = new NoteInfo[0];

        [SerializeField] private int LeftIndex = 1;
        [SerializeField] private int RightIndex = 1; 

        private void Start()
        {
            NoteStage = GameSet.Instance.NoteStage;
        }

        private void Update()
        {
            float currentFloor = EventHandler.LineFloor;
            float currentTime = EventHandler.LineTime;
            float maxFloor = currentFloor + LoadRange;

            // �����ڷ�Χ�ڵ� Note �ƶ��� NoteStage
            foreach (NoteInfo noteInfo in NoteInfos)
            {
                if (noteInfo.startFloor > maxFloor
                    || noteInfo.endFloor < currentFloor
                    || noteInfo.endTime < currentTime)
                {
                    RecycleNote(noteInfo.gameObject);
                    CachedNoteIds.Remove(noteInfo.id);
                    NoteInfos = NoteInfos.Where(n => n.id != noteInfo.id).ToArray(); // ���»���
                }
            }

            //���±�����
            while (currentFloor > LineInfo.notes[LeftIndex].startFloor)
            {
                LeftIndex++;
                if (currentFloor <= LineInfo.notes[LeftIndex].startFloor)
                {
                    break;
                }
            }
            while (currentFloor <= LineInfo.notes[LeftIndex].startFloor)
            {
                if (currentFloor > LineInfo.notes[LeftIndex - 1].startFloor) 
                {
                    break;
                }
                LeftIndex--;
            }

            //���±�����
            while (maxFloor > LineInfo.notes[RightIndex].startFloor)
            {
                if (maxFloor <= LineInfo.notes[RightIndex + 1].startFloor)
                {
                    break;
                }
                RightIndex++;
            }
            while (maxFloor <= LineInfo.notes[RightIndex].startFloor)
            {
                RightIndex--;
                if (maxFloor > LineInfo.notes[RightIndex].startFloor)
                {
                    break;
                }
            }

            for (int i = LeftIndex; i <= RightIndex; i++)
            {
                if (i != -1 || i != LineInfo.notes.Count)
                {
                    if (LineInfo.notes[i].endTime >= currentTime && !CachedNoteIds.Contains(LineInfo.notes[i].id))
                    {
                        GameObject noteObject = GetOrCreateNote();
                        InitializeNote(noteObject, LineInfo.notes[i]);
                        CachedNoteIds.Add(LineInfo.notes[i].id);
                        NoteInfos = NoteInfos.Append(noteObject.GetComponent<NoteInfo>()).ToArray(); // ���»���
                    }
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
            noteInfo.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}