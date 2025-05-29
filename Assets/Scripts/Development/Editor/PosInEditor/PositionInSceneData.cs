using System;
using System.Collections.Generic;

[Serializable]
public class ScenePositions
{
    public string sceneName;
    public List<PositionInScene> positionsInScene;

    public bool IsValid => positionsInScene != null && positionsInScene.Count > 0;
}
[Serializable]
public class ScenesPositions
{
    public List<ScenePositions> forScenes;

    public bool IsValid => forScenes != null && forScenes.Count > 0;
}