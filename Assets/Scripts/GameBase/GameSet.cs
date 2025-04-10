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
            //��ȡ·��
            string ChartPath = TrackPath + "/Chart.json";
            string AudioPath = TrackPath + "/Audio.wav";
            string SpritePath = TrackPath + "/Image.png";
            //������Ƶ
            AudioController.LoadAudio(AudioPath).Forget();
            //��������
            string json = File.ReadAllText(ChartPath);
            PhiChart = JsonUtility.FromJson<PhiChart>(json);

            Chart = ChartConvert.CreateChart(PhiChart);

            //������������� start
            LineCreator.LineCreatorStart();
        }
        // ����ʵ��
        private static GameSet instance;

        // ������̬���Է��ʵ���ʵ��
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
        // ȷ��ֻ��һ��ʵ������
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
