////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: GoapAgent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This class contains/creates states for the goap agent that are used to construct the action plan
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public sealed class GoapAgent : MonoBehaviour
{
    //Variables
    #region Variables
    private FiniteStateMachine stateMachine;
	private FiniteStateMachine.FiniteStateMachineState idleState; // finds something to do
	private FiniteStateMachine.FiniteStateMachineState moveToState; // moves to a target
	private FiniteStateMachine.FiniteStateMachineState performActionState; // performs an action
	private HashSet<GoapAction> availableActions;
	private Queue<GoapAction> currentActions;
	//This class provides world data as well as listens to feedback on planning
	private IGoap dataProvider; 
	private GoapPlanner planner;
    #endregion Variables

	//Start function sets up the goap agent initially 
    void Start()
	{
		stateMachine = new FiniteStateMachine();
		availableActions = new HashSet<GoapAction>();
		currentActions = new Queue<GoapAction>();
		planner = new GoapPlanner();
		findDataProvider();
		createIdleState();
		createMoveToState();
		createPerformActionState();
		stateMachine.pushState(idleState);
		loadActions();
	}

	//Update function updates the state machine for this ai spy
	void Update()
	{
		stateMachine.Update(this.gameObject);
	}

	//This function adds an action passed in as a parameter
	public void addAction(GoapAction a)
	{
		availableActions.Add(a);
	}

	//Function returns action
	public GoapAction getAction(Type action)
	{
		foreach (GoapAction g in availableActions)
		{
			if (g.GetType().Equals(action))
				return g;
		}
		return null;
	}

	//Function removes action
	public void removeAction(GoapAction action)
	{
		availableActions.Remove(action);
	}

	//Function returns if action plan exists
	private bool hasActionPlan()
	{
		return currentActions.Count > 0;
	}

	//Function to create an idle state
	private void createIdleState()
	{
		idleState = (fsm, gameObj) => {

			//Getting the world state and goal
			HashSet<KeyValuePair<string, object>> worldState = dataProvider.getWorldState();
			HashSet<KeyValuePair<string, object>> goal = dataProvider.createGoalState();

			Queue<GoapAction> plan = planner.plan(gameObject, availableActions, worldState, goal);
			//Ai has a plan
			if (plan != null)
			{
				currentActions = plan;
				dataProvider.planFound(goal, plan);

				fsm.popState(); 
				fsm.pushState(performActionState);

			}
			//No plan so will return to idle
			else
			{
				Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
				dataProvider.planFailed(goal);
				fsm.popState();
				fsm.pushState(idleState);
			}

		};
	}

	//Function to create move to a state
	private void createMoveToState()
	{
		moveToState = (fsm, gameObj) => {
			// move the game object

			GoapAction action = currentActions.Peek();
			//Plan failed
			if (action.requiresInRange() && action.target == null)
			{
				Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
				//Move state
				fsm.popState(); 
				//Perform state
				fsm.popState(); 
				fsm.pushState(idleState);
				return;
			}

			//Make agent move itself
			if (dataProvider.moveAgent(action))
			{
				fsm.popState();
			}

		};
	}

	private void createPerformActionState()
	{

		performActionState = (fsm, gameObj) => 
		{
			//Perform an action
			if (!hasActionPlan())
			{
				//No actions to perform
				Debug.Log("<color=red>Done actions</color>");
				fsm.popState();
				fsm.pushState(idleState);
				dataProvider.actionsFinished();
				return;
			}

			GoapAction action = currentActions.Peek();
			if (action.isDone())
			{
				//Action completed, remove it and do another
				currentActions.Dequeue();
			}

			//Check if action plan exists
			if (hasActionPlan())
			{
				//Do the next action
				action = currentActions.Peek();
				bool inRange = action.requiresInRange() ? action.isInRange() : true;

				if (inRange)
				{
					//Because in range then do the action
					bool success = action.perform(gameObj);

					if (!success)
					{
						// action failed, we need to plan again
						fsm.popState();
						fsm.pushState(idleState);
						dataProvider.planAborted(action);
					}
				}
				else
				{
					// we need to move there first
					// push moveTo state
					fsm.pushState(moveToState);
				}

			}
			else
			{
				// no actions left, move to Plan state
				fsm.popState();
				fsm.pushState(idleState);
				dataProvider.actionsFinished();
			}

		};
	}

	//Function to find data provider
	private void findDataProvider()
	{
		foreach (Component comp in gameObject.GetComponents(typeof(Component)))
		{
			if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
			{
				dataProvider = (IGoap)comp;
				return;
			}
		}
	}

	//Function to load actions
	private void loadActions()
	{
		GoapAction[] actions = gameObject.GetComponents<GoapAction>();
		foreach (GoapAction a in actions)
		{
			availableActions.Add(a);
		}
		Debug.Log("Found actions: " + prettyPrint(actions));
	}

	//Overload
	public static string prettyPrint(HashSet<KeyValuePair<string, object>> state)
	{
		String s = "";
		foreach (KeyValuePair<string, object> kvp in state)
		{
			s += kvp.Key + ":" + kvp.Value.ToString();
			s += ", ";
		}
		return s;
	}
	//Overload
	public static string prettyPrint(Queue<GoapAction> actions)
	{
		String s = "";
		foreach (GoapAction a in actions)
		{
			s += a.GetType().Name;
			s += "-> ";
		}
		s += "GOAL";
		return s;
	}
	//Overload
	public static string prettyPrint(GoapAction[] actions)
	{
		String s = "";
		foreach (GoapAction a in actions)
		{
			s += a.GetType().Name;
			s += ", ";
		}
		return s;
	}

	public static string prettyPrint(GoapAction action)
	{
		String s = "" + action.GetType().Name;
		return s;
	}
}
