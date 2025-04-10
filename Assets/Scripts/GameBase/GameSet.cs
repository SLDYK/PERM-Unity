using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace PERM.Player
{
    public class GameSet : MonoBehaviour
    {
        [Header("Components")]
        public Timer Timer;
        public Text TimerDisplay;
        public AudioPool AudioPool;
        public AudioController AudioController;
        public LineCreator LineCreator;
        public Transform NoteStage;

        [Header("Settings")]
        public string TrackPath;

        [Header("TrackObjects")]
        public Chart Chart;
        public PhiChart PhiChart;

        void Start()
        {
            //获取路径
            string ChartPath = TrackPath + "/Chart.json";
            string AudioPath = TrackPath + "/Audio.wav";
            string SpritePath = TrackPath + "/Image.png";
            //加载音频
            AudioController.LoadAudio(AudioPath).Forget();
            //加载谱面
            string json = File.ReadAllText(ChartPath);
            PhiChart = JsonUtility.FromJson<PhiChart>(json);

            Chart = ChartConvert.CreateChart(PhiChart);

            //调用其他组件的 start
            LineCreator.LineCreatorStart();
        }
        // 单例实例
        private static GameSet instance;

        // 公共静态属性访问单例实例
        public static GameSet Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<GameSet>();
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<GameSet>();
                        singletonObject.name = typeof(GameSet).ToString() + " (Singleton)";
                    }
                }
                return instance;
            }
        }
        // 确保只有一个实例存在
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
