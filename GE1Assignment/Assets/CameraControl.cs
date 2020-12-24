﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    
    public GameObject mainCamera;
    public float speed = 50.0f;
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main.gameObject;
        }
    }

    void Walk(float units)
    {
        Vector3 forward = mainCamera.transform.forward;
        //forward.y = 0;
        forward.Normalize();
        transform.position += forward * units;
    }

    void Fly(float units)
    {
        transform.position += Vector3.up * units;
    }

    void Strafe(float units)
    {
        transform.position += mainCamera.transform.right * units;
            
    }

    // Update is called once per frame
    void Update()
    {
        float speed = this.speed;

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.W))
        {
            Walk(Time.deltaTime * speed);
        }
            

        float contWalk = Input.GetAxis("Vertical");
        Walk(contWalk * speed * Time.deltaTime);
    }
}
