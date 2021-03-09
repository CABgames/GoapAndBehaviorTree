////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: EspionageAgent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This child class inherits from spy and simply ensure createGoalState provides the correct goal for gatherer type
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Inherits from spy
public class EspionageAgent : Spy
{
    //Function simply overrides to give specific key value for conductedEspionage

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("conductedEspionage", true));
        return goal;
    }
}
