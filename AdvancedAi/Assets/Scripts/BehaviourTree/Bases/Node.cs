////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Node.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This node contains the behaviours for the behaviour tree 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all nodes
//Behaviour status enum
public enum BEHAVIOUR_STATUS
{
    SUCCESS,
    FAILURE,
    RUNNING,
    NONE
}

public class Node
{
    //Variable
    private BEHAVIOUR_STATUS behaviourStatus;
    
    //Function sets behaviour status to none
    public Node()
    {
        behaviourStatus = BEHAVIOUR_STATUS.NONE;
    }
    
    //Function returns none as behaviour status when called
    public virtual BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
    }
}
