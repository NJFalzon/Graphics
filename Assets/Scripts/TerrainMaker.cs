using UnityEngine;


[ExecuteInEditMode]
public class TerrainMaker : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    [SerializeField]
    private bool generateTerrain = true;

    [SerializeField]
    private bool generatePerlinNoiseTerrain = false;

    [SerializeField]
    private bool flattenTerrain = false;

    [SerializeField]
    private float minRandomHeightRange = 0f;

    [SerializeField]
    private float maxRandomHeightRange = 0.01f;

    [SerializeField]
    private float perlinNoiseWidthScale = 0.01f;

    [SerializeField]
    private float perlinNoiseHeightScale = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        CreateTerrain();
    }

    private void OnValidate()
    {
        if (flattenTerrain)
        {
            generateTerrain = false;
            generatePerlinNoiseTerrain = false;
        }

        if(generateTerrain || flattenTerrain || generatePerlinNoiseTerrain)
        {
            CreateTerrain();
        }
    }

  
    void CreateTerrain()
    {
        //creates a new empty 2D array of float based on the dimensions of heightmap resolution set in the settings
        //float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        ////gets the height map data that already exists in the terrain and loads it into a 2D array
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (generateTerrain)
                {
                    heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }

                if (generatePerlinNoiseTerrain)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
                }

                if (flattenTerrain)
                {
                    heightMap[width, height] = 0;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }


}
