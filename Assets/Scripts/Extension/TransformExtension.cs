using UnityEngine;

public static class TransformExtension {

    public static Vector2 position2D(this Transform transform) {
        return transform.position;
    }
    public static void SetX(this Transform transform, float x) {
        var pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }
    public static void SetY(this Transform transform, float y) {
        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
    public static void SetZ(this Transform transform, float z) {
        var pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }
    public static void IncreaseX(this Transform transform, float x) {
        var pos = transform.position;
        pos.x += x;
        transform.position = pos;
    }
    public static void IncreaseY(this Transform transform, float y) {
        var pos = transform.position;
        pos.y += y;
        transform.position = pos;
    }
    public static void IncreaseZ(this Transform transform, float z) {
        var pos = transform.position;
        pos.z += z;
        transform.position = pos;
    }
}