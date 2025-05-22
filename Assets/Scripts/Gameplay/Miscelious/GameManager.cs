using Scripts.Gameplay.Tags;
using Shark.Gameplay.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string _mainMenuSceneName = "Main Menu";

    [SerializeField]
    private string _nextLevelSceneName;

    [SerializeField]
    private PauseManager _pauseManager;

    [SerializeField]
    private LossGameManager _endGameManager;

    private bool showedWinPanel;

    private bool IsGameEnded => _endGameManager != null && _endGameManager.IsInvoked;

    // если из SerializeField поля сделать field:SerializeField свойство
    // значения установленные в инспекторе отвалятся)
    public string NextLevelSceneName => _nextLevelSceneName;

    private void Start()
    {
        IfMainMenuThen();

        RefreshCarController();

        DisableLossManagerOnGameWin();

        _endGameManager.Init(this);
        new AudioVolumeController();
        new LevelUnlocker(this);
    }
    private void IfMainMenuThen()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            Cursor.lockState = CursorLockMode.None;
        else
            _pauseManager?.TogglePause(false);
    }
    private void DisableLossManagerOnGameWin()
    {
        var finish = FindObjectOfType<Finish>();

        if (finish != null)
            finish.OnActivate += () =>
            {
                _endGameManager.UnsubscribeEvents();
                _endGameManager.HideLossPanel();

                showedWinPanel = true;

                var a = FindObjectOfType<LevelMusicTag>();
                if (a)
                    a.gameObject.Off();
                else
                    Debug.LogError("В сцене источник звука с музыкой не помечен тегом");
            };
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
    }

    private void HandleInput()
    {
        TogglePauseIfPressed();
    }

    private void TogglePauseIfPressed()
    {
        if (IsGameEnded || showedWinPanel)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
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
    public void LoadNextLevelScene()
    {
        if (string.IsNullOrEmpty(_nextLevelSceneName))
        {
            Debug.LogError("Не установлено имя следующего уровня в Game Manager");
        }
        else
        {
            LoadScene(_nextLevelSceneName);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickedExitButton()
    {
        Application.Quit();
    }
}
