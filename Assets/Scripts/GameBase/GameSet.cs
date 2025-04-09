using Cysharp.Threading.Tasks;
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

        [Header("Settings")]
        [SerializeField] private string TrackPath;

        public Chart Chart;

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
            Chart = JsonUtility.FromJson<Chart>(json);
        }
    }
}
