﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMoverVisualizer : MonoBehaviour {
    List<List<GameObject>> movableRingsList;
    List<GameObject> pulsatingCubesList;
    public List<List<GameObject>> endRings;
    public float defaultMoveSpeed = 30;
    public float scaleMoveSpeed = 100;
    public float ringRotateSpeed = 200;

    public float segmentZScale = 15.0f;
    public float segmentXScale = 15.0f;
    public float segmentLerpSpeed = 5.0f;

	// Use this for initialization
	void Start () {
        // The rings are moved with the music
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        movableRingsList = ringSpawner.GetComponent<RingSpawner>().movableRingsList;
        endRings = ringSpawner.GetComponent<RingSpawner>().endRings;
        
        // The cubes will be moved with the music
        GameObject pulsatingCubesVis = GameObject.FindWithTag("PulsatingCubesHolder");
        pulsatingCubesList = pulsatingCubesVis.GetComponent<PulsatingCubeVis>().pulsatingCubes;
        
    }

    // Rings and puksating cubes will be moved.
    void moveForward(float speed) {
        // Move the segments along the z-axis each frame
        foreach (List<GameObject> ringSegments in movableRingsList) {
            // Get the ring holder
            GameObject ringHolder = ringSegments[0].transform.parent.gameObject;
            ringHolder.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
        }

        foreach (GameObject pulsatingCube in pulsatingCubesList) {
            pulsatingCube.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
        }
    }

    // Ring segments will pulse with the beat
    void lerpScaleRingSegments(List<List<GameObject>> ringList, float scale, float minScale, int axis, float lerpSpeed) {
        foreach (List<GameObject> elements in ringList) {

            for (int i = 0; i < elements.Count; i++) {
                // int pos = (int) ((i / elements.Count) * AudioAnalyzer.bands.Length);
                int pos = 1;

                Vector3 ls = elements[i].transform.localScale;
                ls[axis]= Mathf.Lerp(ls[axis], minScale + (AudioAnalyzer.bands[pos] * scale), Time.deltaTime * lerpSpeed);

                Vector3 newPos = elements[i].transform.localPosition;
                elements[i].transform.localScale = ls;
            }
        }
    }
    
    // Entire tunnel will rotate
    void rotateTunnel() {
        // Amplitude is not currently used in rotation but can be used in the future
        float amplitude = AudioAnalyzer.amplitude;
        float thetaInc = Mathf.PI * 2.0f;
        float theta = thetaInc * amplitude;

        GameObject ringSegmentsHolder = GameObject.FindWithTag("TunnelHolder");
        Quaternion toRotation = ringSegmentsHolder.transform.rotation;
        toRotation *= Quaternion.Euler(0, 0, 0.1f);
        ringSegmentsHolder.transform.rotation = Quaternion.Lerp(ringSegmentsHolder.transform.rotation, toRotation, Time.deltaTime * ringRotateSpeed);
    }

    // Update is called once per frame
    void Update () {
        float amplitude = AudioAnalyzer.amplitude;
        lerpScaleRingSegments(movableRingsList, segmentXScale, 1.0f, 0, segmentLerpSpeed);
        lerpScaleRingSegments(movableRingsList, segmentZScale, 1.0f, 2, segmentLerpSpeed);
        rotateTunnel();
        float speed = defaultMoveSpeed + amplitude * scaleMoveSpeed;
        moveForward(speed);
	}
}
