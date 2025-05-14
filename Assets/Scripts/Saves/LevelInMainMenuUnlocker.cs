using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInMainMenuUnlocker : MonoBehaviour
{
    [Serializable]
    private class LevelSelectWidget
    {
        [field: SerializeField] public LevelSelectButton LevelSelectButton { get; private set; }
        [field: SerializeField] public Sprite LevelUnlockedButtonSprite { get; private set; }
    }

    [SerializeField] private LevelSelectWidget[] levelSelectButtons;
    [SerializeField] private Sprite levelLockButtonSprite;

    private LevelLockStateSaver levelLockStateSaver;

    void Start()
    {
        levelLockStateSaver = new();

        for (int i = 0; i < levelSelectButtons.Length; i++)
        {
            var widget = levelSelectButtons[i];

            SubscribeOnClick(widget.LevelSelectButton);
            SetLockState(widget);
        }
    }
    private void SetLockState(LevelSelectWidget widget)
    {
        var button = widget.LevelSelectButton;

        if (levelLockStateSaver.LevelIsUnlocked(button.LevelName))
        {
            button.UnlockLevelSelectButton(widget.LevelUnlockedButtonSprite);
        }
        else
        {
            button.LockLevelSelectButton(levelLockButtonSprite);
        }
    }
    private void SubscribeOnClick(LevelSelectButton widget)
    {
        widget.OnLevelSelect += (levelName) => SceneManager.LoadScene(levelName);
    }
}