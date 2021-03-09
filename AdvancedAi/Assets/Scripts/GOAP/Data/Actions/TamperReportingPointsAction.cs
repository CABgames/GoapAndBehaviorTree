////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: TamperReportingPointsAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action tampers non tampered reporting points
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Inherits goap action
public class TamperReportingPointsAction : GoapAction
{
	//Variables
	private bool reachedReportingPoint = false;
	private ReportingComponent targetReportingBlock; // where we chop the firewood

	//Conditions and preconditions of this action
	public TamperReportingPointsAction()
	{
		addPrecondition("hasReportingDevice", true); // we do not need a weapon to do this
		addPrecondition("hasRadioDevice", false); // we do not need a weapon to do this
		addEffect("hasRadioDevice", true);

	}

	//Function resets the action
	public override void reset()
	{
		reachedReportingPoint = false;
		targetReportingBlock = null;
	}


	//Function returns if action is done
	public override bool isDone()
	{
		return reachedReportingPoint;
	}

	//Function returns if action is required to be in range
	public override bool requiresInRange()
	{
		return true;
	}

	//This function checks the precondition for the action, it gets the closes reporting component
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Find nearest reporting component
		ReportingComponent[] blocks = FindObjectsOfType(typeof(ReportingComponent)) as ReportingComponent[];
		ReportingComponent closest = null;
		float closestDist = 0;

		foreach (ReportingComponent block in blocks)
		{
			if (block != null && block.tamperedWith == false)
			{
				////If closest is null assign it
				if (closest == null)
				{
					closest = block;
					closestDist = (block.gameObject.transform.position - agent.transform.position).magnitude;
				}
				else
				{
					//If there one closer then assign that
					float dist = (block.gameObject.transform.position - agent.transform.position).magnitude;
					if (dist < closestDist)
					{
						closest = block;
						closestDist = dist;
					}
				}
			}
		}
		//If still null return false
		if (closest == null)
		{
			return false;
		}
		targetReportingBlock = closest;
		target = targetReportingBlock.gameObject;

		return closest != null;
	}

	//This function performs the actions task
	public override bool perform(GameObject agent)
	{

		if (targetReportingBlock != null)
        {
			targetReportingBlock.tamperedWith = true;
			reachedReportingPoint = true;
			return true;
		}
		else
		{
			return false;
		}
	}
}
