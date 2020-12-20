using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RingSpawner : MonoBehaviour
{
    public float radius = 10;
    public int noOfSegments = 20;
    public int startingRings = 100;
    public List<List<GameObject>> movableRingsList = new List<List<GameObject>>();
    public List<List<GameObject>> endRings = new List<List<GameObject>>();

    private float movedDistance = 0;
    private int positionOffset;
    public GameObject prefab;

    void Awake() {
        // Distance between each ring is the width of the ring * 2 + 1 (gap)
        positionOffset = (int) prefab.transform.localScale.z * 2 + 1;
        print("POS OFFSET" + positionOffset);
    }

    // Start is called before the first frame update
    void Start()
    {   
        int finalRingCount = 50;
        // Create the initial rings
        for (int i = 0; i < startingRings; i++) {
            List<GameObject> ringSegments = CreateRing();

            // Final end rings that don't respawn
            if (i > startingRings - finalRingCount) {
                radius = radius - (float) 0.1f;
            } 

            // Move the spawner forward
            transform.position += new Vector3(0, 0, positionOffset);
            // If not final rings
            if (i < startingRings - finalRingCount) {
                movableRingsList.Add(ringSegments);
            } else {
                endRings.Add(ringSegments);
            }
        }

        int endPieceCount = endRings.Count;
        transform.position -= new Vector3(0, 0, (endPieceCount + 1) * positionOffset);
        radius = 10;

        // Add final ring so there is no gap
        CreateRing();
    }

    List<GameObject> CreateRing() {
        // The ring is made up of rectangles (segments)
        List<GameObject> ringSegments = new List<GameObject>();

        // Center point of the circle (segments will rotate towards it)
        Vector3 centerPoint = this.transform.position;

        // Calculate the number of segments needed to fill the radius of the circle
        float cubeGap = 1;
        float cubeY = prefab.transform.localScale.y;
        float circumfrence = (2.0f * Mathf.PI * (radius - cubeGap));
        float yScale = (float) (circumfrence / noOfSegments);
        float sizeScale = yScale / cubeY;

        // Get the angle of the segment on the circle (2 pi = 360 degress => 360 / noOfSegments = angle for of each segment)
        float theta = Mathf.PI * 2.0f / ((float) noOfSegments);
        // The rings move along the z-axis
        float z = centerPoint.z;
        // print("Segments: " + segments);
        for (int j = 0 ; j < noOfSegments; j ++)
        {
            float angle  = (j * theta);
            float x = (Mathf.Sin(angle) * radius) - centerPoint.x;
            float y = (Mathf.Cos(angle) * radius) - centerPoint.y;

            GameObject cube = GameObject.Instantiate<GameObject>(prefab);
                cube.transform.position = new Vector3(x, y, z);

            float xScale = prefab.transform.localScale.z;
            if (endRings.Count > 0 && endRings.Count < 50) {
                xScale = xScale + (endRings.Count * 4);
            }
            cube.transform.localScale = new Vector3(
                xScale,
                yScale,
                prefab.transform.localScale.z);

            cube.gameObject.tag = "RingSegment";

            Vector3 target = centerPoint;
            Vector3 relativePos = target - cube.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.right);
            cube.transform.rotation = rotation;

            cube.GetComponent<Renderer>().material.color =
                Color.HSVToRGB(j / (float) noOfSegments, 1, 1);
            
            ringSegments.Add(cube);
        }

        return ringSegments;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 10.0f;
        // Move the segments along the z-axis each frame
        foreach (List<GameObject> ringSegments in movableRingsList) {
            foreach (GameObject segment in ringSegments) {
                segment.transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
            }
        }

        // Calculate how far the rings have moved so that more rings can be spawned
        movedDistance += Time.deltaTime * speed;

        // If the rings have moved a certain distance then spawn in a new ring
        if (movedDistance > 5) {
            List<GameObject> lastRing = movableRingsList[movableRingsList.Count - 1];
            float zCoord = lastRing[0].transform.position.z;

            // Create and synchronize rings
            // numOfRings is used to generate multiple rings in the scenario of a lag spike
            int numOfRings = (int) Mathf.Floor(movedDistance / positionOffset);
            // There has to be at least one ring but can be more than one at a time
            if (numOfRings == 0) {
                numOfRings = 1;
            }
            // Change the position of segments in the ring
            for (int i = 0; i < numOfRings; i++) {
                List<GameObject> ringSegments = CreateRing();

                // Spawn the ring and set it's position relative to the previous one
                foreach (GameObject segment in ringSegments) {
                    float segX = segment.transform.position.x;
                    float segY = segment.transform.position.y;

                    // Offset the ring being the last (hence why it's "synchronising" there will be no gaps)
                    segment.transform.position = new Vector3(segX, segY, zCoord + (positionOffset + i * positionOffset));
                }

                movableRingsList.Add(ringSegments);
                
                // Destory the objects that are no longer used
                int destroyIndex = movableRingsList[0].Count - 1;
                while (destroyIndex > -1) {
                    Destroy(movableRingsList[0][destroyIndex]);
                    destroyIndex -= 1;
                }

                movableRingsList.RemoveAt(0);
                movedDistance -= (float) positionOffset;
            }

            // print(GameObject.FindGameObjectsWithTag("RingSegment").Length);
        }
    }
}
