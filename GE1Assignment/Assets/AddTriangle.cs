using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTriangle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject tri = new GameObject();
        tri.transform.parent = this.transform;

        Vector3 pos1 = this.transform.TransformPoint(0.5f, 0.5f, -0.5f);
        Vector3 pos2 = this.transform.TransformPoint(0.5f, 0.5f, -0.5f);
        Vector3 pos3 = this.transform.TransformPoint(-0.5f, 1, -0.5f);

        tri.AddComponent<MeshFilter>();
        tri.AddComponent<MeshRenderer>();
        Mesh mesh = tri.GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        print(pos1);

        //mesh.vertices = new Vector3[] {new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 1, -0.5f)};
        mesh.vertices = new Vector3[] {pos1, pos2, pos3};
        //mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles =  new int[] {0, 1, 2};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
