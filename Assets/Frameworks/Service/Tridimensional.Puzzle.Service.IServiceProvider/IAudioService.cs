using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IAudioService
	{
        AudioSource Play(AudioClip audioClip, Transform emitter, float volume, float pitch);
    }
}
