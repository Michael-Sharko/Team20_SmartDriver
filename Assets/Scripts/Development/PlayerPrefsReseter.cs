
// Не помещать этот скрипт в папку Editor
// он перестает работать

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class PlayerPrefsReseter
{
    static PlayerPrefsReseter()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(Build);
    }
    private static void Build(BuildPlayerOptions buildOptions)
    {
        UnityEngine.PlayerPrefs.DeleteAll();
    }
}
#endif