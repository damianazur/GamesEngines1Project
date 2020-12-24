using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RingSpawner : MonoBehaviour
{
    public float radius = 11;
    public int noOfSegments = 20;
    public int startingRings = 100;
    public int ringSegmentGapMax = 8;
    public int finalRingCount;
    public List<List<GameObject>> movableRingsList = new List<List<GameObject>>();
    public List<List<GameObject>> endRings = new List<List<GameObject>>();

    private Vector3 ringGeneratePoint;

    private float movedDistance = 0;
    private int positionOffset;
    public GameObject prefab;

    void Awake() {
        // Distance between each ring is the width of the ring * 2 + 1 (gap)
        positionOffset = (int) prefab.transform.localScale.z * 2 + 1;
        print("POS OFFSET: " + positionOffset);
        generateInitalRings();
    }

    // Start is called before the first frame update
    void Start()
    {   
        
    }

    void generateInitalRings() {
        // Last x many rings will have their radius gradually reduced
        // This value needs to be restored
        float savedRadius = radius;

        // Create the initial rings
        for (int i = 0; i < startingRings; i++) {
            List<GameObject> ringSegments = CreateRing();

            // Move the spawner forward
            transform.position += new Vector3(0, 0, positionOffset);
            movableRingsList.Add(ringSegments);
        }

        // Create the end rings
        for (int i = 0; i < finalRingCount; i++) {
            List<GameObject> ringSegments = CreateRing();

            if (radius > 5) {
                radius = radius - (float) 0.1f;
            }

            // Move the spawner forward
            transform.position += new Vector3(0, 0, positionOffset);
            endRings.Add(ringSegments);
        }

        int endPieceCount = endRings.Count;
        transform.position -= new Vector3(0, 0, (endPieceCount + 1) * positionOffset);
        // Radius is reset 
        radius = savedRadius;

        // Add final ring so there is no gap
        CreateRing();
    }

    // Creates a ring in the position of the spawner
    List<GameObject> CreateRing() {
        GameObject ringHolder = new GameObject();
        ringHolder.name = "RingHolder";
        ringHolder.tag = "RingHolder";

        GameObject tunnelHolder = GameObject.FindWithTag("TunnelHolder");
        float tunnelRotZDeg = tunnelHolder.transform.rotation.eulerAngles.z;
        float tunnelRotZRad = tunnelRotZDeg * Mathf.Deg2Rad;

        // The ring is made up of rectangles (segments)
        List<GameObject> ringSegments = new List<GameObject>();

        // Center point of the circle (segments will rotate towards it)
        Vector3 centerPoint = this.transform.position;
        ringHolder.transform.position = centerPoint;

        // Calculate the number of segments needed to fill the radius of the circle
        float segmentGap;
        if (endRings.Count > 0 && endRings.Count < finalRingCount) {
            segmentGap = 1;
        } else {
            segmentGap = Random.Range(0, ringSegmentGapMax);
        }

        float cubeY = prefab.transform.localScale.y;
        float circumfrence = (2.0f * Mathf.PI * (radius - segmentGap));
        float yScale = (float) (circumfrence / noOfSegments);
        // Size scale used for resizing segments to fit the radius
        float sizeScale = yScale / cubeY;

        // Get the angle of the segment on the circle (2 pi = 360 degress => 360 / noOfSegments = angle for of each segment)
        float theta = Mathf.PI * 2.0f / ((float) noOfSegments);
        // The rings move along the z-axis
        float z = centerPoint.z;
        // print("Segments: " + segments);
        for (int j = 0 ; j < noOfSegments; j ++)
        {
            float angle  = (j * theta) - tunnelRotZRad;
            float x = (Mathf.Sin(angle) * radius) - centerPoint.x;
            float y = (Mathf.Cos(angle) * radius) - centerPoint.y;

            GameObject cube = GameObject.Instantiate<GameObject>(prefab);
                cube.transform.position = new Vector3(x, y, z);
            
            cube.transform.parent = ringHolder.transform;
            ringHolder.transform.parent = tunnelHolder.transform;

            // Resize the cube
            float xScale = prefab.transform.localScale.x;
            if (endRings.Count > 0 && endRings.Count < finalRingCount) {
                xScale = 1 + (endRings.Count * 4);
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

    void generateAndSyncNewRing() {
        // Create and synchronize rings
        // numOfRings is used to generate multiple rings in the scenario of a lag spike
        int numOfRings = (int) Mathf.Floor(movedDistance / positionOffset);

        List<GameObject> lastRing = movableRingsList[movableRingsList.Count - 1];
        float zCoord = lastRing[0].transform.position.z;

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
            Destroy(movableRingsList[0][destroyIndex].transform.parent.gameObject);
            // while (destroyIndex > -1) {
            //     Destroy(movableRingsList[0][destroyIndex]);
            //     destroyIndex -= 1;
            // }

            movableRingsList.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 latestRingCenter = new Vector3(transform.position.x, transform.position.y, movableRingsList[movableRingsList.Count-1][0].transform.position.z);
        movedDistance = Vector3.Distance (latestRingCenter, transform.position);

        // If the rings have moved a certain distance then spawn in a new ring
        if (movedDistance > positionOffset) {
            generateAndSyncNewRing();
        }
    }
}
