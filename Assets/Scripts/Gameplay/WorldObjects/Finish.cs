using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;

public class Finish : MonoBehaviour, IActivatable
{
    [SerializeField] private GameObject finishMenu;
    [SerializeField] private SoundOnEvent finishSound;

    public event Action OnActivate;

    private bool wasActivated;

    private void Awake()
    {
        finishSound.Init(ref OnActivate);
    }
    public void Activate()
    {
        if (wasActivated) return;

        wasActivated = true;

        OnActivate?.Invoke();

        finishMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
