using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RingSpawner : MonoBehaviour
{
    public int radius = 10;
    public int startingRings = 200;
    private List<List<GameObject>> ringList = new List<List<GameObject>>();

    private float moveDistance = 0;
    public GameObject prefab;

    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {   
        int endPieceCount = 0;
        for (int i = 0; i < startingRings; i++) {
            List<GameObject> ringSegments = CreateRing();
            if (i > startingRings - 50 && i % 6 == 0) {
                radius = radius - 1;
            } 

            transform.position += new Vector3(0, 0, 5);
            if (i < startingRings - 50) {
                ringList.Add(ringSegments);
            } else {
                endPieceCount += 1;
            }
        }

        transform.position -= new Vector3(0, 0, (endPieceCount + 1) * 5);
        radius = 10;

        // Add final ring so there is no gap
        CreateRing();
    }

    List<GameObject> CreateRing() {
        List<GameObject> ringSegments = new List<GameObject>();

        Vector3 point = this.transform.position;

        float cubeY = prefab.transform.localScale.y;
        float circumfrence = (2.0f * Mathf.PI * radius * 0.95f);
        int segments = (int) (Mathf.Floor(circumfrence / cubeY));

        float thetaInc = (Mathf.PI * 2.0f) / (float)segments;
        float theta = Mathf.PI * 2.0f / ((float) segments);
        float z = point.z;
        for (int j = 0 ; j < segments ; j ++)
        {
            float angle  = (j * theta);

            float x = (Mathf.Sin(angle) * radius) - point.x;
            float y = (Mathf.Cos(angle) * radius) - point.y;

            GameObject cube = GameObject.Instantiate<GameObject>(prefab);
                cube.transform.position = new Vector3(x, y, z);

            //CreateTriangle(new Vector3(0, 0, 0), new Vector3(0, 5, 0), new Vector3(5, 5, 0), cube);

            Vector3 target = point;
            Vector3 relativePos = target - cube.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.right);
            cube.transform.rotation = rotation;

            cube.GetComponent<Renderer>().material.color =
                Color.HSVToRGB(j / (float) segments, 1, 1);
            
            ringSegments.Add(cube);
            //cube.transform.parent = this.transform;
        }

        return ringSegments;
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
        float speed = 30.0f;
        foreach (List<GameObject> ringSegments in ringList) {
            foreach (GameObject segment in ringSegments) {
                segment.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
            }
        }
        moveDistance += Time.deltaTime * speed;

        // If the rings have moved a certain distance then spawn in a new ring
        if (moveDistance > 5) {
            List<GameObject> lastRing = ringList[ringList.Count - 1];
            float zCoord = lastRing[0].transform.position.z;

            // Create and synchronize rings
            // numOfRings is used to generate multiple rings in the scenario of a lag spike
            int numOfRings = (int) Mathf.Floor(moveDistance / 5);
            // There has to be at least one ring but can be more than one at a time
            if (numOfRings == 0) {
                numOfRings = 1;
            }
            // Change the position of segments in the ring
            for (int i = 0; i < numOfRings; i++) {
                List<GameObject> ringSegments = CreateRing();
                foreach (GameObject segment in ringSegments) {
                    float segX = segment.transform.position.x;
                    float segY = segment.transform.position.y;

                    // Offset the ring being the last (hence why it's "synchronising" there will be no gaps)
                    segment.transform.position = new Vector3(segX, segY, zCoord + (5 + i * 5));
                }

                ringList.Add(ringSegments);
                ringList[0].RemoveAll (delegate (GameObject o) { return o == null; });
                ringList.RemoveAt(0);
                moveDistance -= 5.0f;
            }
        }
    }
}
