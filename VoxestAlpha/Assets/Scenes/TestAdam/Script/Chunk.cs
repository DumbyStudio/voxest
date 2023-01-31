using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    private MarchingCube _marchingCube;

    public int chunkSize;
    public Vector3 chunkPos;
    public float scale;
    public int octaves;
    public float lacunarity;
    public float persistance;
    public int planetSize;
    public float amplificator;

    void Start()
    {
        Mesh mesh = new Mesh();
        float[] noiseMap = Noise.GeneratePlanetMap(chunkSize,chunkPos,scale,octaves,persistance,lacunarity,planetSize,amplificator);
        _marchingCube = new MarchingCube(chunkSize,noiseMap);
        MeshData meshData = _marchingCube.GetMeshData();
        
        mesh.vertices = meshData.vertices.ToArray();
        mesh.triangles = meshData.triangles.ToArray();
        mesh.normals = meshData.normals.ToArray();
        
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
    }
}
