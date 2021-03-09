////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: BehaviourTree.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class is for the behaviour tree
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree 
{
    //Variable
    private Agent nodeOwnerBrain;
    
    //Set node owner brain variable to paramater value of this function
    public BehaviourTree(Agent ownerBrain)
    {
        nodeOwnerBrain = ownerBrain;
    }

    public virtual void Update() { }
    public virtual void Interupt() { }

    protected Agent GetOwner()
    {
        return nodeOwnerBrain;
    }
}
