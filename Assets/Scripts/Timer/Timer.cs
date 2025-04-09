using UnityEngine;
using UnityEngine.UI;
namespace PERM.Player
{
    public class Timer : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private AudioController AudioController;
        [SerializeField] private Text TimerDisplay;

        private float StartTime;
        private float TargetTime;
        public float Length;
        private float ElapsedTime;

        public bool Paused = false;
        public float DelayTime = 0;
        public float Multiplier = 1;

        private bool AudioForward = false;
        void Start()
        {
            StartTime = Time.time;
            ElapsedTime = Time.time - StartTime;
            PauseTimer();
        }
        void Update()
        {
            if (!Paused)
            {
                ElapsedTime = (Time.time - StartTime) * Multiplier;
                TargetTime = ElapsedTime;
                AudioTimer();
                TimerDisplay.text = $"TrackTime : {ElapsedTime}\nAudioTime : {ElapsedTime + DelayTime}";
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Paused)
                {
                    ResumeTimer();
                }
                else
                {
                    PauseTimer();
                }
            }
        }
        private void FixedUpdate()
        {
            if (Paused)
            {
                ElapsedTime += (TargetTime - ElapsedTime) / 5;
            }
        }
        public void SetRatio(float Ratio)
        {
            PauseTimer();
            Multiplier = Ratio;
            AudioController.SetRatio(Ratio);
        }
        public void PauseTimer()
        {
            Paused = true;
            AudioController.Pause();
        }
        public void ResumeTimer()
        {
            Paused = false;
            StartTime = Time.time - ElapsedTime / Multiplier;
            AudioController.Resume();
        }
        public void SetTimer(float SetTime)
        {
            PauseTimer();
            TargetTime = Mathf.Max(SetTime, 0);
        }
        private void AudioTimer()
        {
            float AudioTime = ElapsedTime + DelayTime;
            bool AudioCross = AudioTime >= 0;
            if (AudioCross != AudioForward)
            {
                AudioController.Resume();
            }
            AudioForward = AudioCross;
        }
        public float GetElapsedTime()
        {
            return ElapsedTime;
        }
    }
}