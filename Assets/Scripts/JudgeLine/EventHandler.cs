using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PERM.Player
{
    public class EventHandler : MonoBehaviour
    {
        [SerializeField] private LineInfo LineInfo;
        [SerializeField] private Timer Timer;
        [SerializeField] private SpriteRenderer SpriteRenderer;

        private bool Test = true;
        private Camera MainCamera;
        private float LineTime;
        private Vector2 ScreenSize;
        private Vector2 ScreenBottomLeft;
        private Vector2 ScreenTopRight;

        // Ϊÿ���¼�ά���±�
        private int moveEventIndex = 0;
        private int rotateEventIndex = 0;
        private int disappearEventIndex = 0;

        private void Start()
        {
            Timer = GameSet.Instance.Timer;
            MainCamera = Camera.main;

            // ��ȡ��Ļ�Ŀ��
            ScreenBottomLeft = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
            ScreenTopRight = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

            ScreenSize = ScreenTopRight - ScreenBottomLeft;
        }
        private void Update()
        {
            LineTime = Timer.GetElapsedTime() / 60 * LineInfo.bpm * 32;

            // �����ж��ߵ��ƶ��¼�
            HandleMoveEvents();

            // �����ж��ߵ���ת�¼�
            HandleRotateEvents();

            // �����ж��ߵ���ʧ�¼�
            HandleDisappearEvents();
        }
        private void HandleMoveEvents()
        {
            while (Test)
            {
                MoveEvent moveEvent = LineInfo.judgeLineMoveEvents[moveEventIndex];
                if (moveEvent.endTime <= LineTime)
                {
                    // ��ǰ�¼��ѹ��ڣ�����
                    moveEventIndex++;
                }
                else if (moveEvent.startTime <= LineTime)
                {
                    // ��ǰ�¼���Ŀ���¼�
                    HandleMoveEvent(moveEvent);
                    break;
                }
                else
                {
                    // ��ǰ�¼���δ��ʼ
                    moveEventIndex--;
                }
            }
        }
        private void HandleRotateEvents()
        {
            // ��������
            while (Test)
            {
                var rotateEvent = LineInfo.judgeLineRotateEvents[rotateEventIndex];
                if (rotateEvent.endTime <= LineTime)
                {
                    // ��ǰ�¼��ѹ��ڣ�����
                    rotateEventIndex++;
                }
                else if (rotateEvent.startTime <= LineTime)
                {
                    // ��ǰ�¼���Ŀ���¼�
                    HandleRotateEvent(rotateEvent);
                    break;
                }
                else
                {
                    // ��ǰ�¼���δ��ʼ
                    rotateEventIndex--;
                }
            }
        }
        private void HandleDisappearEvents()
        {
            // ��������
            while (Test)
            {
                var disappearEvent = LineInfo.judgeLineDisappearEvents[disappearEventIndex];
                if (disappearEvent.endTime <= LineTime)
                {
                    // ��ǰ�¼��ѹ��ڣ�����
                    disappearEventIndex++;
                }
                else if (disappearEvent.startTime <= LineTime)
                {
                    // ��ǰ�¼���Ŀ���¼�
                    HandleDisappearEvent(disappearEvent);
                    break;
                }
                else
                {
                    // ��ǰ�¼���δ��ʼ
                    disappearEventIndex--;
                }
            }
        }
        private void HandleMoveEvent(MoveEvent moveEvent)
        {
            Vector2 StartPos = new Vector2(moveEvent.start * ScreenSize.x, moveEvent.start2 * ScreenSize.y) + ScreenBottomLeft;
            Vector2 EndPos = new Vector2(moveEvent.end * ScreenSize.x, moveEvent.end2 * ScreenSize.y) + ScreenBottomLeft;

            float t = (LineTime - moveEvent.startTime) / (moveEvent.endTime - moveEvent.startTime);
            Vector2 CurrentPos = Vector2.Lerp(StartPos, EndPos, t);

            transform.localPosition = CurrentPos;
        }
        private void HandleRotateEvent(Event rotateEvent)
        {
            float StartRotation = rotateEvent.start;
            float EndRotation = rotateEvent.end;
            float t = (LineTime - rotateEvent.startTime) / (rotateEvent.endTime - rotateEvent.startTime);
            float CurrentRotation = Mathf.Lerp(StartRotation, EndRotation, t);
            transform.localRotation = Quaternion.Euler(0, 0, CurrentRotation);
        }
        private void HandleDisappearEvent(Event disappearEvent)
        {
            float StartAlpha = disappearEvent.start;
            float EndAlpha = disappearEvent.end;
            float t = (LineTime - disappearEvent.startTime) / (disappearEvent.endTime - disappearEvent.startTime);
            float CurrentAlpha = Mathf.Lerp(StartAlpha, EndAlpha, t);

            Color color = SpriteRenderer.color;
            color.a = CurrentAlpha;
            SpriteRenderer.color = color;
        }
    }
}