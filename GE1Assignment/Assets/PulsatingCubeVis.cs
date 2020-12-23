using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingCubeVis : MonoBehaviour
{
    public int cubeCount = 30;
    public float cubeMinSize = 0.3f;
    public float cubeMaxSize = 1.0f;
    public float scaleSpeed = 3.0f;

    public List<GameObject> pulsatingCubes = new List<GameObject>();

    private float endOfTunnelZ;
    private float ringRadius;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        ringRadius = ringSpawner.GetComponent<RingSpawner>().radius;
        // List<List<GameObject>> endRings = ringSpawner.GetComponent<RingSpawner>().endRings;
        // endOfTunnelZ = endRings[endRings.Count - 1][0].transform.position.z;

        print(endOfTunnelZ);

        spawnInitialCubes();
    }

    // This method spawns in the inital cubes within the tunnel
    private void spawnInitialCubes() {
        // The cubes are spawned within half the radius of the tunnel
        float halfRadius = ringRadius/2;
        // Away from center means that the cubes will spawn slightly away from the center as to not get in the way of the camera
        float awayFromCenter = 2;
        for (int i = 0; i < cubeCount; i++) {
            float size = Random.Range(cubeMinSize, cubeMaxSize);
            float x = Random.Range(-halfRadius, halfRadius);
            float y = Random.Range(-halfRadius, halfRadius);

            // The loops will run as long as the cube's position is too close to the center
            while (x < awayFromCenter && x > -awayFromCenter) {
                x = Random.Range(-halfRadius, halfRadius);
            }
            while (y < awayFromCenter && y > -awayFromCenter) {
                y = Random.Range(-halfRadius, halfRadius);
            }
            float z = Random.Range(0, 700);

            // Cubes position is set
            GameObject cube = CreateCube(x, y, z, size);
            // The hierarchy of the objects is set and the cubes are stored under the object of this script
            cube.transform.parent = this.transform;

            // Cubes added to array to keep track of them for purposes such as moving, despawning etc.
            pulsatingCubes.Add(cube);
        }
    }

    public static float Map(float value, float r1, float r2, float m1, float m2)
    {
        float dist = value - r1;
        float range1 = r2 - r1;
        float range2 = m2 - m1;
        return m1 + ((dist / range1) * range2);
    }

    GameObject CreateCube(float x, float y, float z, float sideSize)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.tag = "PulsatingCube";
        cube.transform.localScale = new Vector3(sideSize, sideSize, sideSize);
        cube.transform.position = new Vector3(x, y, z);

        float hue = Map((x + y), -ringRadius, ringRadius, 0, 1);

        cube.GetComponent<Renderer>().material.color =
            Color.HSVToRGB(hue, 1, 1);

        cube.transform.rotation = Random.rotation;

        return cube;
    }

    void pulseCubes() {
        float amplitude = AudioAnalyzer.amplitude;
        float scale = cubeMaxSize;
        for (int i = 0; i < pulsatingCubes.Count; i++) {
            Vector3 ls = pulsatingCubes[i].transform.localScale;
            ls.x = Mathf.Lerp(ls.x, cubeMinSize + (amplitude * scale), Time.deltaTime * scaleSpeed);
            ls.y = Mathf.Lerp(ls.y, cubeMinSize + (amplitude * scale), Time.deltaTime * scaleSpeed);
            ls.z = Mathf.Lerp(ls.z, cubeMinSize + (amplitude * scale), Time.deltaTime * scaleSpeed);

            Vector3 newPos = pulsatingCubes[i].transform.localPosition;
            pulsatingCubes[i].transform.localScale = ls;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        pulseCubes();

        float halfRadius = ringRadius/2;
        int currentCubeCount = pulsatingCubes.Count;
        for (int i = currentCubeCount - 1; i > 0; i--) {
            GameObject cube = pulsatingCubes[i];
            if (cube.transform.position.z < 0) {
                Destroy(cube);
                pulsatingCubes.RemoveAt(i);

                float size = Random.Range(cubeMinSize, cubeMaxSize);
                float x = Random.Range(-halfRadius, halfRadius);
                float y = Random.Range(-halfRadius, halfRadius);
                while (x < 2 && x > -2) {
                    x = Random.Range(-halfRadius, halfRadius);
                }
                 while (y < 2 && y > -2) {
                    y = Random.Range(-halfRadius, halfRadius);
                }
                float z = 700;

                GameObject newCube = CreateCube(x, y, z, size);
                pulsatingCubes.Add(newCube);
            }
        }
    }
}
