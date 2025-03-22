using System;
using System.Collections;
using Shark.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LossGameManager
{
    public CarController Car { set; private get; }
    public bool CarInitilized => Car != null;
    public bool IsInvoked { get; private set; }

    [SerializeField] UnityEvent OnEndGameInvoked;

    [Serializable]
    struct AnimationSettings
    {
        public float minAlpha, maxAlpha;
        public float fadeFrequency;
    }
    [SerializeField]
    private AnimationSettings settings;

    [SerializeField] CanvasGroup _panel;
    [SerializeField] TextMeshProUGUI _message;
    [SerializeField] TextMeshProUGUI _hintToMenu;

    private MonoBehaviour _monoBehaviour;

    public void SubscribeEvents()
    {
        if (CarInitilized)
            SubscribeCarEvents();
    }

    public void UnsubscribeEvents()
    {
        if (CarInitilized)
            UnsubscribeCarEvents();
    }

    public void Init(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    private void SubscribeCarEvents()
    {
        Car.OnCarBroken += HandleCarBroken;
        Car.OnCarFuelRanOut += HandleCarFuelRanOut;
    }

    private void UnsubscribeCarEvents()
    {
        Car.OnCarBroken -= HandleCarBroken;
        Car.OnCarFuelRanOut -= HandleCarFuelRanOut;
    }

    private void HandleCarFuelRanOut()
    {
        HandleCarEvent("Car fuel ran out!");
    }

    private void HandleCarBroken()
    {
        HandleCarEvent("Car broken!");
    }

    private void HandleCarEvent(string message)
    {
        SetMessage(message);

        _monoBehaviour.StartCoroutine(AnimateMessages());

        OnEndGameInvoked?.Invoke();
        IsInvoked = true;
    }

    private void SetMessage(string message)
    {
        if (_message != null)
            _message.text = message;
    }

    private IEnumerator AnimateMessages()
    {
        while (true)
        {
            _panel.alpha = CalculateNewAlpha();
            yield return null;
        }
    }

    private float CalculateNewAlpha()
    {
        return Mathf.Lerp(settings.minAlpha, settings.maxAlpha, AnimationFormula(Time.time));
    }

    private float AnimationFormula(float time)
    {
        return (Mathf.Sin(2 * Mathf.PI * settings.fadeFrequency * time) + 1) / 2;
    }
}