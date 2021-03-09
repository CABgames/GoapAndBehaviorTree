////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: SpyDetected.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class returns is the spy was detected or not
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from condition
public class SpyDetected : Condition
{
    public SpyDetected(Agent ownerBrain) : base(ownerBrain)
    {
    }

    //Function returns if spy detected or not
    public override BEHAVIOUR_STATUS Update()
    {

        if (GetOwner().spyDetected)
        {
            return BEHAVIOUR_STATUS.SUCCESS;
        }
        else
        {
            return BEHAVIOUR_STATUS.FAILURE;
        }
    }
}
