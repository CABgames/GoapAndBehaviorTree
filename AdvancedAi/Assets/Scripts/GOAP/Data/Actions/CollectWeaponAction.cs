////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: CollectWeaponAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action collects a weapon from one of the weapon components 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class CollectWeaponAction : GoapAction
{
	//Variables
	private bool hasWeapon = false;
	private WeaponComponent targetWeaponPile; 

	//Precondition and effect for this action
	public CollectWeaponAction()
	{
		addPrecondition("hasWeapon", false); 
		addEffect("hasWeapon", true);
	}

	//This function resets this action when called
	public override void reset()
	{
		hasWeapon = false;
		targetWeaponPile = null;
	}

	//Function returns true when action done
	public override bool isDone()
	{
		return hasWeapon;
	}

	//Function returns true that the action is required to be in range
	public override bool requiresInRange()
	{
		return true; 
	}

	//Function checks the precondition by getting the nearest weapon componenet
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Fill the weapon component array with weapon components in the scene
		WeaponComponent[] weaponPiles = (WeaponComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(WeaponComponent));
		WeaponComponent closest = null;
		float closestDist = 0;
		//Loop through finding closoest weapon component
		foreach (WeaponComponent weapon in weaponPiles)
		{
			if (weapon.numWeapons > 0)
			{
				if (closest == null)
				{
					closest = weapon;
					closestDist = (weapon.gameObject.transform.position - agent.transform.position).magnitude;
				}
				else
				{
					float dist = (weapon.gameObject.transform.position - agent.transform.position).magnitude;
					if (dist < closestDist)
					{
						closest = weapon;
						closestDist = dist;
					}
				}
			}
		}
		//If no weapon component found return false
		if (closest == null)
			return false;

		targetWeaponPile = closest;
		target = targetWeaponPile.gameObject;

		return closest != null;
	}
	//This function completes the action and takes a weapon from the selected component and places it in the ai backpack
	public override bool perform(GameObject agent)
	{
		if (targetWeaponPile.numWeapons > 0)
		{
			hasWeapon = true;
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			targetWeaponPile.numWeapons--;
			backpack.numWeapons++;

			return true;
		}
		else
		{
			return false;
		}
	}
}
