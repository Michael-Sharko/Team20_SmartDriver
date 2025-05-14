using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [field: SerializeField] public string LevelName { get; private set; }

    public Action<string> OnLevelSelect;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(() => OnLevelSelect?.Invoke(LevelName));
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(() => OnLevelSelect?.Invoke(LevelName));
    }

    public void LockLevelSelectButton(Sprite sprite)
    {
        _button.interactable = false;
        _button.image.sprite = sprite;
    }
    public void UnlockLevelSelectButton(Sprite sprite)
    {
        _button.interactable = true;
        _button.image.sprite = sprite;
    }
}