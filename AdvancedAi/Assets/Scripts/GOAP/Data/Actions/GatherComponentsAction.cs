////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: GatherComponentsAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action gathers components from workshop components, these components are stored in the backpack of the spy
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class GatherComponentsAction : GoapAction
{
	//Variables
	private bool workshop = false;
	private WorkshopComponent targetWorkshopBlock; 

	//Preconditions and effects
	public GatherComponentsAction()
	{
		addPrecondition("hasWeapon", false);
		addPrecondition("hasReportingDevice", false); 
		addPrecondition("hasRadioDevice", false); 
		addPrecondition("hasComponents", false); 
		addEffect("hasComponents", true);
	}

	//This function resets this action when called
	public override void reset()
	{
		workshop = false;
		targetWorkshopBlock = null;
	}

	//This function returns true when this action is complete
	public override bool isDone()
	{
		return workshop;
	}

	//This function returns true because the action requires the ai to be in range
	public override bool requiresInRange()
	{
		return true; 
	}

	//This function returns true and the closest workshop componenet
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Get the workshop componenets in the scene and add them to this array
		WorkshopComponent[] blocks = FindObjectsOfType(typeof(WorkshopComponent)) as WorkshopComponent[];
		WorkshopComponent closest = null;
		float closestDist = 0;

		foreach (WorkshopComponent block in blocks)
		{
			if (block != null)
            {
				if (block.numComponents > 0)
				{
					if (closest == null)
					{
						closest = block;
						closestDist = (block.gameObject.transform.position - agent.transform.position).magnitude;
					}
					else
					{
						float dist = (block.gameObject.transform.position - agent.transform.position).magnitude;
						if (dist < closestDist)
						{
							closest = block;
							closestDist = dist;
						}
					}
				}
            }
		}
		if (closest == null)
        {
			return false;
        }
		targetWorkshopBlock = closest;
		target = targetWorkshopBlock.gameObject;

		return closest != null;
	}

	//In this function if the target workshop block is not null a component will be removed from it and one will be added to the backpack along with true being returned
	public override bool perform(GameObject agent)
	{

		if (targetWorkshopBlock != null)
		{
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			targetWorkshopBlock.numComponents--;
			backpack.numComponents++;
			workshop = true;

			return true;
		}
		else
		{
			return false;
		}
	}

}

