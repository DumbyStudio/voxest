using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public int renderDistance = 3;
    public int chunkSize = 50;
    public Material material;
    public int seed = 5;
    public List<Chunk> chunks;
    public bool isInterpolate = true;

    public NoiseParam floorParam;
    public NoiseParam caveParam;
    
    public Vector3 planetOrigin;

    void Start()
    {
        Noise.SetSeed(seed);
        chunks = new List<Chunk>();
        
        for (int x = -renderDistance; x < renderDistance; x++)
        {
            for (int z = -renderDistance; z < renderDistance; z++)
            {
                for (int y = -renderDistance; y < renderDistance; y++)
                {
                    chunks.Add(new Chunk(this, new Vector3(x,y,z),chunkSize,material,floorParam,caveParam,planetOrigin,isInterpolate));
                }
            }
        }
    }
}
