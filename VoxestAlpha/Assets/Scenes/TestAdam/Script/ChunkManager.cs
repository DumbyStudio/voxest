using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public int renderDistance = 3;
    public int chunkSize = 50;
    public Material material;
    public int seed = 5;
    public List<Chunk> chunks;

    public NoiseParam noiseParam;
    
    public float scale;
    public int octaves;
    public float lacunarity;
    public float persistance;
    public float amplificator;
    public int planetSize;
    public Vector3 planetOrigin;

    void Start()
    {
        Noise.SetSeed(seed);
        chunks = new List<Chunk>();
        
        noiseParam = new NoiseParam(scale, octaves, lacunarity, persistance, amplificator, planetSize, seed, renderDistance * chunkSize);
        
        for (int x = -renderDistance; x < renderDistance; x++)
        {
            for (int z = -renderDistance; z < renderDistance; z++)
            {
                for (int y = -renderDistance; y < renderDistance; y++)
                {
                    chunks.Add(new Chunk(this, new Vector3(x,y,z),chunkSize,material,noiseParam,planetOrigin));
                }
            }
        }
    }
}
