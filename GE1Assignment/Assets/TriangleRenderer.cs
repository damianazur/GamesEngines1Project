using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TriangleRenderer : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        // make changes to the Mesh by creating arrays which contain the new values
        mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 10, 0), new Vector3(10, 10, 0)};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles =  new int[] {0, 1, 2};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
