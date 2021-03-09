////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: TamperRadioAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action goes through tampering non tampered radios
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Inherits from goap action
public class TamperRadiosAction : GoapAction
{
	//Variables
	private bool reachedRadioPoint = false;
	private RadioComponent targetRadioBlock; // where we chop the firewood

	//Conditions and preconditions of this action
	public TamperRadiosAction()
	{
		addPrecondition("hasReportingDevice", false); // not conducted espionage 
		addPrecondition("hasRadioDevice", true); // we do not need a weapon to do this
		addEffect("hasRadioDevice", false);
		addEffect("hasReportingDevice", true);
	}

	//Function resets this actions bools when called
	public override void reset()
	{
		reachedRadioPoint = false;
		targetRadioBlock = null;
	}

	//Function returns if action is done yet
	public override bool isDone()
	{
		return reachedRadioPoint;
	}

	//Function returns if requires this action to be in range to be completed (it ddoes)
	public override bool requiresInRange()
	{
		return true; 
	}

	//Function checks if radio components are there and then finds nearest
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Find the nearest radiop components
		RadioComponent[] blocks = FindObjectsOfType(typeof(RadioComponent)) as RadioComponent[];
		RadioComponent closest = null;
		float closestDist = 0;
		//Loop through radio components checking for the nearest to the ai
		foreach (RadioComponent block in blocks)
		{
			if (block != null && block.tamperedWith == false)
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
		//If closest is null and therefore no radio component return null
		if (closest == null)
		{
			return false;
		}
		targetRadioBlock = closest;
		target = targetRadioBlock.gameObject;

		return closest != null;
	}

	//Function which returns when the action has been performed
	public override bool perform(GameObject agent)
	{

		if (targetRadioBlock != null)
		{
			targetRadioBlock.tamperedWith = true;
			reachedRadioPoint = true;
			return true;
		}
		else
		{
			return false;
		}
	}
}
