using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] float sharpnessOfFollowing = 0.5f; //определяет насколько быстро камера будет принимать нужное положение. Для ощущения как Paris dakar rally нужно значение около 0.25f.

    private Transform cts;

    private void Start()
    {
        if(objectToFollow == null)
        {
            Debug.LogError("Добавь машину в поле ObjectToFollow игрового объекта CameraHandle. Если не знаешь где CameraHandle - используй поиск в окне иерархии");
        }

        cts = Camera.main.transform;
    }

    private void Update()
    {
        transform.position = (objectToFollow.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(objectToFollow.forward), sharpnessOfFollowing * Time.deltaTime);
        cts.rotation = Quaternion.LookRotation(objectToFollow.position - cts.position);
    }
}
