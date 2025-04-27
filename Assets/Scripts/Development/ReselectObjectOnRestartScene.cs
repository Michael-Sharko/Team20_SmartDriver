#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Development
{
    [InitializeOnLoad]
    public static class ReselectObjectOnRestartScene
    {
        private static string selectedName;

        static ReselectObjectOnRestartScene()
        {
            EditorSceneManager.sceneLoaded += EditorSceneManager_sceneLoaded;
            Selection.selectionChanged += SelectionChanged;

        }
        private static void SelectionChanged()
        {
            if (Selection.activeGameObject != null)
                selectedName = Selection.activeGameObject.name;
        }

        private static void EditorSceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Selection.activeGameObject = GameObject.Find(selectedName);
        }
    }
}
#endif