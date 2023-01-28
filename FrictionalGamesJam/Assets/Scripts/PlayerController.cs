using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Vector2 target;
    NavMeshAgent agent;
    [HideInInspector] public Room currentRoom;
    [HideInInspector] public Room previousRoom;
    NavMeshPath path;
    Animator playerAnimator;
    private bool canMovePlayer = false;

    [HideInInspector] public bool isInMovementScreen;

    public SpriteRenderer sprite;

    [Header("Destination sprite")]
    public GameObject playerDestination;
    GameObject Destination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Destination = null;
        playerAnimator = GetComponent<Animator>();
        isInMovementScreen = true;
    }

    public void CanMovePlayer()
    {
        canMovePlayer = true;
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target);
        if (agent.velocity.magnitude / agent.speed <= 0.01f)
        {
            playerAnimator.SetBool("isMoving", false);
        }
        else
        {
            playerAnimator.SetBool("isMoving", true);
        }
    }
    public void MovePlayer(Vector3 mousePosition)
    {
        if (isInMovementScreen && !GameManager.GM.IM.isInMenus)
        {
            target = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y);
            PaintDestinationSprite();
            SetAgentPosition(target);
            
        }
    }
    void SetAgentPosition(Vector3 target)
    {
        if(canMovePlayer)
        {
            agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
        }
        
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

    public void PaintDestinationSprite()
    {
        if (canMovePlayer)
        {
            if (Destination)
            {
                Destroy(Destination);
            }

            Destination = Instantiate(playerDestination, new Vector3(target.x, target.y, 0), Quaternion.identity);
            AudioManager.instance.PlayPlayerDestination();
        }
    }
}
