using System;
using UnityEngine;
using static Shark.Gameplay.Player.Wheel;

[Serializable]
public class TouchingSlidingSurfaceController {

    public struct WheelFrictionStiffness {

        public float forwardFrictionStiffness;
        public float sidewaysFrictionStiffness;

        public WheelFrictionStiffness(float forwardFrictionStiffness, float sidewaysFrictionStiffness) {
            this.forwardFrictionStiffness = forwardFrictionStiffness;
            this.sidewaysFrictionStiffness = sidewaysFrictionStiffness;
        }
    }

    [SerializeField] private Texture2D[] slidingSurfacesTextures;
    [SerializeField] private float forwardFrictionStiffness;
    [SerializeField] private float sidewaysFrictionStiffness;

    private TerrainCollider _terrainCollider;

    public bool TryCalculateSlidingToWheel(WheelData wheelData, WheelHit hit, out WheelFrictionStiffness stiffness) {

        stiffness = default;

        if (slidingSurfacesTextures == null || slidingSurfacesTextures.Length == 0)
            return false;
        if (wheelData.partNumber == Part.FR || wheelData.partNumber == Part.FL)
            return false;
        if (hit.collider is not TerrainCollider terrainCollider)
            return false;

        _terrainCollider = terrainCollider;

        TerrainData data = terrainCollider.terrainData;


        var textureUnderWheel = data.terrainLayers[GetMainTexture(hit.point)].diffuseTexture;

        if (IsSlidingTexture(textureUnderWheel)) {

            stiffness = new(forwardFrictionStiffness, sidewaysFrictionStiffness);

            return true;

        }

        return false;

    }
    private bool IsSlidingTexture(Texture2D texture) {
        foreach (var slidingTexture in slidingSurfacesTextures) {
            if (texture == slidingTexture) {
                return true;
            }
        }
        return false;
    }
    private int GetMainTexture(Vector3 worldPos) {

        float[] mix = GetTextureMix(worldPos);

        float maxMix = 0;
        int maxIndex = 0;

        for (int n = 0; n < mix.Length; ++n) {
            if (mix[n] > maxMix) {
                maxIndex = n;
                maxMix = mix[n];
            }
        }

        return maxIndex;

    }
    private float[] GetTextureMix(Vector3 worldPos) {

        TerrainData data = _terrainCollider.terrainData;

        Vector3 terrainPos = _terrainCollider.transform.position;

        int mapX = (int)(((worldPos.x - terrainPos.x) / data.size.x) * data.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrainPos.z) / data.size.z) * data.alphamapHeight);

        float[,,] splatmapData = data.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
        for (int n = 0; n < cellMix.Length; ++n) {
            cellMix[n] = splatmapData[0, 0, n];
        }

        return cellMix;

    }
}