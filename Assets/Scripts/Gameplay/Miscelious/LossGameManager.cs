﻿using System;
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
    [SerializeField] GameObject carPanel;
    [SerializeField] GameObject lossPanel1;
    [SerializeField] GameObject lossPanel2;

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

    private MonoBehaviour _coroutineOwner;

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

    public void Init(MonoBehaviour coroutineOwner)
    {
        _coroutineOwner = coroutineOwner;
    }

    private void SubscribeCarEvents()
    {
        Car.CarStrength.OnCarBroken += HandleCarBroken;
        Car.CarFuel.OnCarFuelRanOut += HandleCarFuelRanOut;
    }

    private void UnsubscribeCarEvents()
    {
        Car.CarStrength.OnCarBroken -= HandleCarBroken;
        Car.CarFuel.OnCarFuelRanOut -= HandleCarFuelRanOut;
    }

    private void HandleCarFuelRanOut()
    {
        HandleCarEvent("Топливо закончилось!");
    }

    private void HandleCarBroken()
    {
        HandleCarEvent("Машина сломана!");
    }

    private void HandleCarEvent(string message)
    {
        Cursor.lockState = CursorLockMode.None;

        SetMessage(message);

        _coroutineOwner.StartCoroutine(AnimateMessages());

        ShowLossPanel();

        IsInvoked = true;
    }
    private void ShowLossPanel()
    {
        carPanel.Off();
        lossPanel1.On();
        lossPanel2.On();
    }
    public void HideLossPanel()
    {
        lossPanel1.Off();
        lossPanel2.Off();
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