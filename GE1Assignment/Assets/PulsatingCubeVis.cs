﻿using System.Collections;
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
            while (x < 1 && x > -1) {
                x = Random.Range(-5, 5);
            }
                while (y < 1 && y > -1) {
                y = Random.Range(-5, 5);
            }
            float z = Random.Range(0, 700);

            GameObject cube = CreateCube(x, y, z, size);
            cube.transform.parent = this.transform;

            pulsatingCubes.Add(cube);
        }
    }

    GameObject CreateCube(float x, float y, float z, float sideSize)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.tag = "PulsatingCube";
        cube.transform.localScale = new Vector3(sideSize, sideSize, sideSize);
        cube.transform.position = new Vector3(x, y, z);

        float hue = Random.Range(0f, 1f);

        cube.GetComponent<Renderer>().material.color =
            Color.HSVToRGB(hue, 1, 1);

        cube.transform.rotation = Random.rotation;

        return cube;
    }

    

    // Update is called once per frame
    void Update()
    {
        int currentCubeCount = pulsatingCubes.Count;
        for (int i = currentCubeCount - 1; i > 0; i--) {
            GameObject cube = pulsatingCubes[i];
            if (cube.transform.position.z < 0) {
                // Destroy(cube);
                pulsatingCubes.RemoveAt(i);

                float size = Random.Range(cubeMinSize, cubeMaxSize);
                float x = Random.Range(-5, 5);
                float y = Random.Range(-5, 5);
                while (x < 1 && x > -1) {
                    x = Random.Range(-5, 5);
                }
                 while (y < 1 && y > -1) {
                    y = Random.Range(-5, 5);
                }
                float z = 700;

                GameObject newCube = CreateCube(x, y, z, size);
                pulsatingCubes.Add(newCube);
            }
        }
    }
}