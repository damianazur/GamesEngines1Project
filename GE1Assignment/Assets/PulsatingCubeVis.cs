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
            while (x < 2 && x > -2) {
                x = Random.Range(-5, 5);
            }
                while (y < 2 && y > -2) {
                y = Random.Range(-5, 5);
            }
            float z = Random.Range(0, 700);

            GameObject cube = CreateCube(x, y, z, size);
            cube.transform.parent = this.transform;

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

        float hue = Map((x + y), -10, 10, 0, 1);

        cube.GetComponent<Renderer>().material.color =
            Color.HSVToRGB(hue, 1, 1);

        cube.transform.rotation = Random.rotation;

        return cube;
    }

    void pulseCubes() {
        float amplitude = AudioAnalyzer.amplitude;
        float scale = 1.0f;
        for (int i = 0; i < pulsatingCubes.Count; i++) {
            Vector3 ls = pulsatingCubes[i].transform.localScale;
            ls.x = Mathf.Lerp(ls.x, 0.3f + (amplitude * scale), Time.deltaTime * 3.0f);
            ls.y = Mathf.Lerp(ls.y, 0.3f + (amplitude * scale), Time.deltaTime * 3.0f);
            ls.z = Mathf.Lerp(ls.z, 0.3f + (amplitude * scale), Time.deltaTime * 3.0f);

            Vector3 newPos = pulsatingCubes[i].transform.localPosition;
            pulsatingCubes[i].transform.localScale = ls;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        pulseCubes();

        int currentCubeCount = pulsatingCubes.Count;
        for (int i = currentCubeCount - 1; i > 0; i--) {
            GameObject cube = pulsatingCubes[i];
            if (cube.transform.position.z < 0) {
                Destroy(cube);
                pulsatingCubes.RemoveAt(i);

                float size = Random.Range(cubeMinSize, cubeMaxSize);
                float x = Random.Range(-5, 5);
                float y = Random.Range(-5, 5);
                while (x < 2 && x > -2) {
                    x = Random.Range(-5, 5);
                }
                 while (y < 2 && y > -2) {
                    y = Random.Range(-5, 5);
                }
                float z = 700;

                GameObject newCube = CreateCube(x, y, z, size);
                pulsatingCubes.Add(newCube);
            }
        }
    }
}
