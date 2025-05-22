#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Components.Triggers.Editor
{
    //[InitializeOnLoad]
    public static class TriggersInContextMenu
    {
        //static TriggersInContextMenu()
        //{
            //SceneManager.activeSceneChanged += EditorSceneManager_sceneLoaded;
        //}

        [MenuItem("Tools/Triggers/ShowVisual")]
        private static void ShowVisual()
        {
            ChangeVisualState(true);
        }
        [MenuItem("Tools/Triggers/HideVisual")]
        private static void HideVisual()
        {
            ChangeVisualState(false);
        }
        private static void ChangeVisualState(bool show)
        {
            var objects = Object.FindObjectsByType<TriggerBase>(FindObjectsSortMode.None);

            foreach (var obj in objects)
            {
                obj.ShowVisual = show;
                obj.GetComponent<Renderer>().enabled = show;
            }
        }
    }
}
#endif