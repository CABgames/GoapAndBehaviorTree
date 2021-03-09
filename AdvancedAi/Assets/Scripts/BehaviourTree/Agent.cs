////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Agent.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    //Variables
    #region Variables
    //Static variables
    public static List<Agent> agentList = new List<Agent>();
    //Member variables
    public bool spyDetected;

    private Vector3 targetPosition = Vector3.zero;
    private BtGuard behaviourTree;
    private NavMeshAgent navAgent;
    private Renderer renderer;
    #endregion Variables

    void Start()
    {
        //On start add ourselves to the brain list
        agentList.Add(this);
        //Cache our navmesh component
        navAgent = GetComponent<NavMeshAgent>();
        //Create our behaviour tree
        behaviourTree = new BtGuard(this);
        //Cache our renderer
        renderer = GetComponent<Renderer>();
    }

    //Update function calls behaviour tree update
    void Update()
    {
        behaviourTree.Update();
    }

    //This function checks for collisions with spies and calls other guards
    private void OnCollisionEnter(Collision collision)
    {
        //Layer nine is spies
        if (collision.gameObject.layer == 9 && spyDetected)
        {
            collision.gameObject.SetActive(false);
            Agent[] guardsInRadius = FindObjectsOfType<Agent>();
            for (int i = 0; i < guardsInRadius.Length; i++)
            {
                if (guardsInRadius[i].tag == "Guard")
                {
                    if (guardsInRadius[i].TryGetComponent(out Agent guardAgent))
                    {
                        guardAgent.spyDetected = false;
                        guardAgent.SetTargetSpeed(5);
                    }
                }
            }
            //Add one to the guard score
            FindObjectOfType<ScoreManager>().IncrementGuardScore();
        }
    }
    public void ChangeColor(Color newColor)
    {
        renderer.material.color = newColor;
    }

    public Color GetColor()
    {
        return renderer.material.color;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navAgent;
    }


    public void SetTargetSpeed(float speed)
    {
        navAgent.speed = speed;
    }

    //Setters
    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }
}
