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
        public float mass;
        public float drag;
        public float angularDrag;

        public CarController.CarDriveType driveType;

        public float motorForce;
        public float breakForce;
        public float maxSteerAngle;

        [HideInInspector, NonSerialized]
        public bool foldout;
    }
}