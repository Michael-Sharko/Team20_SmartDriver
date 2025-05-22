using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneInContextMenu
{
    private const string GENERATED_SCRIPT_PATH = "/Scripts/Development/Editor/SceneInContextMenu_Generated.cs";

    [MenuItem("Scene/Refresh", false, 0)]
    public static void Refresh()
    {
        var path = Application.dataPath + GENERATED_SCRIPT_PATH;

        StringBuilder stringBuilder = new();

        stringBuilder.Append($@"
// =================================================================
//
//               THIS CODE IS GENERATED AUTOMATICALLY
//                         DO NOT EDIT
//
// =================================================================

using UnityEditor.SceneManagement;
using UnityEditor;

public class {nameof(SceneInContextMenu)}_Generated {{
");
        var scenes = GetListScenesPaths();

        for (int i = 0; i < scenes.Count; i++)
        {
            var name = PathToName(scenes[i]);
            stringBuilder.Append($@"
    [MenuItem(""Scene/{name}"")]
    public static void Scene_{i + 1}() 
    {{
        EditorSceneManager.OpenScene(""{scenes[i]}"", OpenSceneMode.Single);
    }}
");
        }
        stringBuilder.Append("}");

        File.WriteAllText(path, stringBuilder.ToString());

        AssetDatabase.Refresh();
    }
    private static List<string> GetListScenesPaths()
    {
        var tmp = new List<string>();

        for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            tmp.Add(scenePath);
        }
        return tmp;
    }
    private static string PathToName(string path)
    {
        var lastSlash = path.LastIndexOf("/", StringComparison.Ordinal);
        var name = path.Substring(lastSlash + 1, path.LastIndexOf(".", StringComparison.Ordinal) - lastSlash - 1);
        return name;
    }
}