using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class AudioService : IAudioService
    {
        public AudioSource Play(AudioClip audioClip, Transform emitter, float volume, float pitch)
        {
            var go = new GameObject("Audio");
            go.transform.position = emitter.position;
            go.transform.parent = emitter;

            var audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();

            return audioSource;
        }
    }
}
