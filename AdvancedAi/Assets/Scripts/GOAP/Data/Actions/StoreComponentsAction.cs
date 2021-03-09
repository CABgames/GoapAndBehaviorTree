////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: StoreComponentAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action stores components in a supply pile
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from goap action
public class StoreComponentsAction : GoapAction
{
	//Variables
	private bool droppedOffComponents = false;
	private StorageComponent targetSupplyPile; // where we drop off the firewood

	//Conditions and preconditions of this action
	public StoreComponentsAction()
	{
		addPrecondition("hasComponents", true); 
		addPrecondition("hasReportingDevice", false);
		addPrecondition("hasRadioDevice", false);  
		addEffect("hasComponents", false); 
		addEffect("hasRadioDevice", true); 
		addEffect("hasReportingDevice", false); 
	}

	//Reset function for resetting the action
	public override void reset()
	{
		droppedOffComponents = false;
		targetSupplyPile = null;
	}

	//Function to return if action is done
	public override bool isDone()
	{
		return droppedOffComponents;
	}

	//Function to return if it requires to be in range for this task
	public override bool requiresInRange()
	{
		return true; 
	}

	//Function to return the nearest storage component
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Fill array with storage components
		StorageComponent[] supplyPiles = (StorageComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(StorageComponent));
		StorageComponent closest = null;
		float closestDist = 0;
		//Loop through storage component to find closest one
		foreach (StorageComponent supply in supplyPiles)
		{
			if (supply != null)
            {
				//If null assign this
				if (closest == null)
				{
					closest = supply;
					closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
				}
				//Else check if closer
				else
				{
					float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
					if (dist < closestDist)
					{
						closest = supply;
						closestDist = dist;
					}
				}
            }
		}
		//If null then no storage components found
		if (closest == null)
			return false;
		
		targetSupplyPile = closest;
		target = targetSupplyPile.gameObject;
		return closest != null;
	}

	//Function checks if at target supply pule and then takes components from backpack and puts them in supply pile
	public override bool perform(GameObject agent)
	{
		//If not null then put the components 
		if (targetSupplyPile != null)
		{ 
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			targetSupplyPile.numComponents += backpack.numComponents;
			droppedOffComponents = true;
			backpack.numComponents = 0;
			return true;
		}
		else 
		{

			return false;
		}
		
	}
}
