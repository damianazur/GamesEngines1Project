using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RingSpawner : MonoBehaviour
{
    public int radius = 10;

    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 point = new Vector3(0, 0, 0);

        float cubeY = prefab.transform.localScale.y;
        float circumfrence = (2.0f * Mathf.PI * radius * 0.95f);
        int segments = (int) (Mathf.Floor(circumfrence / cubeY));

        CreateTriangle(new Vector3(0, 0, 0), new Vector3(0, 5, 0), new Vector3(5, 5, 0));

        float thetaInc = (Mathf.PI * 2.0f) / (float)segments;
        float theta = Mathf.PI * 2.0f / ((float) segments);
        float z = 0.0f;
        for (int j = 0 ; j < segments ; j ++)
        {
            float angle  = (j * theta);

            float x = (Mathf.Sin(angle) * radius) + point.x;
            float y = (Mathf.Cos(angle) * radius) + point.y;

            GameObject cube = GameObject.Instantiate<GameObject>(prefab);
                cube.transform.position = new Vector3(x, y, z);

            Vector3 target = point;
            Vector3 relativePos = target - cube.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.right);
            cube.transform.rotation = rotation;

            cube.GetComponent<Renderer>().material.color =
                Color.HSVToRGB(j / (float) segments, 1, 1);
                
            cube.transform.parent = this.transform;
        }
    }

    void CreateTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        GameObject tri = new GameObject();
        tri.AddComponent<MeshFilter>();
        tri.AddComponent<MeshRenderer>();
        Mesh mesh = tri.GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        mesh.vertices = new Vector3[] {v1, v2, v3};
        //mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles =  new int[] {0, 1, 2};
    }

    GameObject CreateBrick(float x, float y, float z, float xScale = 1.0f, float yScale = 1.0f, float zScale = 1.0f)
    {
        GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
        brick.transform.localScale = new Vector3(xScale, yScale, zScale);
        brick.transform.position = new Vector3(x, y, z);
        //brick.GetComponent<Renderer>().material.color = Utilities.RandomColor();
        //Rigidbody rigidBody = brick.AddComponent<Rigidbody>();
        //rigidBody.mass = 1.0f;
        return brick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
