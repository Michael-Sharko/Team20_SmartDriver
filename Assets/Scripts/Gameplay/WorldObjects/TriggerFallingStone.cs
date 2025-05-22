using System;
using System.Collections.Generic;
using Shark.Gameplay.WorldObjects;
using UnityEngine;

[SelectionBase]
public class TriggerFallingStone : MonoBehaviour, IActivatable
{
    [SerializeField] private float mass = 1000;
    [SerializeField] private float fallSpeed;
    [SerializeField] private SoundOnEvent fallingSound;

    private Action onFalling;
    private List<Rigidbody> _stones = new();

    private void Awake()
    {
        fallingSound.Init(ref onFalling, GetComponent<AudioSource>());

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Rigidbody rig))
            {
                _stones.Add(rig);
                rig.mass = mass;
            }
        }
    }
    public void Activate()
    {
        onFalling?.Invoke();

        foreach (var stone in _stones)
        {
            stone.isKinematic = false;
            stone.AddForce(Vector2.down * fallSpeed, ForceMode.VelocityChange);
        }
    }
}