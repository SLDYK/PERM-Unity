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
                lineInfo.notesAbove = line.notesAbove;
                lineInfo.notesBelow = line.notesBelow;
                lineInfo.speedEvents = line.speedEvents;
                lineInfo.judgeLineMoveEvents = line.judgeLineMoveEvents;
                lineInfo.judgeLineRotateEvents = line.judgeLineRotateEvents;
                lineInfo.judgeLineDisappearEvents = line.judgeLineDisappearEvents;
            }
        }
    }
}
