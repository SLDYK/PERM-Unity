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
            Chart = JsonUtility.FromJson<Chart>(json);
        }
    }
}
