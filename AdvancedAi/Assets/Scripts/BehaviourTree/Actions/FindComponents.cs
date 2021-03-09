////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: FindNearestTarget.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class randomly selects a target component for the ai to move to
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.AI;

public class FindComponent : Action
{
    private Transform newTarget = null;
    public FindComponent(Agent owner) : base(owner)
    {

    }

    //Function sets target position to a random component
    public override BEHAVIOUR_STATUS Update()
    {
        int patrolType = (int)Random.Range(0, 6);
        //Start with not target and a large target distance

        //Randomly select component
        if (newTarget == null)
        {
            switch (patrolType)
            {
                //Radio points
                case 0:
                    RadioComponent[] radio = GameObject.FindObjectsOfType<RadioComponent>();
                    newTarget = radio[Random.Range(0, radio.Length)].transform;
                    break;
                //Reporting points
                case 1:
                    ReportingComponent[] reporting = GameObject.FindObjectsOfType<ReportingComponent>();
                    newTarget = reporting[Random.Range(0, reporting.Length)].transform;
                    break;
                //Hiding points
                case 2:
                    HidingComponent[] hiding = GameObject.FindObjectsOfType<HidingComponent>();
                    newTarget = hiding[Random.Range(0, hiding.Length)].transform;
                    break;
                //Document points
                case 3:
                    DocumentComponent[] documents = GameObject.FindObjectsOfType<DocumentComponent>();
                    newTarget = documents[Random.Range(0, documents.Length)].transform;
                    break;
                //Intel points
                case 4:
                    IntelComponent[] intel = GameObject.FindObjectsOfType<IntelComponent>();
                    newTarget = intel[Random.Range(0, intel.Length)].transform;
                    break;
                //Workshop points
                case 5:
                    WorkshopComponent[] workshop = GameObject.FindObjectsOfType<WorkshopComponent>();
                    newTarget = workshop[Random.Range(0, workshop.Length)].transform;
                    break;
                //Weapon points
                case 6:
                    WeaponComponent[] weapon = GameObject.FindObjectsOfType<WeaponComponent>();
                    newTarget = weapon[Random.Range(0, weapon.Length)].transform;
                    break;
                default:
                    break;
            }
        }

        if (newTarget != null)
        {
            //We have a target so the behaviour was successful
            GetOwner().SetTargetPosition(newTarget.position);
            if (GetOwner().GetNavMeshAgent().remainingDistance < 2)
            {
                newTarget = null;
            }
            return BEHAVIOUR_STATUS.SUCCESS;
        }
        else
        {
            //No target was found so node must keep running
            GetOwner().GetNavMeshAgent().isStopped = true;
            return BEHAVIOUR_STATUS.RUNNING;
        }
    }
}
