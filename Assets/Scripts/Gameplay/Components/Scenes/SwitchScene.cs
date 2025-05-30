using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Gameplay.Components.Scenes
{
    public static class SwitchScene
    {
        public static void Switch(string sceneName)
        {
            var transition = Object.FindObjectOfType<SceneTransition>();

            if (transition != null)
            {
                transition.SwitchScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}