////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: FiniteStateMachine.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: State based FSM in which states are push or popped on and off stack
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;


public class FiniteStateMachine
{
	//Variables
	private Stack<FiniteStateMachineState> stateStack = new Stack<FiniteStateMachineState>();
	public delegate void FiniteStateMachineState(FiniteStateMachine fsm, GameObject gameObject);

	//Update function checks state on stack not empty and if not invokes it
	public void Update(GameObject gameObject)
	{
		if (stateStack.Peek() != null)
			stateStack.Peek().Invoke(this, gameObject);
	}

	//Function for pushing a state onto the fsm stack
	public void pushState(FiniteStateMachineState state)
	{
		stateStack.Push(state);
	}

	//Function to pop state on the stack
	public void popState()
	{
		stateStack.Pop();
	}
}
