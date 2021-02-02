using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ChessboardMaker : MonoBehaviour
{
    [SerializeField]
    private float cellSize = 1f;

    [SerializeField]
    private int width = 8;

    [SerializeField]
    private int height = 8;

    [SerializeField]
    private int subMeshSize = 2;

    // Update is called once per frame
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshBuilder meshBuilder = new MeshBuilder(subMeshSize);

        CreateChessboard(meshBuilder);

        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = MaterialsList().ToArray();
    }

    private void CreateChessboard(MeshBuilder meshBuilder)
    {
        //create points of our plane
        Vector3[,] points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                points[x, y] = new Vector3(
                        cellSize * x,
                        0,
                        cellSize * y);
            }
        }

        //create the quads

        int submesh = 0;

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                submesh++;

                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //create 2 triangles that make up a quad
                meshBuilder.BuildTriangle(bl, tr, tl, submesh % subMeshSize);
                meshBuilder.BuildTriangle(bl, br, tr, submesh % subMeshSize);
            }
        }
    }


    private List<Material> MaterialsList()
    {
        List<Material> materialsList = new List<Material>();

        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;

        materialsList.Add(whiteMat);
        materialsList.Add(blackMat);

        return materialsList;
    }
}
