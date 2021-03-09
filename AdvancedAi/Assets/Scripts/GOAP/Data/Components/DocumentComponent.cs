using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DocumentComponent : MonoBehaviour
{
    //Variables
    public int numDocuments = 5;
    private string startingText;
    private TextMeshPro text;
    private string currentText;

    //Start function gets text component needed
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        startingText = text.text;
    }

    //Update function changes text when it needed to be updated
    private void Update()
    {
        if (text.text != (startingText + "\n" + "Amount of documents: " + "\n" + numDocuments))
        {
            text.text = startingText + "\n" + "Amount of documents: " + "\n" + numDocuments;
        }
    }

    //When a guard enters this gameobjects trigger and finds it altered a investigation and replenishment will take place
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Guard" && numDocuments < 5)
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
            numDocuments = 5;
            text.text = startingText + "\n" + "Amount of documents: " + "\n" + numDocuments;
        }
    }
}
