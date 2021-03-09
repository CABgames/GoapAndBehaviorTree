////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: DepositIntelAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action deposits intel from the backpack of the spy into a supply pile
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class DepositIntelAction : GoapAction
{
	//Variables
	private bool droppedOffIntel = false;
	private StorageComponent targetSupplyPile; 

	//Preconditions and effects
	public DepositIntelAction()
	{
		addPrecondition("hasDocuments", true);
		addPrecondition("hasIntel", true);
		addEffect("hasDocuments", false);
		addEffect("hasComponents", true);
		addEffect("hasWeapon", true);
		addEffect("hasRadioDevice", true);
	}

	//This function resets this action when called
	public override void reset()
	{
		droppedOffIntel = false;
		targetSupplyPile = null;
	}

	//Function returns if the action is done
	public override bool isDone()
	{
		return droppedOffIntel;
	}

	//Function returns true because this action requires the ai to be in range
	public override bool requiresInRange()
	{
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent)
	{
		StorageComponent[] supplyPiles = (StorageComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(StorageComponent));
		StorageComponent closest = null;
		float closestDist = 0;
		//Loop through looking for the nearest storage component
		foreach (StorageComponent supply in supplyPiles)
		{
			if (closest == null)
			{
				closest = supply;
				closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
			}
			//Find which supply pile is the closest
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
		//If no storage component can be found then return false
		if (closest == null)
			return false;

		targetSupplyPile = closest;
		target = targetSupplyPile.gameObject;
		return closest != null;
	}

	//Function returns true after adding the intel from the backpack component to the supply storage
	public override bool perform(GameObject agent)
	{
		BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
		targetSupplyPile.numDocuments += backpack.numDocuments;
		droppedOffIntel = true;
		backpack.numDocuments = 0;
		targetSupplyPile.numIntel += backpack.numIntel;
		backpack.numIntel = 0;
		return true;
	}
}
