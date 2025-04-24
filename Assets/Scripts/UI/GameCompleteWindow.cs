using System.Collections;
using TMPro;
using UnityEngine;

public class GameCompleteWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeFrequency;

    private void OnEnable()
    {
        StartCoroutine(AnimateText());
    }
    private IEnumerator AnimateText()
    {
        while (true)
        {
            text.alpha = CalculateNewAlpha();
            yield return null;
        }
    }

    private float CalculateNewAlpha()
    {
        return Mathf.Lerp(0, 1, AnimationFormula(Time.time));
    }

    private float AnimationFormula(float time)
    {
        return (Mathf.Sin(2 * Mathf.PI * fadeFrequency * time) + 1) / 2;
    }
}
