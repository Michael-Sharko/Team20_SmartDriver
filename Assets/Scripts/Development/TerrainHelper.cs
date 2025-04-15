using UnityEngine;

public class TerrainHelper
{
    public static Texture2D TextureUnderWheel(WheelHit hit)
    {
        if (hit.collider is not TerrainCollider terrainCollider)
            return null;

        var layers = terrainCollider.terrainData.terrainLayers;
        var textureIndex = layers[GetMainTextureIndex(hit.point, terrainCollider)];

        var textureUnderWheel = textureIndex.diffuseTexture;

        return textureUnderWheel;
    }
    /*
    private static bool IsSlidingTexture(Texture2D texture)
    {
        foreach (var slidingTexture in dirtTextures)
        {
            if (texture == slidingTexture)
            {
                return true;
            }
        }
        return false;
    }
    */
    private static int GetMainTextureIndex(Vector3 hitWorldPos, TerrainCollider terrainCollider)
    {
        float[] mix = GetTextureMix(hitWorldPos, terrainCollider);

        float maxMix = 0;
        int maxIndex = 0;

        for (int n = 0; n < mix.Length; ++n)
        {
            if (mix[n] > maxMix)
            {
                maxIndex = n;
                maxMix = mix[n];
            }
        }

        return maxIndex;
    }
    private static float[] GetTextureMix(Vector3 worldPos, TerrainCollider terrainCollider)
    {
        TerrainData data = terrainCollider.terrainData;

        Vector3 terrainPos = terrainCollider.transform.position;

        int mapX = (int)(((worldPos.x - terrainPos.x) / data.size.x) * data.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrainPos.z) / data.size.z) * data.alphamapHeight);

        float[,,] splatmapData = data.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
        for (int n = 0; n < cellMix.Length; ++n)
        {
            cellMix[n] = splatmapData[0, 0, n];
        }

        return cellMix;
    }
}
