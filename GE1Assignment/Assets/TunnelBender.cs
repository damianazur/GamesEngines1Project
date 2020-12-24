using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBender : MonoBehaviour
{
    List<List<GameObject>> movableRingsList;

    public GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ringSpawner = GameObject.FindWithTag("RingSpawner");
        movableRingsList = ringSpawner.GetComponent<RingSpawner>().movableRingsList;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
