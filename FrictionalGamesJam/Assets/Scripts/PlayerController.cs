using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Vector3 target;
    NavMeshAgent agent;
    [HideInInspector] public Floor currentFloor;
    NavMeshPath path;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }
    private void Start()
    {
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetPosition();
        //SetAgentPosition();
    }

    public void MovePlayer(Vector3 mousePosition)
    {
        target = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y, 0);
        SetAgentPosition();
    }
    void SetTargetPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            //RaycastHit hit;
            NavMesh.CalculatePath(agent.transform.position, target, NavMesh.AllAreas, path);
            if(path.status == (NavMeshPathStatus.PathPartial))
            {
                Debug.Log("path incomplete");
            }
            else if (path.status == (NavMeshPathStatus.PathInvalid))
            {
                Debug.Log("path invalid");
            }
            else if (path.status == (NavMeshPathStatus.PathComplete))
            {
                Debug.Log("path complete");
            }

            if (NavMesh.CalculatePath(agent.transform.position, target, NavMesh.AllAreas, path))

            {
                Debug.Log("hay path");

            }
            else
            {
                Debug.Log("No hay path");
            }

           
            
            //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            //{
            //    print("sí");
            //    agent.destination = hit.point;
            //}

                //print(CanReachPosition(target));
            }
    }

    public bool CanReachPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
