using System.Collections;
using Scripts.Extension;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Gameplay.Components.Scenes
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private float xPosHidden;
        [SerializeField] private float xPosShow;
        [SerializeField] private float xPosToHide;
        [SerializeField] private float speed = 5f;

        private AsyncOperation loadingSceneOperation;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        private IEnumerator Start()
        {
            rectTransform.anchoredPosition = rectTransform.anchoredPosition.NewX(xPosHidden);

            for (int i = 0; i < 3; i++)
            {
                yield return null;
            }


            StartCoroutine(AnimateBar(xPosShow));
        }

        public void SwitchScene(string sceneName)
        {
            rectTransform.anchoredPosition = rectTransform.anchoredPosition.NewX(xPosToHide);
            StartCoroutine(AnimateBar(xPosHidden));

            loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            loadingSceneOperation.allowSceneActivation = false;
        }
        private IEnumerator AnimateBar(float toX)
        {
            var progress = 0f;
            var startX = rectTransform.anchoredPosition.x;
            do
            {
                progress += speed * Time.unscaledDeltaTime;
                var newX = Mathf.Lerp(startX, toX, progress);
                rectTransform.anchoredPosition = rectTransform.anchoredPosition.NewX(newX);

                yield return null;

            } while (progress <= 1);

            OnAnimationOver();
        }
        public void OnAnimationOver()
        {
            if (loadingSceneOperation != null)
                loadingSceneOperation.allowSceneActivation = true;
        }
    }
}