using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Vector2 target;
    NavMeshAgent agent;
    [HideInInspector] public Room currentRoom;
    NavMeshPath path;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }
    //private void Start()
    //{
    //    path = new NavMeshPath();
    //}

    // Update is called once per frame
    void Update()
    {
        //SetTargetPosition();
        //SetAgentPosition();
    }

    public void MovePlayer(Vector3 mousePosition)
    {
        target = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y);
        SetAgentPosition(target);
    }
    void SetAgentPosition(Vector3 target)
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    public void StopPlayer(Vector3 doorPosition)
    {
        Vector3 playerAwayFromDoor = this.transform.position - doorPosition;
        SetAgentPosition(this.transform.position + playerAwayFromDoor);        
    }

    //void SetTargetPosition()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        target = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    //        RaycastHit2D hit;
    //        NavMesh.CalculatePath(agent.transform.position, target, NavMesh.AllAreas, path);
    //        /*if(path.status == (NavMeshPathStatus.PathPartial))
    //        {
    //            Debug.Log("path incomplete");
    //        }
    //        else if (path.status == (NavMeshPathStatus.PathInvalid))
    //        {
    //            Debug.Log("path invalid");
    //        }
    //        else if (path.status == (NavMeshPathStatus.PathComplete))
    //        {
    //            Debug.Log("path complete");
    //        }

    //        if (NavMesh.CalculatePath(agent.transform.position, target, NavMesh.AllAreas, path))

    //        {
    //            Debug.Log("hay path");

    //        }
    //        else
    //        {
    //            Debug.Log("No hay path");
    //        }*/

    //        //hit = Physics2D.Raycast(Camera.main.transform.position, target);
    //        //if(hit)
    //        //{
    //        //    Debug.Log("sí");
    //        //}
    //        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
    //        //{

    //        //    print("sí");
    //        //    agent.destination = hit.point;
    //        //}

    //        //print(CanReachPosition(target));
    //    }
    //}

    //public bool CanReachPosition(Vector3 position)
    //{
    //    NavMeshPath path = new NavMeshPath();
    //    agent.CalculatePath(position, path);
    //    return path.status == NavMeshPathStatus.PathComplete;
    //}


}
