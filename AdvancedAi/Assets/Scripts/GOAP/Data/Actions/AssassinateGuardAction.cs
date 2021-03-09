using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class AssassinateGuardAction : GoapAction
{
	//Variables
	private bool killed = false;
	private GuardComponent targetGuard; 
	private float startTime = 0;
	public float workDuration = 2; 

	//Precondition and effect for this action
	public AssassinateGuardAction()
	{
		addPrecondition("hasWeapon", true); 
		addEffect("killGuard", true);
	}

	//Function resets this action when called
	public override void reset()
	{
		killed = false;
		targetGuard = null;
		startTime = 0;
	}

	//Function returns if action done or not
	public override bool isDone()
	{
		return killed;
	}

	//Function returns that it requires it to be in range
	public override bool requiresInRange()
	{
		return true; 
	}

	//This overriding function returns the nearest guard component
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Array of guard components filled
		GuardComponent[] blocks = FindObjectsOfType(typeof(GuardComponent)) as GuardComponent[];
		GuardComponent closest = null;
		float closestDist = 0;
		//Loops through finding nearest guard component
		foreach (GuardComponent block in blocks)
		{
			if (block != null)
            {
				//If the closest is not yet assigned then assign it
				if (closest == null)
				{
					closest = block;
					closestDist = (block.gameObject.transform.position - agent.transform.position).magnitude;
				}
				//Else find the nearest to be assigned to closest
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
		//If closest still null return false as guard component could not be found
		if (closest == null)
        {
			return false;
        }
		targetGuard = closest;
		target = targetGuard.gameObject;

		return closest != null;
	}

	//Function checks if target guard and kills that guard then returns true and completes action
	public override bool perform(GameObject agent)
	{
		if (targetGuard)
        {
			if (startTime == 0)
				startTime = Time.time;

			if (Time.time - startTime > workDuration)
			{
				killed = true;
				targetGuard.GuardKilled();
			}
				return true;
        }
		else
        {

			return false;
		}
	}
}
