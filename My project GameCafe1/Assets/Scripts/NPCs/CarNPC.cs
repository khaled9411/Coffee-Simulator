using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarNPC : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    public Transform[] npcs;
    public float stopDistance = 5.0f;
    public float fieldOfViewAngle = 45.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypointIndex = FindClosestWaypoint();
        agent.destination = waypoints[currentWaypointIndex].position;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.destination = waypoints[currentWaypointIndex].position;
        }

        bool npcInFront = false;
        foreach (Transform npc in npcs)
        {
            Vector3 directionToNpc = npc.position - transform.position;
            float distanceToNpc = directionToNpc.magnitude;

            if (distanceToNpc < stopDistance)
            {
                float angleToNpc = Vector3.Angle(transform.forward, directionToNpc);
                if (angleToNpc < fieldOfViewAngle)
                {
                    npcInFront = true;
                    break;
                }
            }
        }

        agent.isStopped = npcInFront;
    }


    private int FindClosestWaypoint()
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
