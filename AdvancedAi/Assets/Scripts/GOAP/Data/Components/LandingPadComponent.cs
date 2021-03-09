////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: LandingPadComponent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This component keeps track of the amount of ai in the landing/helipad area
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LandingPadComponent : MonoBehaviour
{
    //Variables
    private int numberOfPlayers = 0;
    private string startingText;
    private TextMeshPro text;
    private string currentText;

    //Start function assigns what is needed for text mesh pro
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        startingText = text.text;
    }

    //Update function checks if the text is not currently correct and therefore needs updating
    private void Update()
    {
        if (text.text != (startingText + "\n" + "Amount of Ai in area: " + "\n" + numberOfPlayers))
        {
            text.text = startingText + "\n" + "Amount of Ai in area: " + "\n" + numberOfPlayers;
        }

    }

    //When a spy or guard enters increment integer by one
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spy" || other.gameObject.tag == "Guard")
        {
            numberOfPlayers++;
        }
    }

    //When a spy or guard exits lower integer by one
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Spy" || other.gameObject.tag == "Guard")
        {
            if (numberOfPlayers != 0)
            {
                numberOfPlayers--;
            }
        }
    }
}
