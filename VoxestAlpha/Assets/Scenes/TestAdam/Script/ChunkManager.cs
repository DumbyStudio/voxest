using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public int renderDistance = 2;
    public int chunkSize = 20;
    public int planetSize;
    public Material material;
    public int seed = 5;
    public List<Chunk> chunks;

    public int amplificator = 3;

    void Start()
    {
        Noise.SetSeed(seed);
        chunks = new List<Chunk>();
        planetSize = Mathf.RoundToInt(renderDistance * chunkSize * 0.75f);
        for (int x = -renderDistance; x < renderDistance; x++)
        {
            for (int z = -renderDistance; z < renderDistance; z++)
            {
                for (int y = -renderDistance; y < renderDistance; y++)
                {
                    chunks.Add(new Chunk(this, new Vector3(x,y,z),chunkSize,planetSize,material,amplificator));
                }
            }
        }
    }

    
    void Update()
    {
        
    }
}
