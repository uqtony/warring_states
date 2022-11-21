#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(LineRenderer))]
public class MeshFitToTerrain : MonoBehaviour
{
    public const string MESH_PATH = "Terrain/Areas/";
    public Terrain terrain;

    public bool create_mesh_with_edges_and_save_to_files = false;

    private Mesh mesh;

    public static void saveMeshToFile(Mesh mesh, string name)
    {
        var assetsPath = "Assets/";
        var saveFolder = MESH_PATH;
        var savePath = assetsPath + saveFolder + name + ".asset";
       
        try
        {
            AssetDatabase.CreateAsset(mesh, savePath);
            Debug.Log("Save mesh completed! [" + savePath + "]");
        }
        catch(UnityException e)
        {
            Debug.Log(e.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        if (create_mesh_with_edges_and_save_to_files)
            StartCoroutine(createMeshAndEdgesToFitTerrain());
        else
            StartCoroutine(meshFitTerrain());
    }

    IEnumerator meshFitTerrain()
    {
        if (mesh == null)
            yield break;
        mesh.vertices = fitTerrain(mesh, true);
        mesh.RecalculateNormals();
    }

    IEnumerator  createMeshAndEdgesToFitTerrain()
    {
        mesh.vertices = fitTerrain(mesh, true);
        mesh.RecalculateNormals();
        string mesh_name_prefix = "China/";
        string mesh_name = mesh_name_prefix+"/"+transform.parent.parent.name + "/" + transform.parent.name;
        saveMeshToFile(mesh, mesh_name + "_area");
        yield return new WaitForEndOfFrame();

        Mesh lineMesh = generateMeshFromVertices(findEdges(mesh));
        saveMeshToFile(lineMesh, mesh_name + "_edges");
    }

    Vector3[] findEdges(Mesh mesh)
    {
        List<EdgeFinder.Edge> edges = EdgeFinder.SortEdges(EdgeFinder.FindBoundary(EdgeFinder.GetEdges(mesh.triangles)));
        Vector3[] vertices = new Vector3[edges.Count * 2];

        Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
        int i = 0;
        foreach (EdgeFinder.Edge edge in edges)
        {
            //vertices[i] = localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[edge.v1]);
            //vertices[i + 1] = localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[edge.v2]);
            vertices[i] = mesh.vertices[edge.v1];
            vertices[i + 1] = mesh.vertices[edge.v2];
            i += 2;
        }
        return vertices;
    }

    Mesh generateMeshFromVertices(Vector3[] vertices)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;
        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
        Mesh lineMesh = new Mesh();
        lineRenderer.BakeMesh(lineMesh);
        lineMesh.vertices = fitTerrain(lineMesh, true);
        lineMesh.RecalculateNormals();
        return lineMesh;
    }

    Vector3[] fitTerrain(Mesh mesh, bool needTransferLocalToWorld = true)
    {
        if (mesh == null || mesh.vertices.Length <= 0)
        {
            Debug.Log("fitTerrain failed!! mesh is null or vertices is empty!!");
            return null;
        }
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 tempPoint = vertices[i];
            if (needTransferLocalToWorld)
            {
                Matrix4x4 localToWorld = transform.localToWorldMatrix;
                tempPoint = localToWorld.MultiplyPoint3x4(vertices[i]);
            }
            
            float y = terrain.SampleHeight(new Vector3(tempPoint.x, 0, tempPoint.z));
            vertices[i] = new Vector3(vertices[i].x, y, vertices[i].z);
        }
        return vertices;
    }// end fitTerrain

}// end class MeshFitToTerrain
#endif