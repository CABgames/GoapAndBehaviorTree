////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Condition.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class inherits from Node and contains an agent variable for node owner 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from node
public class Condition : Node
{
    //Variable
    private Agent nodeOwner;

    public Condition(Agent ownerBrain)
    {
        nodeOwner = ownerBrain;
    }

    //Function returns updates behaviour
    public override BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
    }

    //Function returns node agent
    public Agent GetOwner()
    {
        return nodeOwner;
    }
}
