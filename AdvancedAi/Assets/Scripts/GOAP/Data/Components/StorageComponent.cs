////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: StorageComponent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This component stores all the numerous types of objects spies collect
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorageComponent : MonoBehaviour
{
    //Variables
    public int numDocuments;
    public int numIntel;
    public int numWeapons;
    public int numComponents;
    private string startingText;
    private TextMeshPro text;
    private string currentText;

    //Start function gets the text component needed
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        startingText = text.text;
    }

    //This update function updates the text for this component when needed
    private void Update()
    {
        if (text.text != (startingText + "\n" + "Amount of documents: " + "\n" + numDocuments + "\n"    + "Amount of intel: " + "\n" + numIntel + "\n" + "Amount of weapons: " + "\n" + numWeapons + "\n" + "Amount of components: " + "\n" + numComponents))
        {
            text.text = startingText + "\n" + "Amount of documents: " + "\n" + numDocuments + "\n" + "Amount of intel: " + "\n" + numIntel + "\n" + "Amount of weapons: " + "\n" + numWeapons + "\n" + "Amount of components: " + "\n" + numComponents;
        }
    }
}
