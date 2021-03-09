////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: GuardComponent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This component is contained on guards and called when a guard is killed by an assasin spy
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardComponent : MonoBehaviour
{
    //Function called when assassin spy kills a guard this function is called which increments the score and sets guard false
    public void GuardKilled()
    {
        FindObjectOfType<ScoreManager>().IncrementSpyScore();
        gameObject.SetActive(false);
    }
}
