/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: LockRotation.cs 
///Created by: Charlie Bullock
///Description: This class is used to lock rotation of text elements either contantly or once
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField]
    private bool keepAdjusting;

    private void Start()
    {
        if (keepAdjusting)
        {
            transform.eulerAngles = new Vector3(90, 0, 0);
            Destroy(GetComponent<LockRotation>());
        }

    }
    //Constantly
    void Update()
    {
        transform.eulerAngles = new Vector3(90, 0, -transform.parent.rotation.eulerAngles.z);
    }
}
