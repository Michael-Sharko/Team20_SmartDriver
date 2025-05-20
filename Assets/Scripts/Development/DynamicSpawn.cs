using UnityEngine;

public static class DynamicSpawn
{
    private static Transform _parent;

    public static Transform Parent
    {
        get
        {
            if (_parent == null)
            {
                _parent = new GameObject("----  DYNAMIC_SPAWN  ----").transform;
            }
            return _parent;
        }
    }

    private static Transform _dontDestroyOnLoadParent;

    public static Transform DontDestroyOnLoadParent
    {
        get
        {
            if (_dontDestroyOnLoadParent == null)
            {
                _dontDestroyOnLoadParent = new GameObject("----  DontDestroyOnLoadParent  ----").transform;
                Object.DontDestroyOnLoad(DontDestroyOnLoadParent);
            }
            return _dontDestroyOnLoadParent;
        }
    }
}