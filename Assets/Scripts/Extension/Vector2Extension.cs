using UnityEngine;

namespace Scripts.Extension
{
    public static class Vector2Extension
    {
        public static Vector2 NewX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }
        public static Vector2 NewY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }


        public static Vector3 NewX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }
        public static Vector3 NewY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
        public static Vector3 NewZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
    }
}