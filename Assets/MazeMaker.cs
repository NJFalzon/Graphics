using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MazeMaker : MonoBehaviour
{
    [SerializeField] List<Material> materialList;
    GameObject plane;
    GameObject wall;

    void Start()
    {
        plane = new GameObject("Plane");
        plane.transform.position = Vector3.zero;
        plane.AddComponent<PlaneMaker>();
        plane.GetComponent<PlaneMaker>().AddMaterial(materialList[1]);
        plane.GetComponent<PlaneMaker>().Plane();
        plane.transform.parent = transform;

        plane = new GameObject("Plane");
        plane.transform.position = new Vector3(1, 0.01f, 1);
        plane.AddComponent<PlaneMaker>().Size(5,5);
        plane.GetComponent<PlaneMaker>().AddMaterial(materialList[2]);
        plane.GetComponent<PlaneMaker>().Plane();
        plane.transform.parent = transform;

        plane = new GameObject("Plane");
        plane.transform.position = new Vector3(1, 0.01f, 19);
        plane.AddComponent<PlaneMaker>().Size(5, 5);
        plane.GetComponent<PlaneMaker>().AddMaterial(materialList[3]);
        plane.GetComponent<PlaneMaker>().Plane();
        plane.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(0, 1, 12f);
        wall.AddComponent<CubeMaker>().size = new Vector3(1,1,13f);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(12, 1, 0);
        wall.AddComponent<CubeMaker>().size = new Vector3(13, 1, 1);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(24, 1, 12);
        wall.AddComponent<CubeMaker>().size = new Vector3(1, 1, 13);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(12, 1, 24);
        wall.AddComponent<CubeMaker>().size = new Vector3(13, 1, 1);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(8, 1, 18);
        wall.AddComponent<CubeMaker>().size = new Vector3(8, 1, 1);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(16, 1, 12);
        wall.AddComponent<CubeMaker>().size = new Vector3(8, 1, 1);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(8, 1, 6);
        wall.AddComponent<CubeMaker>().size = new Vector3(8, 1, 1);
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

    }
}
