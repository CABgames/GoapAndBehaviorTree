////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: RadioReturnEspionageAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: Action returns true for espionage being conducted
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Inherits from radio return action
public class RadioReturnEspionageAction : RadioReturnAction
{
    //Effects and preconditions of this action
    public RadioReturnEspionageAction()
    {
        addEffect("conductedEspionage", true); // we conducted the espionage	

    }
}
