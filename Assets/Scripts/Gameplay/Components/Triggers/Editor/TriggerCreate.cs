using Scripts.Gameplay.Components;
using UnityEditor;
using UnityEngine;

public class TriggerCreate
{
    [MenuItem("GameObject/Triggers/SphereTrigger")]
    private static void CreateSphereTrigger(MenuCommand command)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "SphereTrigger";

        GameObjectUtility.SetParentAndAlign(go, command.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

        var trigger = go.AddComponent<TriggerSphere>();

        for (int i = 0; i < 3; i++)
        {
            UnityEditorInternal.ComponentUtility.MoveComponentUp(trigger);
        }

        var sphere = go.GetComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = 0.5f;
        var renderer = go.GetComponent<MeshRenderer>();
        renderer.material = Resources.Load<Material>(PathResources.Materials.Debug.TransparentRed30);
    }

    [MenuItem("GameObject/Triggers/CubeTrigger")]
    private static void CreateCubeTrigger(MenuCommand command)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "CubeTrigger";

        GameObjectUtility.SetParentAndAlign(go, command.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

        var trigger = go.AddComponent<TriggerBox>();

        for (int i = 0; i < 3; i++)
        {
            UnityEditorInternal.ComponentUtility.MoveComponentUp(trigger);
        }

        var box = go.GetComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = Vector3.one;
        var renderer = go.GetComponent<MeshRenderer>();
        renderer.material = Resources.Load<Material>(PathResources.Materials.Debug.TransparentRed30);
    }
}
