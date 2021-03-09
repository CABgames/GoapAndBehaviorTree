////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: HideWeaponAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action is for the assasin spy to hide the weapon obtained prior to killing a guard in a supply pile
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class HideWeaponAction : GoapAction
{
	//Variables
	private bool droppedOffWeapon = false;
	private StorageComponent targetSupplyPile; 

	//Precondition and effects
	public HideWeaponAction()
	{
		addPrecondition("hasWeapon", true); 
		addEffect("hasWeapon", false);  
		addEffect("hasComponents", true); 
		addEffect("hasRadioDevice", true);
	}

	//This function resets this action when called
	public override void reset()
	{
		droppedOffWeapon = false;
		targetSupplyPile = null;
	}

	//This function returns true when the action is done 
	public override bool isDone()
	{
		return droppedOffWeapon;
	}

	//This function returns true as the action requires ai to be in range
	public override bool requiresInRange()
	{
		return true; 
	}


	//Function to check through storage components to find the nearest, otherwise returning a false if not
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Fill the storage component aray with the storage components in the scene
		StorageComponent[] supplyPiles = (StorageComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(StorageComponent));
		StorageComponent closest = null;
		float closestDist = 0;
		//Loop through finding the nearest storage component
		foreach (StorageComponent supply in supplyPiles)
		{
			if (closest == null)
			{
				closest = supply;
				closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
			}
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
		//If no storage component can be found return false
		if (closest == null)
			return false;

		targetSupplyPile = closest;
		target = targetSupplyPile.gameObject;
		return closest != null;
	}

	//Function returns true and takes a weapon from the backpack and adds one to the supply pile
	public override bool perform(GameObject agent)
	{
		BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
		targetSupplyPile.numWeapons += backpack.numWeapons;
		droppedOffWeapon = true;
		backpack.numWeapons = 0;

		return true;
	}
}
