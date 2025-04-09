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

        private Camera MainCamera;
        private float LineTime;
        private Vector2 ScreenSize;
        private Vector2 ScreenBottomLeft;
        private Vector2 ScreenTopRight;

        private void Start()
        {
            Timer = GameSet.Instance.Timer;
            MainCamera = Camera.main;

            // 获取屏幕的宽高
            ScreenBottomLeft = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
            ScreenTopRight = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

            ScreenSize = ScreenTopRight - ScreenBottomLeft;
        }
        private void Update()
        {
            LineTime = Timer.GetElapsedTime() / 60 * LineInfo.bpm * 32;

            // 处理判定线的移动事件
            List<UniTask> moveTasks = LineInfo.judgeLineMoveEvents
                .Select(moveEvent => HandleMoveEvent(moveEvent))
                .ToList();

            // 处理判定线的旋转事件
            List<UniTask> rotateTasks = LineInfo.judgeLineRotateEvents
                .Select(rotateEvent => HandleRotateEvent(rotateEvent))
                .ToList();

            // 处理判定线的消失事件
            List<UniTask> disappearTasks = LineInfo.judgeLineDisappearEvents
                .Select(disappearEvent => HandleDisappearEvent(disappearEvent))
                .ToList();

            // 不等待所有任务完成
            UniTask.WhenAll(moveTasks.Concat(rotateTasks).Concat(disappearTasks)).Forget();
        }
        private async UniTask HandleMoveEvent(MoveEvent moveEvent)
        {
            if (moveEvent.startTime <= LineTime && moveEvent.endTime > LineTime)
            {
                // 处理判定线的移动事件
                Vector2 StartPos = new Vector2(moveEvent.start * ScreenSize.x, moveEvent.start2 * ScreenSize.y) + ScreenBottomLeft;
                Vector2 EndPos = new Vector2(moveEvent.end * ScreenSize.x, moveEvent.end2 * ScreenSize.y) + ScreenBottomLeft;

                float t = (LineTime - moveEvent.startTime) / (moveEvent.endTime - moveEvent.startTime);
                Vector2 CurrentPos = Vector2.Lerp(StartPos, EndPos, t);

                transform.localPosition = CurrentPos;
            }
            await UniTask.Yield();
        }
        private async UniTask HandleRotateEvent(Event rotateEvent)
        {
            if (rotateEvent.startTime <= LineTime && rotateEvent.endTime > LineTime)
            {
                float StartRotation = rotateEvent.start;
                float EndRotation = rotateEvent.end;
                float t = (LineTime - rotateEvent.startTime) / (rotateEvent.endTime - rotateEvent.startTime);
                float CurrentRotation = Mathf.Lerp(StartRotation, EndRotation, t);
                transform.localRotation = Quaternion.Euler(0, 0, CurrentRotation);
            }
            await UniTask.Yield();
        }
        private async UniTask HandleDisappearEvent(Event disappearEvent)
        {
            if (disappearEvent.startTime <= LineTime && disappearEvent.endTime > LineTime)
            {
                float StartAlpha = disappearEvent.start;
                float EndAlpha = disappearEvent.end;
                float t = (LineTime - disappearEvent.startTime) / (disappearEvent.endTime - disappearEvent.startTime);
                float CurrentAlpha = Mathf.Lerp(StartAlpha, EndAlpha, t);

                Color color = SpriteRenderer.color;
                color.a = CurrentAlpha;
                SpriteRenderer.color = color;
            }
            await UniTask.Yield();
        }
    }
}