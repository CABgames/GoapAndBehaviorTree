////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: RadioComponent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This component can be tampered with or detected when tampered with
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RadioComponent : MonoBehaviour
{
    //Variables
    public bool tamperedWith = false;
    private string startingText;
    private TextMeshPro text;
    private string currentText;

    //Start function gets component for text mesh pro
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        startingText = text.text;
    }

    //Update function will check if text needs updating
    private void Update()
    {
        if (text.text != (startingText + "\n" + "Tampered with: " + "\n" + tamperedWith))
        {
            text.text = startingText + "\n" + "Tampered with: " + "\n" + tamperedWith;
        }
    }

    //When a guard enters this gameobjects trigger and finds it altered a investigation and replenishment will take place
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Guard" && tamperedWith == false)
        {
            //Guard should seak closest spy to investigate and set this component back to normal
            Agent agent = other.GetComponent<Agent>();
            agent.spyDetected = true;
            Spy[] spies = (Spy[])UnityEngine.GameObject.FindObjectsOfType(typeof(Spy));
            Spy closest = null;
            float closestDistance = 0;
            //Find the closest spy
            foreach (Spy spy in spies)
            {
                //If closest is empty this will be assigned
                if (closest == null)
                {
                    // first one, so choose it for now
                    closest = spy;
                    closestDistance = (spy.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    //Get current distance
                    float distance = (spy.gameObject.transform.position - agent.transform.position).magnitude;
                    //If one closer then set that to closest
                    if (distance < closestDistance)
                    {
                        closest = spy;
                        closestDistance = distance;
                    }
                }
            }
            //If no spies left increment guard score (resets)
            if (closest == null)
            {
                FindObjectOfType<ScoreManager>().IncrementGuardScore();
            }
            //Else go to nearest spy position
            else
            {
                agent.SetTargetPosition(closest.transform.position);
            }
            tamperedWith = false;
            text.text = startingText + "\n" + "Tampered with: " + "\n" + tamperedWith;

        }
    }
}
