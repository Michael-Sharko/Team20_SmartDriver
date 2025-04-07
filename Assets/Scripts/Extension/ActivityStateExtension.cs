using UnityEngine;

public static class ActivityStateExtension
{
    public static void On(this Collider component) => component.enabled = true;
    public static void Off(this Collider component) => component.enabled = false;

    public static void On(this Behaviour component) => component.enabled = true;
    public static void Off(this Behaviour component) => component.enabled = false;

    public static void On(this Renderer component) => component.enabled = true;
    public static void Off(this Renderer component) => component.enabled = false;

    public static void On(this GameObject gameObject) => gameObject.SetActive(true);
    public static void Off(this GameObject gameObject) => gameObject.SetActive(false);
}
