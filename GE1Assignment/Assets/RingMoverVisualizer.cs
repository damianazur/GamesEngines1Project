using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMoverVisualizer : MonoBehaviour {
    List<List<GameObject>> movableRingsList;
    public float moveSpeed = 30;
	// Use this for initialization
	void Start () {
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        movableRingsList = ringSpawner.GetComponent<RingSpawner>().movableRingsList;
    }

    void moveRingsForward(float speed) {
        // Move the segments along the z-axis each frame
        foreach (List<GameObject> ringSegments in movableRingsList) {
            foreach (GameObject segment in ringSegments) {
                segment.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //print(movableRingsList.Count);
        float speed = 40.0f;
        moveRingsForward(speed);

        // for (int i = 0; i < elements.Count; i++) {
        //     Vector3 ls = elements[i].transform.localScale;
        //     ls.y = Mathf.Lerp(ls.y, 1 + (AudioAnalyzer.bands[i] * scale), Time.deltaTime * 3.0f);
        //     Vector3 newPos = elements[i].transform.localPosition;
        //     newPos.y = ls.y/2;
        //     elements[i].transform.localScale = ls;
        //     elements[i].transform.localPosition = newPos;
        // }

        // float amplitude = AudioAnalyzer.amplitude;
        // float thetaInc = Mathf.PI * 2.0f;
        // float theta = thetaInc * amplitude;
        // Quaternion toRotation =  transform.rotation;
        // toRotation *= Quaternion.Euler(0, 0.05f + amplitude, 0); // 0.05f as a base roation speed so that it doesn't stop abruptly
        // transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 200);
	}
}
