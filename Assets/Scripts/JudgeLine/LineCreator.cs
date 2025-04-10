using UnityEngine;
namespace PERM.Player
{
    public class LineCreator : MonoBehaviour
    {
        [SerializeField] private GameObject LinePrefab;
        [SerializeField] private Chart Chart;

        // ������ start ���������� GameSet ����
        public void LineCreatorStart()
        {
            Chart = GameSet.Instance.Chart;

            foreach (JudgeLine line in Chart.judgeLineList)
            {
                // ����һ���ж���
                GameObject LineObject = Instantiate(LinePrefab, this.transform);
                LineObject.transform.localPosition = Vector3.zero;

                // ��ȡ LineInfo �������ֵ
                LineInfo lineInfo = LineObject.GetComponent<LineInfo>();
                lineInfo.bpm = line.bpm;
                lineInfo.MoveEvents = line.MoveEvents;
                lineInfo.RotateEvents = line.RotateEvents;
                lineInfo.DisappearEvents = line.DisappearEvents;
                lineInfo.FloorEvents = line.FloorEvents;
                lineInfo.notes = line.notes;
            }
        }
    }
}
