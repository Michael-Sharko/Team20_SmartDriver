using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] float sharpnessOfFollowing = 0.5f; //���������� ��������� ������ ������ ����� ��������� ������ ���������. ��� �������� ��� Paris dakar rally ����� �������� ����� 0.25f.

    private Transform cts;

    private void Start()
    {
        if(objectToFollow == null)
        {
            Debug.LogError("������ ������ � ���� ObjectToFollow �������� ������� CameraHandle. ���� �� ������ ��� CameraHandle - ��������� ����� � ���� ��������");
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
