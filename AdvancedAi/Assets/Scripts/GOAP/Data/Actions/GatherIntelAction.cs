////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: GatherIntelAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action gathers intel from any intel components containing intel
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class GatherIntelAction : GoapAction
{
	//Variables
	private bool document = false;
	private IntelComponent targetIntelBlock;
	public float workDuration = 2; 

	//Preconditions and effect
	public GatherIntelAction()
	{
		addPrecondition("hasWeapon", false); 
		addPrecondition("hasDocuments", false);
		addPrecondition("hasIntel", false); 
		addEffect("hasIntel", true);
	}

	//This function resets this action when called
	public override void reset()
	{
		document = false;
		targetIntelBlock = null;
	}

	//Function returns if the action is done or not
	public override bool isDone()
	{
		return document;
	}

	//Function returns true because the action requires it to be in range
	public override bool requiresInRange()
	{
		return true; 
	}

	//Function finds the closest intel component to the spy ai
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Intel component array to be filled with intel components of the scene
		IntelComponent[] blocks = FindObjectsOfType(typeof(IntelComponent)) as IntelComponent[];
		IntelComponent closest = null;
		float closestDist = 0;
		//Look through intel components finding the nearest 
		foreach (IntelComponent block in blocks)
		{
			if (block != null && block.numIntel > 0)
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
		if (closest == null)
        {
			return false;
        }
		targetIntelBlock = closest;
		target = targetIntelBlock.gameObject;

		return closest != null;
	}

	//Function that if intel block is not null add intel from the component into the spies backpack and then return true or else return false
	public override bool perform(GameObject agent)
	{
		if (targetIntelBlock != null)
		{
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			targetIntelBlock.numIntel--;
			backpack.numIntel++;
			document = true;

			return true;
		}
		else 
		{
			return false;
		}
	}

}
