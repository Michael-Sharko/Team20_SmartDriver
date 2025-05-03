using UnityEngine;

// ��� ������� ����� ����� ���������� ����� ������� � ������ ��������
// �.�. ��� ������ ������������� � ����� ��� �������� ������
[DefaultExecutionOrder(100)]
public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    // ���������� ��������� ������ ������ ����� ��������� ������ ���������.
    // ��� �������� ��� Paris dakar rally ����� �������� ����� 0.25f.
    [SerializeField] float sharpnessOfFollowing = 0.5f; 

    private Transform cts;

    private void Start()
    {
        if (objectToFollow == null)
        {
            Debug.LogError("������ ������ � ���� ObjectToFollow �������� ������� CameraHandle. ���� �� ������ ��� CameraHandle - ��������� ����� � ���� ��������");
        }

        cts = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        var newPosition = (objectToFollow.position);
        var newRotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(objectToFollow.forward), sharpnessOfFollowing * Time.fixedDeltaTime);

        transform.SetPositionAndRotation(newPosition, newRotation);
        cts.rotation = Quaternion.LookRotation(objectToFollow.position - cts.position);
    }
}
