using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;
using System.Linq;

namespace PERM.Player
{
    public class AudioPool : MonoBehaviour
    {
        [SerializeField] private AudioClip Tap;
        [SerializeField] private AudioClip Drag;
        [SerializeField] private AudioClip Flick;
        [SerializeField] private AudioSource AudioSource;
        public void PlayTap(float volume)
        {
            PlayClip(Tap, volume).Forget();
        }
        public void PlayDrag(float volume)
        {
            PlayClip(Drag, volume).Forget();
        }
        public void PlayFlick(float volume)
        {
            PlayClip(Flick, volume).Forget();
        }
        private async UniTaskVoid PlayClip(AudioClip Clip, float Volume)
        {
            AudioSource.PlayOneShot(Clip, Volume);
            await UniTask.Yield();
        }
    }
}