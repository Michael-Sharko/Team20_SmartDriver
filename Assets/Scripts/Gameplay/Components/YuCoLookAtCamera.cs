using UnityEngine;

namespace YusamCommon
{
    public enum YuCoCameraModeEnum
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }
    public class YuCoLookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform mainCamera;

        [SerializeField]
        private Vector3 offset;

        [SerializeField]
        private YuCoCameraModeEnum mode = YuCoCameraModeEnum.CameraForward;

        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main.transform;
            }

        }

        private void LateUpdate()
        {
            if (!mainCamera) return;

            switch (mode)
            {
                case YuCoCameraModeEnum.LookAt:
                transform.LookAt(mainCamera.transform.position + offset);
                break;
                case YuCoCameraModeEnum.LookAtInverted:
                var pos = transform.position;
                Vector3 dirFromCamera = pos - mainCamera.transform.position;
                transform.LookAt(pos + dirFromCamera);
                break;
                case YuCoCameraModeEnum.CameraForward:
                transform.forward = mainCamera.transform.forward;
                break;
                case YuCoCameraModeEnum.CameraForwardInverted:
                transform.forward = -1 * mainCamera.transform.forward;
                break;
            }

        }
    }
}