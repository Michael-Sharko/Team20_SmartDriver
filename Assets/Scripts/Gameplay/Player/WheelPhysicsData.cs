using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.Physics
{
    [CreateAssetMenu(fileName = "Wheel Physics Data", menuName = "Gameplay/Player/Physics/Wheel Data")]
    public class WheelPhysicsData : ScriptableObject
    {
        public float mass;
        public float wheelDampingRate;
        public float suspensionDistance;
        public float forceAppPointDistance;

        public JointSpringData suspensionSpring;
        public WheelFrictionCurveData forwardFiction;
        public WheelFrictionCurveData sidewaysFiction;

        [Serializable]
        public struct JointSpringData
        {
            public float spring;
            public float damper;
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
            public float extremumSlip;
            public float extremumValue;
            public float asymptoteSlip;
            public float asymptoteValue;
            public float stiffness;

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
        public bool foldout;
    }
}