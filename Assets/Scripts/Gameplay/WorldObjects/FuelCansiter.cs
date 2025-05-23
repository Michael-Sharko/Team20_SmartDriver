using System;
using Shark.Gameplay.Player;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FuelCansiter : MonoBehaviour, IPickupable
{
    private Renderer _canisterRenderer;
    [SerializeField] 
    private Color _canisterColor;
    
    [Min(0.1f)]
    public float amountOfFuel = 20.0f;

    public float rotatingSpeed = 30.0f;

    [SerializeField] private SoundOnEvent pickUpSounds;

    public event Action OnPickUp;

    private void Start()
    {
        InitializeCanister();

        pickUpSounds.Init(ref OnPickUp, GetComponent<AudioSource>());
    }

    private void OnValidate()
    {
        InitializeCanister();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotatingSpeed, Space.World);
    }

    private void InitializeCanister()
    {
        InitializeCanisterRenderer();
        RepaintIfCanisterHasRenderer(_canisterColor);  
    }

    private void InitializeCanisterRenderer()
    {
        _canisterRenderer = FindRendererInGameObject();
    }

    private Renderer FindRendererInGameObject()
    {
        Renderer renderer;
        if (!TryGetComponent(out renderer))
        {
            renderer = GetComponentInChildren<Renderer>();
        }
        return renderer;
    }

    private void RepaintIfCanisterHasRenderer(Color newColor)
    {
        if (_canisterRenderer != null)
            _canisterRenderer.sharedMaterial.color = newColor;
    }

    public void PickUp(IPlayer player)
    {
        OnPickUp?.Invoke();

        player.Refuel(amountOfFuel);

        GetComponent<Collider>().enabled = false;
        _canisterRenderer.enabled = false;

        this.LateAndInvoke(2,() => Destroy(gameObject));
    }
}
