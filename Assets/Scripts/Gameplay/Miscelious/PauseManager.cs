using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PauseManager
{
    [SerializeField] UnityEvent OnPauseActivated;
    [SerializeField] UnityEvent OnPauseDeactivated;

    private bool _isPaused = false;

    public void TogglePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        (isPaused ? OnPauseActivated : OnPauseDeactivated)?.Invoke();

        _isPaused = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePause()
    {
        TogglePause(!_isPaused);
    }
}