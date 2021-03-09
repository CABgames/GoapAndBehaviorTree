////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Sequence.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class returns the status of behaviours
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

//Inherits from composite
public class Sequence : Composite
{
    //Function calls initialise when called
    public Sequence()
    {
        Init();
    }

    //Function for updating the behaviour status
    public override BEHAVIOUR_STATUS Update()
    {
        BEHAVIOUR_STATUS returnStatus = BEHAVIOUR_STATUS.FAILURE;
        Node currentBehaviour = GetChildBehaviours()[currentIndex];
        //If current index is not null
        if (currentIndex != null)

        {
            BEHAVIOUR_STATUS behaviourStatus = currentBehaviour.Update();

            if (behaviourStatus == BEHAVIOUR_STATUS.SUCCESS)
            {
                //Return success
                if (currentIndex == GetChildBehaviours().Count - 1)
                {
                    returnStatus = BEHAVIOUR_STATUS.SUCCESS;                
                }
                //Pre increment and then return keep running
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
        //If success or failure then reset
        if (returnStatus == BEHAVIOUR_STATUS.SUCCESS || returnStatus == BEHAVIOUR_STATUS.FAILURE)
        {
            Reset();
        }
        //Return the status
        return returnStatus;
    }
}
