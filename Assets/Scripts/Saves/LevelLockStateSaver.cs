using UnityEngine;

public class LevelLockStateSaver
{
    public bool LevelIsUnlocked(string levelName)
        => PlayerPrefs.HasKey(levelName);
}