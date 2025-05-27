using UnityEditor;
using UnityEngine.SceneManagement;

public class ReloadScene
{
    [MenuItem("Tools/ReloadScene #&r")]
    private static void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}