using UnityEngine;

// скрипт, чтобы AudioListener корректно определял сторону звука
// подробнее: https://www.youtube.com/watch?v=ghnXK5X3E-g
// тайминг 3:48, видос впринципе короткий, можно фул гляянуть

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