using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RingSpawner : MonoBehaviour
{
    public int radius = 10;
    public int startingRings = 1;

    public GameObject prefab;

    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startingRings; i++) {
            print("Pos: " + transform.position);
            CreateRing();
            transform.position += new Vector3(0, 0, 5);
        }
    }

    void CreateRing() {
        Vector3 point = this.transform.position;

        float cubeY = prefab.transform.localScale.y;
        float circumfrence = (2.0f * Mathf.PI * radius * 0.95f);
        int segments = (int) (Mathf.Floor(circumfrence / cubeY));

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

            //CreateTriangle(new Vector3(0, 0, 0), new Vector3(0, 5, 0), new Vector3(5, 5, 0), cube);


            Vector3 target = new Vector3(0, 0, 0);
            Vector3 relativePos = target - cube.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.right);
            cube.transform.rotation = rotation;

            cube.GetComponent<Renderer>().material.color =
                Color.HSVToRGB(j / (float) segments, 1, 1);
                
            cube.transform.parent = this.transform;
        }

    }

    void CreateTriangle(Vector3 v1, Vector3 v2, Vector3 v3, GameObject cube) {
        // GameObject tri = new GameObject();
        // tri.transform.parent = cube.transform;

        // Quaternion rot = cube.transform.rotation;

        // Vector3 pos1 = cube.transform.TransformPoint(0.5f, 0.5f, -0.5f);
        // Vector3 pos2 = cube.transform.TransformPoint(-0.5f, 0.5f, -0.5f);
        // Vector3 pos3 = cube.transform.TransformPoint(-0.5f, 1, -0.5f);

        // tri.AddComponent<MeshFilter>();
        // tri.AddComponent<MeshRenderer>();
        // Mesh mesh = tri.GetComponent<MeshFilter>().mesh;

        // mesh.Clear();

        // print(pos1);

        // //mesh.vertices = new Vector3[] {new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 1, -0.5f)};
        // mesh.vertices = new Vector3[] {pos1, pos2, pos3};
        // //mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        // mesh.triangles =  new int[] {0, 1, 2};
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
