using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Development
{
    public class FPS : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> fpsChange;
        [SerializeField] private UnityEvent<string> fpsChangeString;

        private int frameCountBefore;
        private WaitForSecondsRealtime timer;

        void Start()
        {
            timer = new WaitForSecondsRealtime(1);

            StartCoroutine(EverySconds());
        }
        private IEnumerator EverySconds()
        {
            while (true)
            {
                frameCountBefore = Time.frameCount;

                yield return timer;

                var fsp = Time.frameCount - frameCountBefore;

                fpsChange.Invoke(fsp);
                fpsChangeString.Invoke(fsp.ToString());
            }
        }
    }
}