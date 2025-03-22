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

    private bool IsGameEnded => _endGameManager != null && _endGameManager.IsInvoked;

    private void Start()
    {
        _pauseManager?.TogglePause(false);
        RefreshCarController();

        _endGameManager.Init(this);
        new AudioVolumeController();
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

    public void LoadNextLevelScene()
    {
        if (string.IsNullOrEmpty(_nextLevelSceneName))
        {
            Debug.LogError("Ќе установлено им€ следующего уровн€ в Game Manager");
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
