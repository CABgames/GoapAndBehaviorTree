////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Action.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This action class inherits from the Node class 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from node
public class Action : Node
{
    //Variable
    private Agent nodeOwnerBrain;

    //Function asigns parameter to node owner brain
    public Action(Agent ownerBrain)
    {
        nodeOwnerBrain = ownerBrain;
    }

    //Function returns none status when called
    public override BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
    }

    //Function returns node ownder brain when called
    public Agent GetOwner()
    {
        return nodeOwnerBrain;
    }
}
