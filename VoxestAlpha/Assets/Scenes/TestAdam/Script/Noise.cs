using UnityEngine;

public struct NoiseParam
{
    public float scale;
    public int octaves;
    public float lacunarity;
    public float persistance;
    public float amplificator;
    public int seed;
    public int planetSize;
    public int worldSize;

    public NoiseParam(float scale,int octaves, float lacunarity, float persistance, float amplificator, int planetSize, int seed, int worldSize)
    {
        this.scale = scale <= 0 ? 0.0001f : scale;
        this.octaves = octaves;
        this.lacunarity = lacunarity;
        this.persistance = persistance;
        this.amplificator = amplificator;
        this.planetSize = planetSize;
        this.seed = seed;
        this.worldSize = worldSize;
    }
}

public static class Noise
{
    public static void SetSeed(int seed)
    {
        Random.InitState(seed);
    }

    public static Voxel[] GeneratePlanetMap(int chunkSize, Vector3 chunkPos, Vector3 planetOrigin, NoiseParam noiseParam)
    {
        Voxel[] noiseMap = new Voxel[(chunkSize + 1) * (chunkSize + 1) * (chunkSize + 1)];
        
        float maxPossibleHeight = 0.1f;
        float amplitude = 1;
        for (int i = 0; i < noiseParam.octaves; ++i)
        {
            maxPossibleHeight += amplitude;
            amplitude *= noiseParam.persistance;
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
        
                for (int i = 0; i < noiseParam.octaves; ++i)
                {
                    float sampleX = globalX/ noiseParam.scale * frequency;
                    float sampleZ = globalZ / noiseParam.scale * frequency;
        
                    float perlinValue = Mathf.PerlinNoise(sampleX / noiseParam.worldSize, sampleZ / noiseParam.worldSize);
                    
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= noiseParam.persistance;
                    frequency *= noiseParam.lacunarity;
                }
			     
                float normalizedHeight = (noiseHeight + 1) / (2.0f * maxPossibleHeight);

                float distanceMax = noiseParam.planetSize + normalizedHeight * noiseParam.amplificator;
        
                for (int y = 0; y < chunkSize + 1; ++y)
                {
                    float globalY = (int) chunkPos.y * chunkSize + y;
                    Vector3 voxelPos = new Vector3(globalX,globalY,globalZ);
        
                    float distance = Vector3.Distance(planetOrigin,voxelPos);

                    if (distance <= distanceMax)
                    {
                        float perlinValue = Noise3D(
                            globalX / noiseParam.worldSize,
                            globalY / noiseParam.worldSize,
                            globalZ / noiseParam.worldSize,
                            frequency,
                            noiseParam.lacunarity,
                            noiseParam.octaves,
                            noiseParam.seed
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
    
    public static float Noise3D(float x, float y, float z, float frequency, float lacunarity, int octave, int seed)
    {
        float noise = 0.0f;

        for (int i = 0; i < octave; ++i)
        {
            // Get all permutations of noise for each individual axis
            float noiseXY = Mathf.PerlinNoise(x * frequency + seed, y * frequency + seed);
            float noiseXZ = Mathf.PerlinNoise(x * frequency + seed, z * frequency + seed);
            float noiseYZ = Mathf.PerlinNoise(y * frequency + seed, z * frequency + seed);

            // Reverse of the permutations of noise for each individual axis
            float noiseYX = Mathf.PerlinNoise(y * frequency + seed, x * frequency + seed);
            float noiseZX = Mathf.PerlinNoise(z * frequency + seed, x * frequency + seed);
            float noiseZY = Mathf.PerlinNoise(z * frequency + seed, y * frequency + seed);

            // Use the average of the noise functions
            noise += (noiseXY + noiseXZ + noiseYZ + noiseYX + noiseZX + noiseZY) / 6.0f;

            frequency *= lacunarity;
        }

        // Use the average of all octaves
        return noise / octave;
    }
}