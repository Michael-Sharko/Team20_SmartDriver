using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Widgets
{
    public class AudioSliderWidget : MonoBehaviour
    {
        [field: SerializeField] public AudioVolumeID ID { get; private set; }
        public Slider Slider { get; private set; }

        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }
    }
}