using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingCubeVis : MonoBehaviour
{
    public int cubeCount = 30;
    public float cubeMinSize = 0.3f;
    public float cubeMaxSize = 1.0f;
    public float scaleSpeed = 3.0f;

    // Cubes will not spawn within a certain range of the camera
    public float awayFromCenterZone = 2.0f;
    
    public List<GameObject> pulsatingCubes = new List<GameObject>();

    private float endOfTunnelZ;
    private float ringRadius;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        ringRadius = ringSpawner.GetComponent<RingSpawner>().radius;
        List<List<GameObject>> endRings = ringSpawner.GetComponent<RingSpawner>().endRings;
        endOfTunnelZ = endRings[endRings.Count - 1][0].transform.position.z;

        print("End of Tunnel Z-Axis: " + endOfTunnelZ);

        spawnInitialCubes();
    }

    // This method spawns in the inital cubes within the tunnel
    private void spawnInitialCubes() {
        // Away from center means that the cubes will spawn slightly away from the center as to not get in the way of the camera
        for (int i = 0; i < cubeCount; i++) {
            float size = Random.Range(cubeMinSize, cubeMaxSize);
            
            float x = genRandomCoord(ringRadius/2, awayFromCenterZone);
            float y = genRandomCoord(ringRadius/2, awayFromCenterZone);

            float z = Random.Range(0, endOfTunnelZ);

            // Cubes position is set
            GameObject cube = CreateCube(x, y, z, size);
            // The hierarchy of the objects is set and the cubes are stored under the object of this script
            cube.transform.parent = this.transform;

            // Cubes added to array to keep track of them for purposes such as moving, despawning etc.
            pulsatingCubes.Add(cube);
        }
    }

    public float genRandomCoord(float radius, float noSpawnZone) {
        float coord = Random.Range(-radius, radius);

        // The loops will run as long as the cube's position is too close to the center
        while (coord < noSpawnZone && coord > -noSpawnZone) {
            coord = Random.Range(-radius, radius);
        }

        return coord;
    }

    // Creates the pulsating cube
    GameObject CreateCube(float x, float y, float z, float sideSize)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.tag = "PulsatingCube";
        cube.transform.localScale = new Vector3(sideSize, sideSize, sideSize);
        cube.transform.position = new Vector3(x, y, z);

        // Hue is determined by the position of the cube (this makes the visualizer less messy looking)
        float hue = Utilities.Map((x + y), -ringRadius, ringRadius, 0, 1);

        cube.GetComponent<Renderer>().material.color =
            Color.HSVToRGB(hue, 1, 1);
        // A random rotation is applied to add variation
        cube.transform.rotation = Random.rotation;

        return cube;
    }

    // The cubes are scaled over time to the amplitude of the music
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

        // Iterate over cubes and destroy the ones that are no longer within the player's FOV
        // Spawn new cubes to replace the old ones
        for (int i = currentCubeCount - 1; i > 0; i--) {
            GameObject cube = pulsatingCubes[i];
            // If cube is behind player
            if (cube.transform.position.z < 0) {
                // Destroy the object and free up the memory 
                Destroy(cube);
                // Remove from the array
                pulsatingCubes.RemoveAt(i);

                float size = Random.Range(cubeMinSize, cubeMaxSize);
                float x = Random.Range(-halfRadius, halfRadius);
                float y = Random.Range(-halfRadius, halfRadius);
                while (x < awayFromCenterZone && x > -awayFromCenterZone) {
                    x = Random.Range(-halfRadius, halfRadius);
                }
                 while (y < awayFromCenterZone && y > -awayFromCenterZone) {
                    y = Random.Range(-halfRadius, halfRadius);
                }
                float z = endOfTunnelZ;

                GameObject newCube = CreateCube(x, y, z, size);
                pulsatingCubes.Add(newCube);
            }
        }
    }
}
