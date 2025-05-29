using Scripts.Gameplay.Tags;
using UnityEngine;

public static class Tags
{
    public static bool TryGetTag<T>(out T tag) where T : Tag
    {
        tag = Object.FindObjectOfType<T>();

        if (!tag)
            Debug.LogError($"В сцене нет объекта помеченного тегом {typeof(T).Name}");

        return tag;
    }
}