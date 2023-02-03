using UnityEngine;

public struct Voxel
{
    public float density;
    public float type;
}

public class Chunk
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    private MarchingCube _marchingCube;
    public Material material;

    private GameObject _gameObject;  

    public ChunkManager chunkManager;

    public int chunkSize;
    public Vector3 chunkPos;

    private NoiseParam _noiseParam;
    private Vector3 _planetOrigin;

    private Voxel[] _noiseMap;
    private MeshData _meshData;

    public Chunk(ChunkManager chunkManager, Vector3 chunkPos, int chunkSize, Material material, NoiseParam noiseParam, Vector3 planetOrigin)
    {
        this.chunkManager = chunkManager;
        _gameObject = new GameObject();
        meshFilter = _gameObject.AddComponent<MeshFilter>();
        meshRenderer = _gameObject.AddComponent<MeshRenderer>();

        this.chunkSize = chunkSize;
        this.chunkPos = chunkPos;
        _noiseParam = noiseParam;
        _planetOrigin = planetOrigin;
        this.material = material;

        _gameObject.transform.position = this.chunkPos * chunkSize;

        GenerateNoiseMap();
        
        ComputeChunk();
        
        RenderChunk();
    }

    void GenerateNoiseMap()
    {
        _noiseMap = Noise.GeneratePlanetMap(chunkSize,chunkPos,_planetOrigin,_noiseParam);
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
        mesh.colors = _meshData.colors.ToArray();

        meshRenderer.material = material;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
