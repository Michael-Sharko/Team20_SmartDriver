using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.Miscelious
{
    public interface IPauseHandler
    {
        void HandlePauseStateChanged(bool isPaused);
    }
}