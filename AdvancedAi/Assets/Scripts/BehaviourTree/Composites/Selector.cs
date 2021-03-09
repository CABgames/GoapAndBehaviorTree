////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Selector.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class to go through child behaviours returning their status
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

//Inherits from composite
public class Selector : Composite
{
    //Function call init
    public Selector()
    {
        Init();
    }


    public override BEHAVIOUR_STATUS Update()
    {
        BEHAVIOUR_STATUS returnStatus = BEHAVIOUR_STATUS.FAILURE;
        Node currentNode = GetChildBehaviours()[currentIndex];

        if (currentNode != null)
        {
            BEHAVIOUR_STATUS behaviourStatus = currentNode.Update();
            if (behaviourStatus == BEHAVIOUR_STATUS.FAILURE)
            {
                if (currentIndex == GetChildBehaviours().Count - 1)
                {
                    returnStatus = BEHAVIOUR_STATUS.FAILURE;
                }
                else
                {
                    ++currentIndex;
                    returnStatus = BEHAVIOUR_STATUS.RUNNING;
                }
            }
            else
            {
                returnStatus = behaviourStatus;
            }
        }

        //If suceeded or failed call the reset function
        if (returnStatus == BEHAVIOUR_STATUS.SUCCESS || returnStatus == BEHAVIOUR_STATUS.FAILURE)
        {
            Reset();
        }
        //Return that status
        return returnStatus;
    }
}
