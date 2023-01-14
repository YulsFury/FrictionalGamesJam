using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Vector3 target;
    private NavMeshNode actualNode;
    private NavMeshAgent agent;
    [HideInInspector] public bool isFollowingPlayer = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            GetPlayerPosition();
        }
        else
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        SetTarget();
                    }
                }  
            }
        }

        Move();
    }

    //To be implemented. It will get the player position and set it has the target
    private void GetPlayerPosition()
    {
        
    }

    private void SetTarget()
    {
        if (!actualNode)
        {
            actualNode = GameManager.GM.NMC.graph[0];
        }

        int numOfAdjacentsNodes = actualNode.adjacentsNodes.Count;
        actualNode = actualNode.adjacentsNodes[Random.Range(0, numOfAdjacentsNodes)];
        target = actualNode.transform.position;
    }

    private void Move()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    

    
}
