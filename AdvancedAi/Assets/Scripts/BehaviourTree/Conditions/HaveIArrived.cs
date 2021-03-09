////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: HaveIArrived.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class returns if the ai have arrived at the intended point (within range) or not
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.AI;

//Inherits from 
public class HaveIArrived : Condition
{
    //Variable
    private const float arrivalRange = 1;
    
    public HaveIArrived(Agent ownerBrain) : base(ownerBrain)
    {

    }

    //Returns true if within the arrival range distance otherwise returning false
    public override BEHAVIOUR_STATUS Update()
    {
        if (GetOwner().GetTargetPosition() == Vector3.zero)
        {
            return BEHAVIOUR_STATUS.SUCCESS;
        }
        float dist = GetOwner().GetNavMeshAgent().remainingDistance;
        //If distance less than the arrival range then return success
        if (dist < arrivalRange)
        {

            return BEHAVIOUR_STATUS.SUCCESS;
        }
        else
        {
            return BEHAVIOUR_STATUS.FAILURE;
        }
    }
}
