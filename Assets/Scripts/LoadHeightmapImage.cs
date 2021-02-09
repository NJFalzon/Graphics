using System.Collections.Generic;
using UnityEngine;

public class LoadHeightmapImage : MonoBehaviour
{
    private TerrainData terrainData;
    [SerializeField] Texture2D heightMapImage;

    [Space]

    [SerializeField] GameObject water;
    [SerializeField, Range(0,1)] float waterHeight;

    [Space]

    [SerializeField] Transform cloudParent;
    [SerializeField] List<GameObject> cloud;
    [SerializeField] Material rainMat;
    [SerializeField, Range(0, 100)] int cloudCoverage;

    [Space]

    [SerializeField] List<TreeData> treeDataList;
    [SerializeField] int maxTrees = 2000;
    [SerializeField] int treeSpacing = 10;
    [SerializeField] float randomXRange = 5.0f;
    [SerializeField] float randomZRange = 5.0f;
    [SerializeField] int terrainLayerIndex = 8;

    [Space]

    [SerializeField] Vector3 heightMapScale = new Vector3(1, 1, 1);
    [SerializeField] bool loadHeightMap = true;

    void Start()
    {
        terrainData = Terrain.activeTerrain.terrainData;

        UpdateHeightmap();
        AddWater();
        AddClouds();
        AddTrees();
    }

    void UpdateHeightmap()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for(int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for(int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (loadHeightMap)
                {

                    heightMap[width, height] = heightMapImage.GetPixel((int)(width * heightMapScale.x),
                                                                       (int)(height * heightMapScale.z)).grayscale
                                                                       * heightMapScale.y;
                }
            }
        }

        terrainData.SetHeights(0 , 0, heightMap);
    }

    void AddWater()
    {
        GameObject waterGameObject = GameObject.Find("Water");

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "Water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y,
            terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    void AddClouds()
    {
        for(int i = 0; i < cloudCoverage; i++)
        {
            float randomX = Random.Range(transform.position.x, terrainData.size.x);
            float y = 1 * terrainData.size.y;
            float randomZ = Random.Range(transform.position.x, terrainData.size.z);
            GameObject tempCloud = Instantiate(cloud[Random.Range(0, cloud.Count-1)], new Vector3(randomX, y, randomZ), Quaternion.identity);
            tempCloud.transform.parent = cloudParent;
            AddRain(tempCloud.transform);
        }

        if (cloudParent.childCount > cloudCoverage)
        {
            for(int i = cloudCoverage; i < cloudParent.childCount; i++)
            {
                Destroy(cloudParent.GetChild(i).gameObject);
            }
        }
    }

    void AddTrees()
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        for (int z = 0; z < terrainData.size.z; z += treeSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
            {
                for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                {
                    if (treeInstanceList.Count < maxTrees)
                    {
                        float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                        if (currentHeight >= treeDataList[treePrototypeIndex].minHeight &&
                           currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                        {
                            float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            TreeInstance treeInstance = new TreeInstance();

                            treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                            Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                treeInstance.position.y * terrainData.size.y,
                                                                treeInstance.position.z * terrainData.size.z) + this.transform.position;



                            RaycastHit raycastHit;
                            int layerMask = 1 << terrainLayerIndex;

                            if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                            {
                                float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);

                                treeInstance.rotation = Random.Range(0, 360);
                                treeInstance.prototypeIndex = treePrototypeIndex;
                                treeInstance.color = Color.white;
                                treeInstance.lightmapColor = Color.white;
                                treeInstance.heightScale = 0.95f;
                                treeInstance.widthScale = 0.95f;

                                treeInstanceList.Add(treeInstance);
                            }
                        }
                    }
                }
            }
        }
        

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

    void AddRain(Transform cloud)
    {
        for (int i = 0; i < cloud.transform.childCount; i++)
        {
            ParticleSystem rain = cloud.GetChild(i).gameObject.AddComponent<ParticleSystem>();
            var main = rain.main;
            main.maxParticles = 10000;

            var emission = rain.emission;
            emission.enabled = true;
            emission.rateOverTime = Random.Range(50, 100);

            var shape = rain.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.MeshRenderer;
            print(cloud.GetChild(i).GetComponent<MeshFilter>().mesh);
            shape.meshRenderer = cloud.GetChild(i).GetComponent<MeshRenderer>();

            var force = rain.forceOverLifetime;
            force.enabled = true;
            force.y = -50;

            var color = rain.colorOverLifetime;
            color.enabled = true;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            color.color = grad;

            var collide = rain.collision;
            collide.enabled = true;
            collide.type = ParticleSystemCollisionType.World;
            collide.bounce = 0;

            var render = rain.GetComponent<ParticleSystemRenderer>();
            render.renderMode = ParticleSystemRenderMode.Stretch;
            render.material = rainMat;
        }
    }
}
