////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: HidingComponent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This component is used to hide spies being pursued and displays when being hidden inside
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HidingComponent : MonoBehaviour
{

    //Variables
    public bool spyHiding = false;
    private string startingText;
    private TextMeshPro text;
    private string currentText;

    //Start function gets text component
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        startingText = text.text;
    }

    //Update function text changes when variable changes
    private void Update()
    {
        if (text.text != (startingText + "\n" + "Spy hiding: " + "\n" + spyHiding))
        {
            text.text = startingText + "\n" + "Spy hiding: " + "\n" + spyHiding;
        }
    }

    //When a guard enters this gameobjects trigger and finds it altered a investigation and replenishment will take place
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Guard" && spyHiding == true)
        {
            spyHiding = false;
            text.text = startingText + "\n" + "Spy hiding: " + "\n" + spyHiding;

        }
    }
}
