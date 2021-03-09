////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: MoveToTarget.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class moves the ai to it's target
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.AI;

//Inherits from Action
public class MoveToTarget : Action
{
    public MoveToTarget(Agent owner) : base(owner)
    {
        
    }
    //Function to update moving to target
    public override BEHAVIOUR_STATUS Update()
    {
        if (GetOwner().GetTargetPosition() != Vector3.zero)
        {
            GetOwner().GetNavMeshAgent().isStopped = false;
            GetOwner().GetNavMeshAgent().SetDestination(GetOwner().GetTargetPosition());
        }
        return BEHAVIOUR_STATUS.FAILURE;
    }
}
