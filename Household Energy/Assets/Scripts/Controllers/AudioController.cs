using System.Collections.Generic;
using UnityEngine;

public enum AnswerType
{
    WRONG = 0,
    CORRECT = 1
}

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip backgroundAudioClip;

    [SerializeField]
    private List<AudioClip> soundEffectsAudioClip;

    internal AudioSource BackgroundAudioSource { get; set; }
    internal AudioSource SoundEffectsAudioSource { get; set; }

    private void Awake()
    {
        if (backgroundAudioClip != null)
        {
            BackgroundAudioSource = gameObject.AddComponent<AudioSource>();
            BackgroundAudioSource.clip = backgroundAudioClip;
            BackgroundAudioSource.loop = true;
            BackgroundAudioSource.priority = 200;
        }

        if (soundEffectsAudioClip != null && soundEffectsAudioClip.Count > 0)
        {
            SoundEffectsAudioSource = gameObject.AddComponent<AudioSource>();
            SoundEffectsAudioSource.priority = 100;
            SoundEffectsAudioSource.loop = false;
            SoundEffectsAudioSource.playOnAwake = false;
        }

        UpdateBackgroundMusicEnable();
        UpdateBackgroundMusicVolume();
        UpdateSoundEffectsVolume();
    }

    internal void UpdateBackgroundMusicEnable()
    {
        if (GameInfo.BackgroundMusicEnable)
            BackgroundAudioSource.Play();
        else
            BackgroundAudioSource.Stop();
    }

    internal void UpdateBackgroundMusicVolume()
    {
        BackgroundAudioSource.volume = GameInfo.BackgroundMusicVolume;
    }

    internal void UpdateSoundEffectsVolume()
    {
        if (SoundEffectsAudioSource != null)
            SoundEffectsAudioSource.volume = GameInfo.SoundEffectsVolume;
    }

    internal void PlaySoundEffects(AnswerType answerType)
    {
        if (SoundEffectsAudioSource != null)
        {
            if (GameInfo.SoundEffectsEnable) SoundEffectsAudioSource.PlayOneShot(soundEffectsAudioClip[(int)answerType]);
        }
    }
}
