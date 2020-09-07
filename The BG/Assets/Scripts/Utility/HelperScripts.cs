public enum GameLevels
{
    Easy = 0,
    Hard,
    Zombie
}

public class ApplicationUtil
{
    //By default game level is easy
    static public GameLevels GameLevel { get; set; } = 0;
    static public float GameVolume { get; set; } = 0.25f;
    static public bool GameMusic { get; set; } = true;
    static public bool FirstLevel { get; set; } = true;
    static public int CurrentSceneIndex { get; set; } = 0;
    static public bool GamePaused { get; set; } = false;
}

public class Axis
{
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";
}
