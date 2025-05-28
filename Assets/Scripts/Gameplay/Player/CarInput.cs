using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarInput : MonoBehaviour, IInput
    {
        private const string INPUT_HORIZONTAL = "Horizontal";
        private const string INPUT_VERTICAL = "Vertical";

        public float HInput { get; private set; }
        public float VInput { get; private set; }
        public bool SpaceInput { get; private set; }
        public bool Enabled { get; set; } = true;

        public void Update()
        {
            if (!Enabled)
            {
                HInput = 0;
                VInput = 0;
                SpaceInput = false;

                return;
            }

            HInput = Input.GetAxis(INPUT_HORIZONTAL);
            VInput = Input.GetAxis(INPUT_VERTICAL);

            //Debug.Log($"hInput {hInput}, vInput {vInput}");

            SpaceInput = Input.GetKey(KeyCode.Space);
        }
    }
}
