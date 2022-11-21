#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ShiftVertices : MonoBehaviour
{
    public Vector3 new_center;
    Vector3[] vertices;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        shiftVertices();
    }

    private void Update()
    {
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    void logMesh(Mesh mesh)
    {
        Debug.Log("[ShiftVertices]mesh uv=" + mesh.uv);
        Debug.Log("[ShiftVertices]mesh bound=" + mesh.bounds);
    }

    void shiftVertices()
    {
        if (new_center == null)
            return;

        mesh = GetComponent<MeshFilter>().mesh;
        logMesh(mesh);
        vertices = mesh.vertices;
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] -= new_center;
        }
        Bounds bounds = mesh.bounds;
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        center.y -= extents.y;
        extents.y = 0;
        bounds.center = center;
        bounds.extents = extents;
        mesh.bounds = bounds;

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = mesh.triangles;
        newMesh.colors = mesh.colors;
        newMesh.bounds = mesh.bounds;
        newMesh.tangents = mesh.tangents;
        newMesh.normals = mesh.normals;
        newMesh.uv = mesh.uv;
        saveMesh(newMesh, name);
    }

    void saveMesh(Mesh mesh, string saveName)
    {       
        var savePath = "Assets/" + saveName + ".asset";
        Debug.Log("Saved Mesh to:" + savePath);
        AssetDatabase.CreateAsset(mesh, savePath);
    }//end saveMesh
}
#endif