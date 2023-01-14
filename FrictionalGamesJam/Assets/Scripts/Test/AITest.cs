using UnityEngine;
using UnityEngine.AI;

public class AITest : MonoBehaviour
{
    private Vector3 target;
    NavMeshAgent agent;
    public float range = 10.0f;
    public PlayerController roboto;
    public bool followPlayer = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            GetPlayerPosition();
        } else
        {
            SetRandomPosition();
        }
        
        SetAgentPosition();
    }

    void GetPlayerPosition()
    {
        if (roboto)
        {
            target = roboto.transform.position;
        }
    }

    void SetRandomPosition()
    {
        if (agent.pathStatus.Equals(NavMeshPathStatus.PathComplete))
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                target = point;
            }
        }
        

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
