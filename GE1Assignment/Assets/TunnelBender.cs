using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBender : MonoBehaviour
{
    public GameObject prefab;
    public GameObject mainCamera;
    public bool isEnabled = false;
    public float creationSpeed = 2.0f;
    public int startSec = 30;
    public int timeBetweenBends = 90;
    public float tunnelDurationSec = 5.0f;
    private float untilNextOscillation;
    private float lastOscillation;
    private GameObject ringSpawnerObj;
    List<List<GameObject>> movableRingsList;
    private float amplitudeY = 30.0f;
    private float omegaY = 0.05f;
    private float globIndex;

    // Start is called before the first frame update
    void Start()
    {
        untilNextOscillation = startSec;
        ringSpawnerObj = GameObject.FindWithTag("RingSpawner");
        RingSpawner ringSpawner = ringSpawnerObj.GetComponent<RingSpawner>();
        movableRingsList = ringSpawner.movableRingsList;
    }

    void LoopAtLerp(GameObject object1, GameObject gameObjectToLookAt, float lerpSpeed)
    {
        Vector3 relativePos = gameObjectToLookAt.transform.position - object1.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        object1.transform.rotation = Quaternion.Lerp(object1.transform.rotation, toRotation, lerpSpeed * Time.deltaTime );
    }

    float ringBendCount = 1.0f;

    void resetSegmentSize() {
        Vector3 originalScale = prefab.transform.localScale;
        float ySegmentGap = 0.3f;
        float lerpSpeed = 10.0f;

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

    void transitionToOscillation() {
        for (int i = 0; i < (int) ringBendCount; i++) {
            float index = (float) i + globIndex;
            List<GameObject> ringSegments = movableRingsList[i];
            GameObject segmentParent = ringSegments[0].transform.parent.gameObject;
            float localZ = segmentParent.transform.localPosition.z;
            index += Time.deltaTime;
            globIndex += Time.deltaTime / 10;
            float y = amplitudeY * Mathf.Sin (omegaY * index);

            Vector3 wantedPosition =  new Vector3(0, -y, localZ);
            Vector3 lerpedPosition = Vector3.Lerp(segmentParent.transform.position, wantedPosition, Time.deltaTime * creationSpeed);
            segmentParent.transform.position = lerpedPosition;
        }

        if (ringBendCount < movableRingsList.Count - 20) {
            ringBendCount += Time.deltaTime * 20.0f;
        }
    }

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

        } else if (isEnabled == false) {
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
