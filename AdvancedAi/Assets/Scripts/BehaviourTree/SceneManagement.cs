////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: SceneManagement.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneManagement : MonoBehaviour
{
    //Variables
    public Object agentPrefab;
    public int numberOfAgents = 1;
    public float spawnRange = 10.0f;

    //Start function loops through instantiating ai agent prefabs
    void Start()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            Vector2 randomCircle = (Random.insideUnitCircle * spawnRange);
            Vector3 randomPoint = Vector3.zero + (Random.insideUnitSphere * spawnRange);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 100.0f, NavMesh.AllAreas))
            {
                GameObject go = (GameObject)Instantiate(agentPrefab, hit.position, Quaternion.identity);

            }
        }
    }
}
