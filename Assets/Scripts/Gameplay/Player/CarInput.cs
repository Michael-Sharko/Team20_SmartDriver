using System;
using Assets.Scripts.Development;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarInput : IDisposable
    {
        private const string INPUT_HORIZONTAL = "Horizontal";
        private const string INPUT_VERTICAL = "Vertical";

        public float hInput { get; private set; }
        public float vInput { get; private set; }
        public bool spaceInput { get; private set; }
        public bool Enabled { get; set; } = true;

        public CarInput()
        {
            MonoEventSender.Events.OnUpdate += Update;
        }
        public void Dispose()
        {
            MonoEventSender.Events.OnUpdate -= Update;
        }

        private void Update()
        {
            if (!Enabled)
            {
                hInput = 0;
                vInput = 0;
                spaceInput = false;

                return;
            }

            hInput = Input.GetAxis(INPUT_HORIZONTAL);
            vInput = Input.GetAxis(INPUT_VERTICAL);

            //Debug.Log($"hInput {hInput}, vInput {vInput}");

            spaceInput = Input.GetKey(KeyCode.Space);
        }
    }
}