using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int sceneIndex;
    public int gameLevel;
    public float gameVolume;
    public bool gameMusic;

    public GameData()
    {
        sceneIndex = ApplicationUtil.CurrentSceneIndex;
        gameLevel = (int)ApplicationUtil.GameLevel;
        gameVolume = ApplicationUtil.GameVolume;
        gameMusic = ApplicationUtil.GameMusic;
    }
}
