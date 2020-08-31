using System;

[Serializable]
public class GameData
{
    internal bool backgroundMusicEnable;
    internal float backgroundMusicVolume;
    internal bool soundEffectEnable;
    internal float soundEffectVolume;
    internal int currentTargetEfficiency;

    public GameData()
    {
        backgroundMusicEnable = GameInfo.BackgroundMusicEnable;
        backgroundMusicVolume = GameInfo.BackgroundMusicVolume;
        soundEffectEnable = GameInfo.SoundEffectsEnable;
        soundEffectVolume = GameInfo.SoundEffectsVolume;
        currentTargetEfficiency = GameInfo.CurrentTargetEfficiency;
    }
}
