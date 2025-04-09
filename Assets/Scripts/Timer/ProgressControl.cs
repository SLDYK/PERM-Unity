using UnityEngine;
using UnityEngine.EventSystems;
namespace PERM.Player
{
    public class ProgressControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Timer Timer;

        private bool isPointerDown = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerDown = false;
        }

        private void Update()
        {
            // 拖动进度条逻辑
            if (isPointerDown)
            {
                Vector2 pointerPosition = Input.mousePosition;

                float relativePosition = pointerPosition.x / Screen.width;
                Timer.SetTimer(relativePosition * Timer.Length);
            }
            // 鼠标滚轮调整时间逻辑
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) > 0.01f) // 检测滚轮滚动
            {
                float adjustment = scrollDelta * Timer.Length * 0.05f; // 调整幅度为总长度的5%
                float newTime = Mathf.Clamp(Timer.GetElapsedTime() + adjustment, 0, Timer.Length); // 确保时间在合法范围内
                Timer.SetTimer(newTime);
            }
        }
    }
}