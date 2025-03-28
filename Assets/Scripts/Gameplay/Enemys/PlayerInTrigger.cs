using Shark.Gameplay.Player;
using UnityEngine;

public class PlayerInTrigger : MonoBehaviour
{
    public bool IsTouching { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer _))
            IsTouching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer _))
            IsTouching = false;
    }
}
