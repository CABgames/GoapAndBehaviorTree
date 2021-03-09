////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Spy.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This parent class contains the broad functionality utilised for the three spy types and their actions 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/**
 * A general spy class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 */
public abstract class Spy : MonoBehaviour, IGoap
{
    //Variables
    #region Variables
    public BackpackComponent backpack;
	public float moveSpeed = 1;
	private FieldOfView fov;
	private NavMeshAgent navAgent;
	private enum detectionStates {Avoid,Hide,Sneak, Normal}
	private detectionStates spyDetectionState;
	private bool enumLock = false;
	private HidingComponent closest = null;
	public Transform enemyPosition;
	#endregion Variables
	//Start function assigns components to respective variables 
	void Start()
	{
		fov = GetComponent<FieldOfView>();
		navAgent = GetComponent<NavMeshAgent>();
		spyDetectionState = detectionStates.Normal;
		//If navmesh is not null set it's speed
		if (GetComponent<NavMeshAgent>() != null)
		{
			gameObject.GetComponent<NavMeshAgent>().speed = moveSpeed;
		}
		else
		{
			Debug.Log("NavmeshAgent missing!");
		}
		//If backpack null assign the one equipped to ai
		if (backpack == null)
			backpack = gameObject.AddComponent<BackpackComponent>() as BackpackComponent;

	}

	//Method will feed data into the goap action, each keypair has equivalent in backpack
	public HashSet<KeyValuePair<string, object>> getWorldState()
	{
		HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

		worldData.Add(new KeyValuePair<string, object>("hasDocuments", (backpack.numDocuments > 0)));
		worldData.Add(new KeyValuePair<string, object>("hasIntel", (backpack.numIntel > 0)));
		worldData.Add(new KeyValuePair<string, object>("hasWeapon", (backpack.numWeapons > 0)));
		worldData.Add(new KeyValuePair<string, object>("hasComponents", (backpack.numComponents > 0)));
		worldData.Add(new KeyValuePair<string, object>("hasRadioDevice", (backpack.hasRadioDevice == false)));
		worldData.Add(new KeyValuePair<string, object>("hasReportingDevice", (backpack.hasReportingDevice == false)));
		return worldData;
	}

	//Implement in subclass
	public abstract HashSet<KeyValuePair<string, object>> createGoalState();

	//This function is not handles here but would be usually run if world changes
	public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
	{

	}
	//This function debugs into console when plan is found 
	public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
	{
		Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
	}
	//This function debugs into console when an action is finished
	public void actionsFinished()
	{
		// Everything is done, we completed our actions for this gool. Hooray!
		Debug.Log("<color=blue>Actions completed</color>");
	}

	//This function debugs into console when a plan is aborted
	public void planAborted(GoapAction aborter)
	{
		Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
	}
	//This coroutine sets the ai to hiding in a hiding component and then unhides after fifteen seconds
	IEnumerator WaitForSecond()
    {
		GameObject text = GetComponentInChildren<TextMeshPro>().gameObject;
		text.SetActive(false);
		closest.spyHiding = true;
		//Set gameobject and children off spy layer
		foreach (Transform child in transform)
		{
			child.gameObject.layer = 0;
		}
		gameObject.layer = 0;
		yield return new WaitForSeconds(15);
		//Set gameobject and children back to spy layer
		gameObject.layer = 9;		
		foreach (Transform child in transform)
		{
			child.gameObject.layer = 9;
		}
		//Set to sneak
		spyDetectionState = detectionStates.Sneak;
		closest.spyHiding = false;
		text.SetActive(true);
		enumLock = false;
    }

	//Function for moving this ai agent, goap action related moving contained within normal state whilst other states are for detection response
	public bool moveAgent(GoapAction nextAction)
	{
		//If enemy detected
		if (enemyPosition != null)
		{
			spyDetectionState = detectionStates.Avoid;
		}
		//Switch statement for the detection states
		switch (spyDetectionState)
        {
			//Avoid
			case detectionStates.Avoid:
				//Ensure distance kept from guard
				if (Vector3.Distance(gameObject.transform.position, enemyPosition.position) < 10f)
                {
					Vector3 directionOfGuard = transform.position - enemyPosition.position;
					Vector3 newPosition = transform.position + directionOfGuard;
					navAgent.SetDestination(newPosition);
                }
				else
                {
					//Go to hide state and increase speed
					spyDetectionState = detectionStates.Hide;
					enemyPosition = null;
					closest = null;
					navAgent.speed = (moveSpeed * 2);
                }
				return false;
			//Hide
			case detectionStates.Hide:
				if (closest == null)
                {
					//Find nearest hiding components
					HidingComponent[] components = FindObjectsOfType(typeof(HidingComponent)) as HidingComponent[];
					float closestDist = 0;

					foreach (HidingComponent component in components)
					{
						if (component != null)
						{
							if (component.spyHiding == false)
							{
								//First one
								if (closest == null)
								{									
									closest = component;
									closestDist = (component.gameObject.transform.position - navAgent.transform.position).magnitude;
								}
								else
								{
									//Seeking nearest one
									float dist = (component.gameObject.transform.position - navAgent.transform.position).magnitude;
									if (dist < closestDist)
									{
										closest = component;
										closestDist = dist;
									}
								}
							}
						}
					}
					//Set destination to nearest hiding component
					if (navAgent != null)
                    {
						navAgent.SetDestination(closest.transform.position);

                    }
                }
				//When close enough begin coroutine for waiting
				else if (Vector3.Distance(gameObject.transform.position, new Vector3(closest.transform.position.x, transform.position.y, closest.transform.position.z)) < 1.5f)
                {
					if (enumLock == false)
                    {
						enumLock = true;
						StartCoroutine(WaitForSecond());
					}
                }
				return false;
			//Sneak
			case detectionStates.Sneak:
				//Sneak towards nearest helipad at half normal speed
				HelipadComponent helipad = FindObjectOfType<HelipadComponent>();
				if (navAgent.speed == (moveSpeed * 2))
                {
					navAgent.speed = (moveSpeed * 0.5f);
					navAgent.SetDestination(helipad.transform.position);
                }
				//When close enough reset to normal
				else if (Vector3.Distance(gameObject.transform.position, new Vector3(helipad.transform.position.x, transform.position.y, helipad.transform.position.z)) < 1.5f)
                {
					navAgent.speed = moveSpeed;
					spyDetectionState = detectionStates.Normal;
                }
				//Sneak to random position
				return false;
			//Normal
			case detectionStates.Normal:
					navAgent.SetDestination(nextAction.target.transform.position);
					if (Vector3.Distance(gameObject.transform.position, new Vector3 (nextAction.target.transform.position.x,transform.position.y, nextAction.target.transform.position.z)) < 1.5f)
					{
						//At set location is true
						nextAction.setInRange(true);
						return true;
					}
					else
						return false;
			default:
				return false;
		}

	}
}

