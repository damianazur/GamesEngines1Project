using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpawner : MonoBehaviour
{
    public int segments = 10;

    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 point = new Vector3(0, 0, 0);
        float thetaInc = (Mathf.PI * 2.0f) / (float)segments;
        float radius = 10.0f;

        float theta = Mathf.PI * 2.0f / ((float) segments);
        float z = 0.0f;
        for (int j = 0 ; j < segments ; j ++)
        {
            float angle  = (j * theta);

            float x = (Mathf.Sin(angle) * radius) + point.x;
            float y = (Mathf.Cos(angle) * radius) + point.y;
            GameObject brick = CreateBrick(x, y, z, 1, 1, 1);

            Vector3 target = point;
            Vector3 relativePos = target - brick.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.right);
            brick.transform.rotation = rotation;

            brick.GetComponent<Renderer>().material.color =
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
            brick.transform.parent = this.transform;
        }
    }

    GameObject CreateBrick(float x, float y, float z, float xScale = 1.0f, float yScale = 1.0f, float zScale = 1.0f)
    {
        GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
        brick.transform.localScale = new Vector3(xScale, yScale, zScale);
        brick.transform.position = new Vector3(x, y, z);
        //brick.GetComponent<Renderer>().material.color = Utilities.RandomColor();
        //Rigidbody rigidBody = brick.AddComponent<Rigidbody>();
        //rigidBody.mass = 1.0f;
        return brick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
