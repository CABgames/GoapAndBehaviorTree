////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: RadioRturnAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action is for spies when they have completed their plan and must move to the helipad
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioReturnAction : GoapAction
{
	//Variables
	private bool reachedLandingPad = false;
	private HelipadComponent targetLandingBlock; // where we chop the firewood
	private RadioComponent[] radioBlocks;
	private ReportingComponent[] reportingBlocks;

	//Effect and precondition of this action
	public RadioReturnAction()
	{
		addPrecondition("hasRadioDevice", true); 
		addPrecondition("hasReportingDevice", true);
	}

	//Start function gets radio and reporting blocks in the scene
    private void Start()
    {
		radioBlocks = FindObjectsOfType(typeof(RadioComponent)) as RadioComponent[];
		reportingBlocks = FindObjectsOfType(typeof(ReportingComponent)) as ReportingComponent[];        
    }

	//Function resets the action booleans
    public override void reset()
	{
		reachedLandingPad = false;
		targetLandingBlock = null;
	}

	//Returns if this action if done or not
	public override bool isDone()
	{
		return reachedLandingPad;
	}

	//Function returns if this action requires it to be within range
	public override bool requiresInRange()
	{
		return true; 
	}

	//This function gets the helipad component
	public override bool checkProceduralPrecondition(GameObject agent)
	{

		HelipadComponent closest = FindObjectOfType<HelipadComponent>();
		float closestDist = (closest.gameObject.transform.position - agent.transform.position).magnitude;

		targetLandingBlock = closest;
		target = targetLandingBlock.gameObject;

		return closest != null;
	}

	//Function called when ai reached the helipad and then the spy score will be incremented
	public override bool perform(GameObject agent)
	{

		reachedLandingPad = true;
		FindObjectOfType<ScoreManager>().IncrementSpyScore();
		return true;

	}
}
