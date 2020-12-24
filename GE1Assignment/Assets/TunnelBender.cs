using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBender : MonoBehaviour
{
    public GameObject prefab;
    public GameObject mainCamera;
    public float creationSpeed = 2.0f;
    public int startSec = 30;
    public int timeBetweenBends = 90;
    public float tunnelDurationSec = 5.0f;
    public float oscillationSpeed = 10.0f;
    private bool isEnabled = false;
    private float untilNextOscillation;
    private float lastOscillation;
    private GameObject ringSpawnerObj;
    List<List<GameObject>> movableRingsList;
    // Amplitude of sine wave
    private float amplitudeY = 30.0f;
    // Omega variable is used to detemine the frequency of the wave
    private float omegaY = 0.05f;
    // globIndex is used to track the position of the rings on the sine wave 
    // so that it updates as the rings move forward
    private float globIndex;
    // The rings will be gradually bent when the transition starts
    float ringBendCount = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        untilNextOscillation = startSec;

        // Get the spawner objects as the rings will be worked with
        ringSpawnerObj = GameObject.FindWithTag("RingSpawner");
        RingSpawner ringSpawner = ringSpawnerObj.GetComponent<RingSpawner>();
        movableRingsList = ringSpawner.movableRingsList;
    }

    // Object1 looks at another object
    void LoopAtLerp(GameObject object1, GameObject gameObjectToLookAt, float lerpSpeed)
    {
        Vector3 relativePos = gameObjectToLookAt.transform.position - object1.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        object1.transform.rotation = Quaternion.Lerp(object1.transform.rotation, toRotation, lerpSpeed * Time.deltaTime );
    }

    // Gaps between ring segments doesn't look good. These gaps are removed while
    // the tunnel is bending so that it looks smooth. This is achived by restoring the segment
    // to it's original "prefab" size
    void resetSegmentSize() {
        Vector3 originalScale = prefab.transform.localScale;
        float ySegmentGap = 0.3f;
        float lerpSpeed = 10.0f;

        // Iterate over each segment and lerp the scale
        for (int i = 0; i < (int) ringBendCount; i++) {
            List<GameObject> ringSegments = movableRingsList[i];

            foreach (GameObject segment in ringSegments) {
                Vector3 ls = segment.transform.localScale;
                ls.x = Mathf.Lerp(ls.x, originalScale.x, Time.deltaTime * lerpSpeed);
                ls.y = Mathf.Lerp(ls.y, originalScale.y - ySegmentGap, Time.deltaTime * lerpSpeed);
                ls.z = Mathf.Lerp(ls.z, originalScale.z, Time.deltaTime * lerpSpeed);
                segment.transform.localScale = ls;
            }
        }
    }

    // Tunnel transitions from a "linear" looking tunnel to one that is bending
    void transitionToOscillation() {
        for (int i = 0; i < (int) ringBendCount; i++) {
            float index = (float) i + globIndex;
            List<GameObject> ringSegments = movableRingsList[i];
            GameObject segmentParent = ringSegments[0].transform.parent.gameObject;
            float localZ = segmentParent.transform.localPosition.z;
            index += Time.deltaTime;
            globIndex += Time.deltaTime / oscillationSpeed;
            float y = amplitudeY * Mathf.Sin (omegaY * index);

            Vector3 wantedPosition =  new Vector3(0, -y, localZ);
            Vector3 lerpedPosition = Vector3.Lerp(segmentParent.transform.position, wantedPosition, Time.deltaTime * creationSpeed);
            segmentParent.transform.position = lerpedPosition;
        }

        if (ringBendCount < movableRingsList.Count - 20) {
            ringBendCount += Time.deltaTime * 20.0f;
        }
    }

    // The camera position needs to be set on the x and y axies as the tunnel oscillates
    void setCamera(float posLerpSpeed) {
        Vector3 camPos = mainCamera.transform.position;
        GameObject currentRing =  movableRingsList[2][0].transform.parent.gameObject;
        float ringPosX = currentRing.transform.position.x;
        float ringPosY = currentRing.transform.position.y;
        Vector3 newCamPos = new Vector3(ringPosX, ringPosY, camPos.z);
        mainCamera.transform.position = Vector3.Lerp(camPos, newCamPos, Time.deltaTime * posLerpSpeed);

        GameObject lookAtRing =  movableRingsList[20][0].transform.parent.gameObject;
        LoopAtLerp(mainCamera, lookAtRing, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // If it is time to enable the tunnel bending
        if (Time.time > untilNextOscillation) {
            untilNextOscillation += timeBetweenBends + tunnelDurationSec;
            lastOscillation = Time.time;
            isEnabled = true;
        }

        if (isEnabled) {
            resetSegmentSize();
            transitionToOscillation();
            setCamera(3.0f);

            if (Time.time > lastOscillation + tunnelDurationSec) {
                isEnabled = false;
            }

        } 
        // Gradually restore the rings to their original position along the y-axis
        else if (isEnabled == false) {
            
            float spawnerY = ringSpawnerObj.transform.position.y;
            for (int i = 0; i < movableRingsList.Count - 1; i++) {
                List<GameObject> ringSegmentsArray = movableRingsList[i];
                GameObject ringHolder = ringSegmentsArray[0].transform.parent.gameObject;
                Vector3 currentRingPos = ringHolder.transform.position;

                Vector3 wantedPosition =  new Vector3(currentRingPos.x, spawnerY, currentRingPos.z);
                Vector3 lerpedPosition = Vector3.Lerp(currentRingPos, wantedPosition, Time.deltaTime * 0.3f);
                ringHolder.transform.position = lerpedPosition;

            }
            setCamera(3.0f);
        }
    }
}
