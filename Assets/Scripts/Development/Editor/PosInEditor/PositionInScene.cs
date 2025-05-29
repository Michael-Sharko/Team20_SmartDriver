using System;
using UnityEngine;

[Serializable]
public struct PositionInScene
{
    public string description;
    public PositionInSceneMode mode;
    public Vector3 position;
    public Quaternion rotation;
    public string gameobjectName;
}
