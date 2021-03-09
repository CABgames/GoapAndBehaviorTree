////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: GoToObject.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class goes to the target objects position
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Function inherits from Action
public class GoToObjects : Action
{
    public GoToObjects(Agent owner) : base(owner)
    {

    }

    public override BEHAVIOUR_STATUS Update()
    {
        Vector3 newTarget = GetOwner().GetTargetPosition();
        
        if (newTarget != null)
        {
            //We have a target so the behaviour was successful
            GetOwner().SetTargetPosition(newTarget);
            return BEHAVIOUR_STATUS.SUCCESS;
        }
        else
        {
            //No target was found so node must keep running
            GetOwner().GetNavMeshAgent().isStopped = true;
            return BEHAVIOUR_STATUS.RUNNING;
        }
    }
}
