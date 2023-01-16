using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshNode : MonoBehaviour
{
    public List<NavMeshNode> adjacentsNodes;

    private void Awake()
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    private void OnDrawGizmos()
    {
        NavMeshController controller = this.GetComponentInParent<NavMeshController>();

        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        sprite.enabled = controller.showGraph;

        if (controller.showGraph)
        {
            Gizmos.color = controller.connectionsColor;

            if (adjacentsNodes.Count != 0)
            {
                foreach (NavMeshNode node in adjacentsNodes)
                {
                    Gizmos.DrawLine(this.transform.position, node.transform.position);
                }
            }
        }    
    }
}
