using UnityEngine;

[CreateAssetMenu(fileName = "BrakeMarkData", menuName = "Settings/BrakeMarkData")]
public class BrakeMarkData : ScriptableObject
{
    [field: SerializeField] public float Width { get; private set; } = 0.5f;
    [field: SerializeField] public float Time { get; private set; } = 60f;
    [field: SerializeField] public Material Material { get; private set; }
}
