using Shark.Gameplay.WorldObjects;
using UnityEngine;

public class Finish : MonoBehaviour, IActivatable
{
    [SerializeField] private GameObject finishMenu;

    public void Activate()
    {
        finishMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
