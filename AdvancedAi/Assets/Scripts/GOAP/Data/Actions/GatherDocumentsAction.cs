////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: GatherDocumentsAction.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This action gathers documents from document components
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inheriting goap action
public class GatherDocumentsAction : GoapAction
{
	//Variables
	private bool document = false;
	private DocumentComponent targetDocumentBlock; 

	//Preconditions and effect
	public GatherDocumentsAction()
	{
		addPrecondition("hasWeapon", false);
		addPrecondition("hasDocuments", false); 
		addPrecondition("hasIntel", true);
		addEffect("hasDocuments", true);
	}

	//This function resets this action when called
	public override void reset()
	{
		document = false;
		targetDocumentBlock = null;
	}

	//This function returns if the action is done or not
	public override bool isDone()
	{
		return document;
	}

	//A function which returns that the action requires the ai to be in range
	public override bool requiresInRange()
	{
		return true; 
	}

	//In this function the nearest document component to the spy is returned
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//Fill the array with document components in the scene
		DocumentComponent[] blocks = FindObjectsOfType(typeof(DocumentComponent)) as DocumentComponent[];
		DocumentComponent closest = null;
		float closestDist = 0;

		//Loop through seeking the closest document component
		foreach (DocumentComponent block in blocks)
		{
			if (block != null && block.numDocuments > 0)
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
		//If no document component can be found return false
		if (closest == null)
        {
			return false;
        }

		targetDocumentBlock = closest;
		target = targetDocumentBlock.gameObject;

		return closest != null;
	}

	//This function returns true when the action is complete and ads a document into a spies backpack whilst removing it from a document component
	public override bool perform(GameObject agent)
	{

		if (targetDocumentBlock != null)
		{
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			targetDocumentBlock.numDocuments--;
			backpack.numDocuments++;
			document = true;
			return true;
		}
		else 
		{
			return false;
		}
	}
}
