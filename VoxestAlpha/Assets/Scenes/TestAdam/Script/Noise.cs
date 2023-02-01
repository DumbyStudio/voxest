using UnityEngine;

public static class Noise
{
    public static void SetSeed(int seed)
    {
        Random.InitState(seed);
    }

    public static float[] GeneratePlanetMap(int chunkSize, Vector3 chunkPos, float scale, int octaves, float persistance,
        float lacunarity, int planetSize, float amplificator)
    {
        float[] noiseMap = new float[(chunkSize + 1) * (chunkSize + 1) * (chunkSize + 1)];
        
        float maxPossibleHeight = 0;
        float amplitude = 1;
        for (int i = 0; i < octaves; ++i)
        {
            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }
        
        if (scale <= 0)
            scale = 0.0001f;
        
        Vector3 planetOrigin = new Vector3(0,0,0);
        
        int valueIndex = 0;
        for (int x = 0; x < chunkSize + 1; ++x)
        {
            int globalX = (int) chunkPos.x * chunkSize + x;
            for (int z = 0; z < chunkSize + 1; ++z)
            {
                int globalZ = (int) chunkPos.z * chunkSize + z;
			     
                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
        
                for (int i = 0; i < octaves; ++i)
                {
                    float sampleX = globalX/ scale * frequency;
                    float sampleZ = globalZ / scale * frequency;
        
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * -1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
			     
                float normalizedHeight = (noiseHeight + 1) / (2.0f * maxPossibleHeight);
			     
                float distanceMax = planetSize + normalizedHeight * amplificator;
        
                for (int y = 0; y < chunkSize + 1; ++y)
                {
                    int globalY = (int) chunkPos.y * chunkSize + y;
                    Vector3 voxelPos = new Vector3(globalX,globalY,globalZ);
        
                    float distance = Vector3.Distance(planetOrigin,voxelPos);
				    
                    noiseMap[valueIndex] = distance <= distanceMax ? 0 : 1;
                    valueIndex++;
                }
            }
        }

        // int valueIndex = 0;
        // for (int x = 0; x < chunkSize + 1; x++)
        // {
        //     for (int z = 0; z < chunkSize + 1; z++)
        //     {
        //         for (int y = 0; y < chunkSize + 1; y++)
        //         {
        //             noiseMap[valueIndex] = y <= chunkSize / 2 ? 0 : 1;
        //             valueIndex++;
        //         }
        //     }
        // }
        
        return noiseMap;
    }
}
