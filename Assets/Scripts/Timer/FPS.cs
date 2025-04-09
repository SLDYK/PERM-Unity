using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace PERM.Player
{
    public class FPS : MonoBehaviour
    {
        private float CurrentFPS; // 实时帧率
        private const float Interval = 5f; // 计算平均帧率的时间间隔
        private float ElapsedTime = 0f; // 累计时间
        private Queue<float> FPSQueue = new Queue<float>(); // 存储帧率数据

        [SerializeField] private Text FPSDisplay;
        
        void Update()
        {
            // 计算实时帧率
            CurrentFPS = 1f / Time.deltaTime;

            // 更新累计时间
            ElapsedTime += Time.deltaTime;

            // 将当前帧率加入队列
            FPSQueue.Enqueue(CurrentFPS);

            // 移除超过五秒的帧率数据
            while (ElapsedTime > Interval && FPSQueue.Count > 0)
            {
                FPSQueue.Dequeue();
                ElapsedTime -= Time.deltaTime;
            }

            // 计算五秒平均帧率
            float AverageFPS = CalculateAverageFPS();

            // 输出帧率信息（可选）
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