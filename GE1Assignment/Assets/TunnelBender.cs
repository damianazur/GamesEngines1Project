using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBender : MonoBehaviour
{
    List<List<GameObject>> movableRingsList;
    public GameObject mainCamera;

    private float amplitudeX = 10.0f;
    private float amplitudeY = 30.0f;
    private float omegaX = 1.0f;
    private float omegaY = 0.05f;
    private float index;
    private float globIndex;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        movableRingsList = ringSpawner.GetComponent<RingSpawner>().movableRingsList;
    }

    void LoopAtLerp(GameObject object1, GameObject gameObjectToLookAt, float lerpSpeed)
    {
        Vector3 relativePos = gameObjectToLookAt.transform.position - object1.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        object1.transform.rotation = Quaternion.Lerp(object1.transform.rotation, toRotation, lerpSpeed * Time.deltaTime );
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < movableRingsList.Count - 10; i++) {
            float index = (float) i + globIndex;
            List<GameObject> ringSegments = movableRingsList[i];
            GameObject segmentParent = ringSegments[0].transform.parent.gameObject;
            float localZ = segmentParent.transform.localPosition.z;
            index += Time.deltaTime;
            globIndex += Time.deltaTime / 10;
            float y = amplitudeY * Mathf.Sin (omegaY * index);

            Vector3 wantedPosition =  new Vector3(0, -y, localZ);
            Vector3 lerpedPosition = Vector3.Lerp(segmentParent.transform.position, wantedPosition, Time.deltaTime * 1.0f);
            segmentParent.transform.position = lerpedPosition;

            if (i > 0) {
                LoopAtLerp(segmentParent, movableRingsList[i-1][0].transform.parent.gameObject, 2);
            }
        }
    }
}
