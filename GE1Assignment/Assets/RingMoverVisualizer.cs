using System.Collections;
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
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        movableRingsList = ringSpawner.GetComponent<RingSpawner>().movableRingsList;
        endRings = ringSpawner.GetComponent<RingSpawner>().endRings;

        
        GameObject pulsatingCubesVis = GameObject.FindWithTag("PulsatingCubesHolder");
        pulsatingCubesList = pulsatingCubesVis.GetComponent<PulsatingCubeVis>().pulsatingCubes;
        
    }

    void moveRingsForward(float speed) {
        // Move the segments along the z-axis each frame
        foreach (List<GameObject> ringSegments in movableRingsList) {
            foreach (GameObject segment in ringSegments) {
                segment.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
            }
        }

        foreach (GameObject pulsatingCube in pulsatingCubesList) {
            pulsatingCube.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
        }
    }

    void lerpScaleRingSegments(List<List<GameObject>> segmentList, float scale, float minScale, int axis, float lerpSpeed) {
        foreach (List<GameObject> elements in segmentList) {
            for (int i = 0; i < elements.Count; i++) {
                int pos = (int) (i / elements.Count) * AudioAnalyzer.bands.Length;

                Vector3 ls = elements[i].transform.localScale;
                ls[axis]= Mathf.Lerp(ls[axis], minScale + (AudioAnalyzer.bands[pos] * scale), Time.deltaTime * lerpSpeed);

                Vector3 newPos = elements[i].transform.localPosition;
                elements[i].transform.localScale = ls;
            }
        }
    }

    // Update is called once per frame
    void Update () {

        float amplitude = AudioAnalyzer.amplitude;
        lerpScaleRingSegments(movableRingsList, segmentXScale, 1.0f, 0, segmentLerpSpeed);
        lerpScaleRingSegments(movableRingsList, segmentZScale, 1.0f, 2, segmentLerpSpeed);

        GameObject ringSegmentsHolder = GameObject.FindWithTag("TunnelHolder");
        float thetaInc = Mathf.PI * 2.0f;
        float theta = thetaInc * amplitude;
        Quaternion toRotation = ringSegmentsHolder.transform.rotation;
        toRotation *= Quaternion.Euler(0, 0, 0.1f);
        ringSegmentsHolder.transform.rotation = Quaternion.Lerp(ringSegmentsHolder.transform.rotation, toRotation, Time.deltaTime * ringRotateSpeed);
        
        float speed = defaultMoveSpeed + amplitude * scaleMoveSpeed;
        moveRingsForward(speed);
	}
}
