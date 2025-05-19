using System;
using UnityEngine;

namespace Shark.Gameplay.Physics
{
    [CreateAssetMenu(fileName = "Wheel Physics Data", menuName = "Gameplay/Player/Physics/Wheel Data")]
    public class WheelPhysicsData : ScriptableObject
    {
        [Min(.0001f)] public float mass;
        [Min(.0001f)] public float wheelDampingRate;
        public float suspensionDistance;
        public float forceAppPointDistance;

        public JointSpringData suspensionSpring;
        public WheelFrictionCurveData forwardFiction;
        public WheelFrictionCurveData sidewaysFiction;

        [Serializable]
        public struct JointSpringData
        {
            [Min(0f)] public float spring;
            [Min(0f)] public float damper;
            public float targetPosition;

            public static implicit operator JointSpring(JointSpringData data)
            {
                return new JointSpring
                {
                    spring = data.spring,
                    damper = data.damper,
                    targetPosition = data.targetPosition
                };
            }
        }

        [Serializable]
        public struct WheelFrictionCurveData
        {
            [Min(.001f)] public float extremumSlip;
            [Min(.001f)] public float extremumValue;
            [Min(.001f)] public float asymptoteSlip;
            [Min(.001f)] public float asymptoteValue;
            [Min(0f)] public float stiffness;

            public static implicit operator WheelFrictionCurve(WheelFrictionCurveData data)
            {
                return new WheelFrictionCurve
                {
                    extremumSlip = data.extremumSlip,
                    extremumValue = data.extremumValue,
                    asymptoteSlip = data.asymptoteSlip,
                    asymptoteValue = data.asymptoteValue,
                    stiffness = data.stiffness
                };
            }
        }

        [HideInInspector, NonSerialized]
        public bool foldout = true;
    }
}