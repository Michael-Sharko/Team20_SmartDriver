using UnityEngine;

public abstract class BaseGetSound : ScriptableObject
{
    public abstract AudioClip GetClip();

    protected AudioClip LoadClip(string path)
    {
        var clip = Resources.Load<AudioClip>(path);
        //clip.completed += Operation_completed;
        return clip;
    }

    //private void Operation_completed(AsyncOperation obj)
    //{
    //    if (obj.isDone)
    //    {

    //    }
    //}
}
