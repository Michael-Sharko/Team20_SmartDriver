using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>().transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}