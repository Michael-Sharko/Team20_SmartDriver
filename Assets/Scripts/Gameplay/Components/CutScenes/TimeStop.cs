using System.Collections;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public void Do(float seconds)
    {
        StartCoroutine(Anim(seconds));
    }
    private IEnumerator Anim(float seconds)
    {
        var startTime = Time.unscaledTime;
        var finishTime = Time.unscaledTime + seconds;
        while (Time.unscaledTime < finishTime)
        {
            Time.timeScale = 1 - Mathf.InverseLerp(startTime, finishTime, Time.unscaledTime);

            yield return null;
        }
        Time.timeScale = 0;
    }
}