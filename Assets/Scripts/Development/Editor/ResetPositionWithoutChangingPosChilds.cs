using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Development
{
    public class ResetPositionWithoutChangingPosChilds
    {
        [MenuItem("CONTEXT/Transform/ResetPosWithoutChangingPosChilds")]
        private static void Do(MenuCommand command)
        {
            Transform parent = (Transform)command.context;

            List<Vector3> originalPositions = new();

            foreach (var child in parent)
            {
                var childTransform = child as Transform;
                originalPositions.Add(childTransform.position);
            }

            parent.position = Vector3.zero;

            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                child.position = originalPositions[i];
            }
        }
    }
}