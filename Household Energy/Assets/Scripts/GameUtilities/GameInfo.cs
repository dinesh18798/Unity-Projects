using System.Collections.Generic;

public class GameInfo
{
    private static float backgroundMusicVolume = 0.5f;
    private static float soundEffectsVolume = 0.5f;
    private static bool backgroundMusicEnable = true;
    private static bool soundEffectsEnable = true;

    static public float BackgroundMusicVolume
    {
        get { return backgroundMusicVolume; }
        set
        {
            if (value != backgroundMusicVolume)
            {
                backgroundMusicVolume = value;
                SaveAndLoadManager.SaveGameData();
            }
        }
    }

    static public float SoundEffectsVolume
    {
        get { return soundEffectsVolume; }
        set
        {
            if (value != soundEffectsVolume)
            {
                soundEffectsVolume = value;
                SaveAndLoadManager.SaveGameData();
            }
        }
    }

    static public bool BackgroundMusicEnable
    {
        get { return backgroundMusicEnable; }
        set
        {
            if (value != backgroundMusicEnable)
            {
                backgroundMusicEnable = value;
                SaveAndLoadManager.SaveGameData();
            }
        }
    }

    static public bool SoundEffectsEnable
    {
        get { return soundEffectsEnable; }
        set
        {
            if (value != soundEffectsEnable)
            {
                soundEffectsEnable = value;
                SaveAndLoadManager.SaveGameData();
            }
        }
    }

    public static bool IsNewGame { get; set; } = false;

    public static int CurrentSceneIndex { get; internal set; }

    public static bool GamePaused { get; internal set; }

    public static int CurrentTargetEfficiency { get; set; } = 60;

    public static int MinTargetEfficiency { get; set; } = 60;

    public static int MaxTargetEfficiency { get; set; } = 90;
}

public class PlayerInfo
{
    static public string PlayerName { get; set; } = string.Empty;

    static public int PlayerCharacterID { get; set; } = 0;

    static public int Coins { get; set; } = 1000;

    static public int QuizCurrentLevel { get; set; } = 1;

    static public int MaxQuizLevel { get; } = 15;

    static public int PuzzleCurrentLevel { get; set; } = 1;

    static public int MaxPuzzleLevel { get; } = 4;

    static public Dictionary<string, ApplianceInfo> PurchasedAppliances { get; set; } = new Dictionary<string, ApplianceInfo>();

    static public Dictionary<string, UtilityInfo> PurchasedUtilities { get; set; } = new Dictionary<string, UtilityInfo>();

    static public List<Appliance> AllAppliancesList { get; set; } = new List<Appliance>();

    static public List<Utility> AllUtilitiesList { get; set; } = new List<Utility>();
}