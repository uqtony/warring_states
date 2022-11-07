using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFitToTerrain : MonoBehaviour
{
    public Terrain terrain;

    private Mesh mesh;
    private Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        StartCoroutine(fitTerrain());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    IEnumerator fitTerrain()
    {
        if (mesh == null || mesh.vertices.Length <= 0)
        {
            Debug.Log("fitTerrain failed!! mesh is null or vertices is empty!!");
            yield break;
        }

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Matrix4x4 localToWorld = transform.localToWorldMatrix;
            Vector3 worldPoint = localToWorld.MultiplyPoint3x4(vertices[i]);
            float y = terrain.SampleHeight(new Vector3(worldPoint.x, 0, worldPoint.z));
            vertices[i] = new Vector3(vertices[i].x, y, vertices[i].z);            
        }
       
    }// end fitTerrain

    void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

}
