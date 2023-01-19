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
            if (adjacentsNodes.Count > 0)
            {
                foreach (NavMeshNode node in adjacentsNodes)
                {
                    DrawArrow.ForGizmo(this.transform.position, node.transform.position - this.transform.position, controller.connectionsColor);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        NavMeshController controller = this.GetComponentInParent<NavMeshController>();

        if (controller.showGraph)
        {
            if (adjacentsNodes.Count > 0)
            {
                foreach (NavMeshNode node in adjacentsNodes)
                {
                    Gizmos.color = controller.higlihgtColor;

                    SpriteRenderer spriteNode = node.GetComponentInChildren<SpriteRenderer>();
                    Vector3 nodeScale = spriteNode.size * node.transform.localScale * spriteNode.transform.localScale;
                    Gizmos.DrawWireSphere(node.transform.position, nodeScale.magnitude * 3f);
                }
            }
        }
    }
}
