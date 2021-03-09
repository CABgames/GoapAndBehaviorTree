////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Decorator.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class inherits from node and is for a child node
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from node
public class Decorator : Node
{
    //Variable
    Node childNode;
        
    public Decorator(Node childNode)
    {
        this.childNode = childNode;
    }

    //Function returns updates behaviour
    public override BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
    }

    //Function returns child node behaviour
    public Node GetChildBehaviour()
    {
        return childNode;
    }
}
