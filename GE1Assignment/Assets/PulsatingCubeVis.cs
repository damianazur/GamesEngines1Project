using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingCubeVis : MonoBehaviour
{
    public int cubeCount = 30;
    public float cubeMinSize = 0.1f;
    public float cubeMaxSize = 2.0f;
    public List<GameObject> pulsatingCubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawnCubes();
    }

    private void spawnCubes() {
        for (int i = 0; i < cubeCount; i++) {
            float size = Random.Range(cubeMinSize, cubeMaxSize);
            float x = Random.Range(-5, 5);
            float y = Random.Range(-5, 5);
            float z = Random.Range(0, 300);

            GameObject cube = CreateCube(x, y, z, size);
            cube.transform.parent = this.transform;

            pulsatingCubes.Add(cube);
        }
    }

    GameObject CreateCube(float x, float y, float z, float sideSize)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.tag = "cube";
        cube.transform.localScale = new Vector3(sideSize, sideSize, sideSize);
        cube.transform.position = new Vector3(x, y, z);
        // cube.GetComponent<Renderer>().material.color = Utilities.RandomColor();

        return cube;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
