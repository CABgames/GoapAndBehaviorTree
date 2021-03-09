////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: BTGuard.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: The behaviour tree guard class inherits from behaviour tree and contains the various compounds, conditions 
///and actions for the behaviour tree guard ai
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from behaviour tree
public class BtGuard : BehaviourTree
{
    #region Variables

    //Compounds
    private Selector rootSelector;
    private Sequence patrolSequence;
    private Sequence seekingSequence;

    //Conditions
    private SpyDetected spyDetected;
    private HaveIArrived haveIArrived;

    //Actions
    private FindComponent findNearestTarget;
    private GoToObjects patrolMoveToTarget;
    private FindComponent findNearestPoint;
    private MoveToTarget guardMoveToTarget;


    public BtGuard(Agent ownerBrain) : base(ownerBrain)
    {
        //Initialise nodes
        //Compounds
        rootSelector = new Selector();
        patrolSequence = new Sequence();
        seekingSequence = new Sequence();

        //Conditions
        spyDetected = new SpyDetected(GetOwner());
        haveIArrived = new HaveIArrived(GetOwner());

        //Actions
        findNearestTarget = new FindComponent(GetOwner());
        patrolMoveToTarget = new GoToObjects(GetOwner());
        findNearestPoint = new FindComponent(GetOwner());
        guardMoveToTarget = new MoveToTarget(GetOwner());

        //Link nodes
        //Top level root selector
        rootSelector.AddChild(patrolSequence);
        rootSelector.AddChild(seekingSequence);

        //Patrol components sequence
        patrolSequence.AddChild(spyDetected);
        patrolSequence.AddChild(patrolMoveToTarget);
        patrolSequence.AddChild(guardMoveToTarget);

        //Chase spy sequence
        seekingSequence.AddChild(haveIArrived);
        seekingSequence.AddChild(findNearestPoint);
        seekingSequence.AddChild(guardMoveToTarget);



    }
    //Update function calling root selector
    public override void Update()
    {
        rootSelector.Update();
    }

    public override void Interupt()
    {
    }

    #endregion Variables
}
