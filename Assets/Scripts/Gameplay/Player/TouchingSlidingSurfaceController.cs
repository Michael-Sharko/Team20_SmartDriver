using System;
using UnityEngine;
using static Shark.Gameplay.Player.Wheels;

[Serializable]
public class TouchingSlidingSurfaceController
{
    public struct WheelFrictionStiffness
    {

        public float forwardFrictionStiffness;
        public float sidewaysFrictionStiffness;

        public WheelFrictionStiffness(float forwardFrictionStiffness, float sidewaysFrictionStiffness)
        {
            this.forwardFrictionStiffness = forwardFrictionStiffness;
            this.sidewaysFrictionStiffness = sidewaysFrictionStiffness;
        }
    }

    [SerializeField] private Texture2D[] slidingSurfacesTextures;
    [SerializeField] private float forwardFrictionStiffness;
    [SerializeField] private float sidewaysFrictionStiffness;

    public bool TryCalculateSlidingToWheel(
        Texture2D textureUnderWheel,
        WheelData wheelData,
        out WheelFrictionStiffness stiffness)
    {
        stiffness = default;

        if (slidingSurfacesTextures == null || slidingSurfacesTextures.Length == 0)
            return false;
        if (wheelData.partNumber == Part.FR || wheelData.partNumber == Part.FL)
            return false;

        if (IsSlidingTexture(textureUnderWheel))
        {
            stiffness = new(forwardFrictionStiffness, sidewaysFrictionStiffness);

            return true;
        }

        return false;
    }
    private bool IsSlidingTexture(Texture2D texture)
    {
        foreach (var slidingTexture in slidingSurfacesTextures)
        {
            if (texture == slidingTexture)
            {
                return true;
            }
        }
        return false;
    }
}