using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationUtil
{
    //By default game level is easy
    static public float GameSoundVolume { get; set; } = 0.25f;
    static public float GameMusicVolume { get; set; } = 0.25f;
    static public bool FirstLevel { get; set; } = true;
    static public int CurrentSceneIndex { get; set; } = 0;
    static public int CurrentLevel { get; set; } = 0;
    static public bool GamePaused { get; set; } = false;
    static public string PlayerName { get; set; } = String.Empty;
    static public int MaximumLevel { get; set; } = 4;


}
