using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
namespace PERM.Player
{
    public class FPSCounter : MonoBehaviour
    {
        private float CurrentFPS = 0f; // 实时帧率
        private float AverageFPS = 0f; // 平均帧率
        private const float Interval = 5f;
        private Queue<float> FPSQueue = new Queue<float>();

        [SerializeField] private Text FPSDisplay;
        
        void Update()
        {
            float Delta = Time.deltaTime;

            FPSQueue.Enqueue(Delta);
            while (FPSQueue.Sum() > Interval) 
            {
                FPSQueue.Dequeue();
            }

            CurrentFPS = 1f / Delta;
            AverageFPS = CalculateAverageFPS();

            FPSDisplay.text = $"CurrentFPS : {CurrentFPS:F1}\nAverageFPS : {AverageFPS:F1}";
        }

        private float CalculateAverageFPS()
        {
            return FPSQueue.Count / Interval;
        }
    }
}