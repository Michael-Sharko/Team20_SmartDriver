using Cinemachine;
using UnityEngine;

namespace Scripts.Component.Camera
{
    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
    [AddComponentMenu("")] // Don't display in add component menu
    [SaveDuringPlay]
    public class MyFollower : CinemachineComponentBase
    {
        [SerializeField] float sharpnessOfFollowing = 0.5f;
        [SerializeField] private Vector3 offset = new(-1.73f, 15.38f, -18.11f);

        /// <summary>True if component is enabled and has a Follow target defined</summary>
        public override bool IsValid => enabled && FollowTarget != null;

        public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;


        // Report maximum damping time needed for this component.
        public override float GetMaxDampTime() => sharpnessOfFollowing;

        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            if (!IsValid)
                return;

            var parent = transform.parent.parent;

            var newPosition = FollowTarget.position;
            var newRotation = Quaternion.Slerp(
                parent.rotation,
                Quaternion.LookRotation(FollowTarget.forward), 
                sharpnessOfFollowing * Time.deltaTime);

            parent.SetPositionAndRotation(newPosition, newRotation);

            curState.RawPosition = parent.TransformPoint(offset);
        }
    }
}