using Shark.Gameplay.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.Physics
{
    [CreateAssetMenu(fileName = "Car Physics Data", menuName = "Gameplay/Player/Physics/Car Data"), Serializable]
    public class CarPhysicsData : ScriptableObject
    {
        public enum CarDriveType
        {
            FrontWheelDrive,
            RearWheelDrive,
            AllWheelDrive
        }

        public float mass;
        public Vector3 centerOfMass;

        public float drag;
        public float angularDrag;

        public CarDriveType driveType;

        public float motorForce;
        public float breakForce;
        public float maxSteerAngle;

        [HideInInspector, NonSerialized]
        public bool foldout = true;
    }
}