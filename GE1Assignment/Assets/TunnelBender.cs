using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBender : MonoBehaviour
{
    List<List<GameObject>> movableRingsList;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        movableRingsList = ringSpawner.GetComponent<RingSpawner>().movableRingsList;
    }

    // Update is called once per frame
    void Update()
    {
        float angleIncrement = 0.1f;

        // List<GameObject> ringSegments = movableRingsList[20];
        // GameObject segmentParent = ringSegments[0].transform.parent.gameObject;
        // segmentParent.transform.rotation *= Quaternion.Euler(10.0f, 0, 0);

        // segmentParent.transform.RotateAround(segmentParent.transform.position, Vector3.up, 20.0f * Time.deltaTime);

        // foreach (List<GameObject> ringSegments in movableRingsList) {
        //     GameObject segmentParent = ringSegments[0].transform.parent.gameObject;
        //     segmentParent.transform.rotation *= Quaternion.Euler(0.1f, 0, 0);
        // }
    }
}
