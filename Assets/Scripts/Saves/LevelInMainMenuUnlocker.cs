using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInMainMenuUnlocker : MonoBehaviour
{
    [SerializeField] private LevelSelectWidget[] levelSelectButtons;
    [SerializeField] private Sprite levelLockButtonSprite;
    [SerializeField] private Sprite levelUnlockButtonSprite;

    private LevelLockStateSaver levelLockStateSaver;

    void Start()
    {
        levelLockStateSaver = new();

        for (int i = 0; i < levelSelectButtons.Length; i++)
        {
            var widget = levelSelectButtons[i];

            SubscribeOnClick(widget);
            SetLockState(widget);
        }
    }
    private void SetLockState(LevelSelectWidget widget)
    {
        if (levelLockStateSaver.LevelIsUnlocked(widget.LevelName))
        {
            widget.UnlockLevelSelectButton(levelUnlockButtonSprite);
        }
        else
        {
            widget.LockLevelSelectButton(levelLockButtonSprite);
        }
    }
    private void SubscribeOnClick(LevelSelectWidget widget)
    {
        widget.OnLevelSelect += (levelName) => SceneManager.LoadScene(levelName);
    }
}