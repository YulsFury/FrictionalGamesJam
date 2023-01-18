using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Vector2 target;
    NavMeshAgent agent;
    [HideInInspector] public Room currentRoom;
    NavMeshPath path;

    [Header("Destination sprite")]
    public GameObject playerDestination;
    GameObject Destination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Destination = null;
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

    public void RepositionPlayer(Vector3 doorPosition)
    {
        Vector3 playerAwayFromDoor = this.transform.position - doorPosition;
        SetAgentPosition(this.transform.position + playerAwayFromDoor);        
    }

    public void StopPlayerMovement()
    {
        SetAgentPosition(this.transform.position);
    }

    public void PaintDestinationSprite(Vector3 clickPosition)
    {

        if(Destination)
        {
            Destroy(Destination);
        }
        
        Destination = Instantiate(playerDestination, new Vector3(clickPosition.x, clickPosition.y, 0), Quaternion.identity);
    }
}
