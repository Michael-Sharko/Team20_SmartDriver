using Shark.Gameplay.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public struct Wheel
    {
        [SerializeField]
        private WheelPhysicsData _data;

        private const string WHEEL_TOOLTIP =
            "0 - Front left wheel part\n" +
            "1 - Front right wheel part\n" +
            "2 - Rear left wheel part\n" +
            "3 - Rear right wheel part";

        [Tooltip(WHEEL_TOOLTIP), SerializeField]
        private WheelCollider[] _colliders;

        [Tooltip(WHEEL_TOOLTIP), SerializeField]
        private Transform[] _transforms;

        private float[] _originalForwardStiffness;
        private float[] _originalSidewaysStiffness;

#if UNITY_EDITOR
        public WheelPhysicsData wheelPhysics => _data;
#endif

        public enum Part : int
        {
            FL = 0, FR = 1,
            RL = 2, RR = 3,

            Count = 4
        }

        public struct WheelData
        {
            public Part partNumber;

            public WheelCollider whellCollider;
            public Transform transform;

            public float originalForwardStiffness { get; set; }
            public float originalSidewaysStiffness { get; set; }
        }

        public readonly WheelData this[Part part]
        {
            get
            {
                return new WheelData
                {
                    partNumber = part,
                    whellCollider = _colliders[(int)part],
                    transform = _transforms[(int)part],

                    originalForwardStiffness = _originalForwardStiffness[(int)part],
                    originalSidewaysStiffness = _originalSidewaysStiffness[(int)part],
                };
            }
        }

        public void ApplyWheelData()
        {
            _originalForwardStiffness = new float[_colliders.Length];
            _originalSidewaysStiffness = new float[_colliders.Length];

            int part = 0;
            foreach (var wheel in _colliders)
            {
                wheel.mass = _data.mass;
                wheel.wheelDampingRate = _data.wheelDampingRate;
                wheel.suspensionDistance = _data.suspensionDistance;
                wheel.forceAppPointDistance = _data.forceAppPointDistance;

                wheel.suspensionSpring = _data.suspensionSpring;
                wheel.forwardFriction = _data.forwardFiction;
                wheel.sidewaysFriction = _data.sidewaysFiction;

                _originalForwardStiffness[part] = wheel.forwardFriction.stiffness;
                _originalSidewaysStiffness[part] = wheel.sidewaysFriction.stiffness;

                ++part;
            }
        }
    }
}