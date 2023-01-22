using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaMapGenerator : MonoBehaviour
{

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = DaNoise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        DaMapDisplay display = FindObjectOfType<DaMapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }

}
