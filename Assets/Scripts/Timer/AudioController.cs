using Cysharp.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
namespace PERM.Player
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource AudioSource;
        [SerializeField] private Timer Timer;
        public void Pause()
        {
            AudioSource.Pause();
        }
        public void Resume()
        {
            float AudioTime = Timer.GetElapsedTime() + Timer.DelayTime;
            if (AudioTime >= 0)
            {
                AudioSource.time = AudioTime;
                AudioSource.Play();
            }
        }
        public void SetRatio(float Ratio)
        {
            AudioSource.pitch = Ratio;
        }
        public async UniTaskVoid LoadAudio(string AudioPath)
        {
            try
            {
                AudioType AudioType = DetectAudioType(AudioPath);
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(AudioPath, AudioType))
                {
                    var operation = www.SendWebRequest();
                    await operation.ToUniTask();
                    if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError(www.error);
                    }
                    else
                    {
                        AudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                        Timer.Length = AudioSource.clip.length;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading audio: {ex.Message}");
            }
        }
        AudioType DetectAudioType(string AudioPath)
        {
            string Extension = Path.GetExtension(AudioPath).ToLower();
            switch (Extension)
            {
                case ".mp3":
                    return AudioType.MPEG;
                case ".wav":
                    return AudioType.WAV;
                case ".ogg":
                    return AudioType.OGGVORBIS;
                default:
                    return AudioType.UNKNOWN;
            }
        }
    }
}