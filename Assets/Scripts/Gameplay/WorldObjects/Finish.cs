using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;

public class Finish : MonoBehaviour, IActivatable
{
    [SerializeField] private GameObject finishMenu;

    public event Action OnActivate;

    public void Activate()
    {
        OnActivate?.Invoke();

        finishMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
