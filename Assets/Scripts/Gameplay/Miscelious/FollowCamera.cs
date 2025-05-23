using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    // определяет насколько быстро камера будет принимать нужное положение.
    // Для ощущения как Paris dakar rally нужно значение около 0.25f.
    [SerializeField] float sharpnessOfFollowing = 0.5f; 

    private Transform cts;

    private void Start()
    {
        if (objectToFollow == null)
        {
            Debug.LogError("Добавь машину в поле ObjectToFollow игрового объекта CameraHandle. Если не знаешь где CameraHandle - используй поиск в окне иерархии");
        }

        cts = Camera.main.transform;
    }

    private void Update()
    {
        var newPosition = (objectToFollow.position);
        var newRotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(objectToFollow.forward), sharpnessOfFollowing * Time.deltaTime);

        transform.SetPositionAndRotation(newPosition, newRotation);
        cts.rotation = Quaternion.LookRotation(objectToFollow.position - cts.position);
    }
}
