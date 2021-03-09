/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: MoveCamera.cs 
///Created by: Charlie Bullock
///Description: This class moves the player camera within given clamp restraints, players move with keys and zoom with mouse
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //Variable
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        //Move the camera around based on horizontal or vertical values that are being got and also zooming in and out the camera based on mouse scrolling
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * 0.25f, Input.GetAxis("Vertical") * 0.25f, Input.GetAxis("Mouse ScrollWheel") * 4);
        targetVelocity = transform.TransformDirection(targetVelocity);

        transform.position += targetVelocity;
        Vector3 clampedPosition = transform.position;
        //Clamp camera position to the correct constraints to stop leaving area
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -60, 60);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 5, 50);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -60, 60);
        transform.position = clampedPosition;
    }
}
