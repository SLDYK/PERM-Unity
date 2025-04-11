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
        private Vector2 ScreenSize;
        private Vector2 ScreenBottomLeft;
        private Vector2 ScreenTopRight;

        public float LineFloor = 0;
        public float LineTime;

        // 为每种事件维护下标
        private int moveEventIndex = 0;
        private int rotateEventIndex = 0;
        private int disappearEventIndex = 0;
        private int floorEventIndex = 0;

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

            HandleMoveEvents();
            HandleRotateEvents();
            HandleDisappearEvents();
            HandleFloorEvents();
        }
        private void HandleMoveEvents()
        {
            while (Test)
            {
                MoveEvent moveEvent = LineInfo.MoveEvents[moveEventIndex];
                if (moveEvent.endTime <= LineTime)
                {
                    // 当前事件已过期，跳过
                    moveEventIndex++;
                }
                else if (moveEvent.startTime <= LineTime)
                {
                    // 当前事件是目标事件
                    HandleMoveEvent(moveEvent);
                    break;
                }
                else
                {
                    // 当前事件尚未开始
                    moveEventIndex--;
                }
            }
        }
        private void HandleRotateEvents()
        {
            // 后向搜索
            while (Test)
            {
                var rotateEvent = LineInfo.RotateEvents[rotateEventIndex];
                if (rotateEvent.endTime <= LineTime)
                {
                    // 当前事件已过期，跳过
                    rotateEventIndex++;
                }
                else if (rotateEvent.startTime <= LineTime)
                {
                    // 当前事件是目标事件
                    HandleRotateEvent(rotateEvent);
                    break;
                }
                else
                {
                    // 当前事件尚未开始
                    rotateEventIndex--;
                }
            }
        }
        private void HandleDisappearEvents()
        {
            // 后向搜索
            while (Test)
            {
                var disappearEvent = LineInfo.DisappearEvents[disappearEventIndex];
                if (disappearEvent.endTime <= LineTime)
                {
                    // 当前事件已过期，跳过
                    disappearEventIndex++;
                }
                else if (disappearEvent.startTime <= LineTime)
                {
                    // 当前事件是目标事件
                    HandleDisappearEvent(disappearEvent);
                    break;
                }
                else
                {
                    // 当前事件尚未开始
                    disappearEventIndex--;
                }
            }
        }
        private void HandleFloorEvents()
        {
            // 后向搜索
            while (Test)
            {
                var floorEvent = LineInfo.FloorEvents[floorEventIndex];
                if (floorEvent.endTime <= LineTime)
                {
                    // 当前事件已过期，跳过
                    floorEventIndex++;
                }
                else if (floorEvent.startTime <= LineTime)
                {
                    // 当前事件是目标事件
                    HandleFloorEvent(floorEvent);
                    break;
                }
                else
                {
                    // 当前事件尚未开始
                    floorEventIndex--;
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
        private void HandleFloorEvent(Event floorEvent)
        {
            float StartFloor = floorEvent.start;
            float EndFloor = floorEvent.end;
            float t = (LineTime - floorEvent.startTime) / (floorEvent.endTime - floorEvent.startTime);
            float CurrentFloor = Mathf.Lerp(StartFloor, EndFloor, t);
            LineFloor = CurrentFloor;
        }
    }
}