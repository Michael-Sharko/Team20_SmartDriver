using UnityEngine;

public class LevelUnlocker
{
    private readonly GameManager manager;
    public LevelUnlocker(GameManager manager)
    {
        // зачем нужен финиш, я хотел дергать события в гейм менеджере, когда он запускает
        // след уровень, но тогда если игрок прошел уровень и НЕ нажал "след уровень"
        // а например "в главное меню", то уровень НЕ разблокируется
        var finish = Object.FindObjectOfType<Finish>();

        if (finish)
            finish.OnActivate += LevelUnlocker_OnActivate;

        this.manager = manager;
    }

    private void LevelUnlocker_OnActivate()
    {
        var unlockedLevelName = manager.NextLevelSceneName;
        PlayerPrefs.SetString(unlockedLevelName, unlockedLevelName);
    }
}