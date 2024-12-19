using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GamePauseManager
{

    private bool _isPaused;

    public void TogglePause()
    {
        _isPaused = !_isPaused;
    }
}
