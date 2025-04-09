using UnityEngine;
namespace PERM.Player
{
    public class LineCreator : MonoBehaviour
    {
        [SerializeField] private GameObject LinePrefab;
        [SerializeField] private Chart Chart;

        // 本质是 start 函数，但由 GameSet 调用
        public void LineCreatorStart()
        {
            Chart = GameSet.Instance.Chart;

            foreach (JudgeLine line in Chart.judgeLineList)
            {
                // 创建一条判定线
                GameObject LineObject = Instantiate(LinePrefab, this.transform);
                LineObject.transform.localPosition = Vector3.zero;

                // 获取 LineInfo 组件并赋值
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
