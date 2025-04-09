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
            // �϶��������߼�
            if (isPointerDown)
            {
                Vector2 pointerPosition = Input.mousePosition;

                float relativePosition = pointerPosition.x / Screen.width;
                Timer.SetTimer(relativePosition * Timer.Length);
            }
            // �����ֵ���ʱ���߼�
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) > 0.01f) // �����ֹ���
            {
                float adjustment = scrollDelta * Timer.Length * 0.05f; // ��������Ϊ�ܳ��ȵ�5%
                float newTime = Mathf.Clamp(Timer.GetElapsedTime() + adjustment, 0, Timer.Length); // ȷ��ʱ���ںϷ���Χ��
                Timer.SetTimer(newTime);
            }
        }
    }
}