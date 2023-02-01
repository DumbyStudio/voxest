using UnityEngine;

public class Chunk
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    private MarchingCube _marchingCube;
    public Material material;

    private GameObject _gameObject;  

    public ChunkManager chunkManager;

    public int chunkSize;
    public int planetSize;
    public Vector3 chunkPos;
    
    public float scale = 1;
    public int octaves = 3;
    public float lacunarity = 0.5f;
    public float persistance = 1.0f;
    public float amplificator;

    private float[] _noiseMap;
    private MeshData _meshData;

    public Chunk(ChunkManager chunkManager, Vector3 chunkPos, int chunkSize, int planetSize, Material material,int amplificator)
    {
        this.chunkManager = chunkManager;
        _gameObject = new GameObject();
        meshFilter = _gameObject.AddComponent<MeshFilter>();
        meshRenderer = _gameObject.AddComponent<MeshRenderer>();

        this.chunkSize = chunkSize;
        this.chunkPos = chunkPos;
        this.planetSize = planetSize;
        this.material = material;
        this.amplificator = amplificator;

        _gameObject.transform.position = this.chunkPos * chunkSize;

        GenerateNoiseMap();
        
        ComputeChunk();
        
        RenderChunk();
    }

    void GenerateNoiseMap()
    {
        _noiseMap = Noise.GeneratePlanetMap(chunkSize,chunkPos,scale,octaves,persistance,lacunarity,planetSize,amplificator);
    }

    void ComputeChunk()
    {
        _marchingCube = new MarchingCube(chunkSize,_noiseMap);
        _meshData = _marchingCube.GetMeshData();

    }

    void RenderChunk()
    {
        Mesh mesh = new Mesh();
        
        mesh.vertices = _meshData.vertices.ToArray();
        mesh.triangles = _meshData.triangles.ToArray();
        mesh.normals = _meshData.normals.ToArray();

        meshRenderer.material = material;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
