////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: RadioReturnGatheringAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action is done when an espionage spy has conducted espionage
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioReturnGatheringAction : RadioReturnAction
{
    //Effects and preconditions of this action
    public RadioReturnGatheringAction()
    {
        addPrecondition("hasWeapon", true); // we do not need a weapon to do this
        addEffect("collectDocuments", true); // we conducted the espionage	
    }
}
