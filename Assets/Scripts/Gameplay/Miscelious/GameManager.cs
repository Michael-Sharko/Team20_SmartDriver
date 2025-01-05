using Shark.Gameplay.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string _mainMenuSceneName = "Main Menu";

    [Serializable]
    private class PauseManager
    {
        [SerializeField] UnityEvent OnPauseActivated;
        [SerializeField] UnityEvent OnPauseDeactivated;

        private bool _isPaused = false;

        public void TogglePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
            (isPaused ? OnPauseActivated : OnPauseDeactivated)?.Invoke();

            _isPaused = isPaused;
        }

        public void TogglePause()
        {
            TogglePause(!_isPaused);
        }
    }
    [SerializeField]
    private PauseManager _pauseManager;

    [Serializable]
    private class EndGameManager
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

        [SerializeField] Transform _panel;
        [SerializeField] TextMeshProUGUI _message;
        [SerializeField] TextMeshProUGUI _hintToMenu;

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

            OnEndGameInvoked?.Invoke();
            IsInvoked = true;
        }

        private void SetMessage(string message)
        {
            if (_message != null)
                _message.text = message;
        }

        public void AnimateMessages()
        {
            if (_panel != null) 
                UpdateAlphaRecursive(_panel, CalculateNewAlpha());
        }

        private void UpdateAlphaRecursive(Transform parent, float alpha)
        {
            if (parent.TryGetComponent(out CanvasRenderer renderer))
                renderer.SetAlpha(alpha);

            for (int index = 0; index < parent.childCount; ++index)
            {
                UpdateAlphaRecursive(parent.GetChild(index), alpha);
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
    [SerializeField]
    private EndGameManager _endGameManager;

    private bool IsGameEnded => _endGameManager != null && _endGameManager.IsInvoked;

    private void Start()
    {
        _pauseManager?.TogglePause(false);
        RefreshCarController();
    }

    private void RefreshCarController()
    {
        if (_endGameManager != null && !_endGameManager.CarInitilized)
            _endGameManager.Car = FindFirstObjectByType<CarController>();
    }

    private void OnEnable()
    {
        RefreshCarController();
        _endGameManager?.SubscribeEvents();
    }

    private void OnDisable()
    {
        RefreshCarController();
        _endGameManager?.UnsubscribeEvents();
    }

    private void Update()
    {
        HandleInput();

        _endGameManager?.AnimateMessages();
    }

    private void HandleInput()
    {
        ToggleBackMenuIfGameEnded();
        TogglePauseIfPressed();
    }

    private void ToggleBackMenuIfGameEnded()
    {
        if (IsGameEnded)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainMenuScene();
            }
        }
    }

    private void TogglePauseIfPressed()
    {
        if (IsGameEnded)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _pauseManager?.TogglePause();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenuScene()
    {
        LoadScene(_mainMenuSceneName);
    }

    public void OnClickedExitButton()
    {
        Application.Quit();
    }
}
