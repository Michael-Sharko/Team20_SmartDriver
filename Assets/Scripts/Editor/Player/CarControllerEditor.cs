using UnityEditor;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [CustomEditor(typeof(CarController))]
    public class CarControllerEditor : Editor
    {
        CarController _controller;
        Editor _editor;

        private void OnEnable()
        {
            _controller = (CarController)target;
        }

        public override void OnInspectorGUI()
        {
            UpdateEditorScope();

#if UNITY_EDITOR
            EditorGUILayout.LabelField("Physic Settings:");

            DrawCarPhysicsData();
            DrawWheelPhysicsData();
#endif
        }

        private void UpdateEditorScope()
        {
            base.OnInspectorGUI();
        }

        private void DrawCarPhysicsData()
        {
            DrawScriptableObjectEditor(_controller.carPhysics, ref _controller.carPhysics.foldout, ref _editor);
        }

        private void DrawWheelPhysicsData()
        {
            DrawScriptableObjectEditor(_controller.wheelPhysics, ref _controller.wheelPhysics.foldout, ref _editor);
        }

        private void DrawScriptableObjectEditor(Object scriptable, ref bool foldout, ref Editor editor)
        {
            if (scriptable != null && (foldout = EditorGUILayout.InspectorTitlebar(foldout, scriptable)))
            {
                CreateCachedEditor(scriptable, null, ref editor);
                editor.OnInspectorGUI();
            }
        }
    }
}