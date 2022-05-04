using Core.Audio;
using UnityEngine;

namespace DefaultNamespace
{
    public class PenaltiesAudio : MonoBehaviour
    {
        [SerializeField] AudioContext audio;
        [SerializeField] AudioClip cheerClip;
        [SerializeField] AudioClip kickClip;
        [SerializeField] AudioClip ambientClip;
        
        public void PlayCheer() => audio.PlayFxSound(cheerClip);
        public void PlayKick() => audio.PlayFxSound(kickClip);

        void Start() => audio.PlayLoopeableFxSound(ambientClip);
    }
}