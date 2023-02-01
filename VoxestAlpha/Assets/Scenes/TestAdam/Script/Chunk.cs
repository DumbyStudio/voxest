using UnityEngine;

public class Chunk
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    private MarchingCube _marchingCube;

    private GameObject _gameObject;  

    public ChunkManager chunkManager;

    public int chunkSize;
    public Vector3 chunkPos;
    public float scale = 1;
    public int octaves = 3;
    public float lacunarity = 0.5f;
    public float persistance = 1.0f;
    public int planetSize = 10;
    public float amplificator = 2;

    public Chunk(ChunkManager chunkManager, Vector3 chunkPos, int chunkSize, int planetSize)
    {
        this.chunkManager = chunkManager;
        _gameObject = new GameObject();
        meshFilter = _gameObject.AddComponent<MeshFilter>();
        meshRenderer = _gameObject.AddComponent<MeshRenderer>();

        this.chunkSize = chunkSize;
        this.chunkPos = chunkPos;
        this.planetSize = planetSize;

        _gameObject.transform.position = this.chunkPos * chunkSize;

        Start();
    }

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
