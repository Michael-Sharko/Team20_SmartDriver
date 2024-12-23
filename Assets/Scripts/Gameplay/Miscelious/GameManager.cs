using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    private void Start()
    {
        _pauseManager?.TogglePause(false);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        TogglePauseIfPressed();
    }

    private void TogglePauseIfPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _pauseManager?.TogglePause();
    }

#if UNITY_EDITOR
    public void LoadScene(SceneAsset scene)
    {
        LoadScene(scene.name);
    }
#endif

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickedExitButton()
    {
        Application.Quit();
    }
}
