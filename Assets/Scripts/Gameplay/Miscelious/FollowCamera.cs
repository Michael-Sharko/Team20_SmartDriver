using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float multiplySmoothSpeed = 4f;

    public float minDistanceToTarget = 25.0f;
    public float maxDistanceToTarget = 30.0f;

#if UNITY_EDITOR
    [field: SerializeField, Header("Debug-Constant")]
    public float DistanceToTarget { get; private set; }
#endif

    private void LateUpdate()
    {
        float distanceToTarget = CalculateDistanceToTarget();
        if (distanceToTarget > minDistanceToTarget)
        {
            float currentSmoothSpeed = CalculateSmoothSpeedIfCameraIsCloserThanMaxDistance(distanceToTarget);
            transform.position = CalculateSmoothPosition(currentSmoothSpeed);
        }
        transform.LookAt(target.position);

#if UNITY_EDITOR
        DistanceToTarget = distanceToTarget;
#endif
    }

    private float CalculateDistanceToTarget()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    private float CalculateSmoothSpeedIfCameraIsCloserThanMaxDistance(float distanceToTarget)
    {
        return distanceToTarget < maxDistanceToTarget ? CalculateNormalSmoothSpeed() : CalculateAcceleratedSmoothSpeed();
    }

    private float CalculateNormalSmoothSpeed()
    {
        return Time.deltaTime * smoothSpeed;
    }

    private float CalculateAcceleratedSmoothSpeed()
    {
        return CalculateNormalSmoothSpeed() * multiplySmoothSpeed;
    }

    private Vector3 CalculateSmoothPosition(float smoothSpeed)
    {
        Vector3 positionToGo = new Vector3(target.position.x, 0, target.position.z) + offset;
        return Vector3.Lerp(transform.position, positionToGo, smoothSpeed);
    }
}
