using UnityEngine;

public interface IAudioService
{
    void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f);
    void PlayMusic(AudioClip clip, bool loop = true);
    void SetSfxVolume(float linear01);
    void SetMusicVolume(float linear01);
}
