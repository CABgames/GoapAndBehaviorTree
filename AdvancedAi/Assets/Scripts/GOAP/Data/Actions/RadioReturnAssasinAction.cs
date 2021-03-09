////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: RadioReturnAssasinAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description:  Action for when guards have been killed
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Inherits from radio return action
public class RadioReturnAssasinAction : RadioReturnAction
{
    //Effects and preconditions of this action
    public RadioReturnAssasinAction ()
    {
        addPrecondition("hasComponents", true); // can't drop off weapons if we don't already have some
        addEffect("killGuard", true); // we conducted the espionage	
    }
}
