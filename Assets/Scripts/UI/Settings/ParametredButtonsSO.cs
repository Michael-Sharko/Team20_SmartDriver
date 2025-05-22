using UnityEngine;

namespace Assets.Scripts.UI.Settings
{

    [CreateAssetMenu(
        fileName = nameof(ParametredButtonsSO),
        menuName = "ParametredButtonsSO"
)]
    public class ParametredButtonsSO : ScriptableObject
    {
        [field: SerializeField] public Color HighlightedColor { get; private set; }
        [field: SerializeField] public BaseGetSound ClickSound { get; private set; }
        [field: SerializeField] public BaseGetSound HighlightedSound { get; private set; }
    }
}