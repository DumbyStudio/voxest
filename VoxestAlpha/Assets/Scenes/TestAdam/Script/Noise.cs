using UnityEngine;

[System.Serializable]
public struct NoiseParam
{
    public float scale;
    public int octaves;
    public float lacunarity;
    public float persistance;
    public float amplificator;
    public int planetSize;
}

public static class Noise
{
    public static void SetSeed(int seed)
    {
        Random.InitState(seed);
    }

    public static Voxel[] GeneratePlanetMap(int chunkSize, Vector3 chunkPos, Vector3 planetOrigin, NoiseParam floorParam,NoiseParam caveParam)
    {
        Voxel[] noiseMap = new Voxel[(chunkSize + 1) * (chunkSize + 1) * (chunkSize + 1)];
        
        float maxPossibleHeight = 0.1f;
        float amplitude = 1;
        for (int i = 0; i < floorParam.octaves; ++i)
        {
            maxPossibleHeight += amplitude;
            amplitude *= floorParam.persistance;
        }
        
        int valueIndex = 0;
        for (int x = 0; x < chunkSize + 1; ++x)
        {
            float globalX = (int) chunkPos.x * chunkSize + x;
            for (int z = 0; z < chunkSize + 1; ++z)
            {
                float globalZ = (int) chunkPos.z * chunkSize + z;
			     
                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
        
                for (int i = 0; i < floorParam.octaves; ++i)
                {
                    float sampleX = globalX/ floorParam.scale * frequency;
                    float sampleZ = globalZ / floorParam.scale * frequency;
        
                    float perlinValue = (float) SimplexNoise.Noise(sampleX, sampleZ);
                    
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= floorParam.persistance;
                    frequency *= floorParam.lacunarity;
                }
			     
                float normalizedHeight = (noiseHeight + 1) / (2.0f * maxPossibleHeight);

                float distanceMax = floorParam.planetSize + normalizedHeight * floorParam.amplificator;

                for (int y = 0; y < chunkSize + 1; ++y)
                {
                    float globalY = (int) chunkPos.y * chunkSize + y;
                    Vector3 voxelPos = new Vector3(globalX,globalY,globalZ);
        
                    float distance = Vector3.Distance(planetOrigin,voxelPos);

                    if (distance <= distanceMax)
                    {
                        float perlinValue = Noise3D(
                            globalX,
                            globalY,
                            globalZ,
                            caveParam
                        );

                        noiseMap[valueIndex].density = perlinValue;
                        noiseMap[valueIndex].type = distance <= distanceMax * 0.95f ? 1 : 0;
                    }
                    else
                    {
                        noiseMap[valueIndex].density = 1;
                    }
                    valueIndex++;
                }
            }
        }
        
        return noiseMap;
    }
    
    public static float Noise3D(float x, float y, float z, NoiseParam noiseParam)
    {
        float noiseHeight = 0.0f;
        float frequency = 1;
        float amplitude = 1;
        
        float maxPossibleHeight = 0.1f;
        for (int i = 0; i < noiseParam.octaves; ++i)
        {
            maxPossibleHeight += amplitude;
            amplitude *= noiseParam.persistance;
        }

        amplitude = 1;

        for (int i = 0; i < noiseParam.octaves; ++i)
        {
            float sampleX = x / noiseParam.scale * frequency;
            float sampleY = y / noiseParam.scale * frequency;
            float sampleZ = z / noiseParam.scale * frequency;

            float perlinValue = (float) SimplexNoise.Noise(sampleX,sampleY,sampleZ);

            perlinValue *= noiseParam.amplificator;
            noiseHeight += perlinValue * amplitude;
            amplitude *= noiseParam.persistance;
            frequency *= noiseParam.lacunarity;
        }

        return (noiseHeight + 1) / (2.0f * maxPossibleHeight);
    }

}