using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace PERM.Player
{
    public class FPS : MonoBehaviour
    {
        private float CurrentFPS; // ʵʱ֡��
        private const float Interval = 5f; // ����ƽ��֡�ʵ�ʱ����
        private float ElapsedTime = 0f; // �ۼ�ʱ��
        private Queue<float> FPSQueue = new Queue<float>(); // �洢֡������

        [SerializeField] private Text FPSDisplay;
        
        void Update()
        {
            // ����ʵʱ֡��
            CurrentFPS = 1f / Time.deltaTime;

            // �����ۼ�ʱ��
            ElapsedTime += Time.deltaTime;

            // ����ǰ֡�ʼ������
            FPSQueue.Enqueue(CurrentFPS);

            // �Ƴ����������֡������
            while (ElapsedTime > Interval && FPSQueue.Count > 0)
            {
                FPSQueue.Dequeue();
                ElapsedTime -= Time.deltaTime;
            }

            // ��������ƽ��֡��
            float AverageFPS = CalculateAverageFPS();

            // ���֡����Ϣ����ѡ��
            FPSDisplay.text = $"CurrentFPS : {CurrentFPS:F2}\nAverageFPS : {AverageFPS:F2}";
        }

        private float CalculateAverageFPS()
        {
            if (FPSQueue.Count == 0) return 0f;

            float sum = 0f;
            foreach (float fps in FPSQueue)
            {
                sum += fps;
            }
            return sum / FPSQueue.Count;
        }
    }
}