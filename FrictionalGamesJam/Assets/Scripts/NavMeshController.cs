using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshController : MonoBehaviour
{
    private NavMeshPlus.Components.NavMeshSurface navMesh;
    [HideInInspector] public List<NavMeshNode> graph;
    public bool showGraph = false;
    public Color connectionsColor = Color.blue;

    private void Start()
    {
        navMesh = GetComponent<NavMeshPlus.Components.NavMeshSurface>();

        int numOfChildren = transform.childCount;
        for(int i = 0; i < numOfChildren; i++)
        {
            NavMeshNode node = transform.GetChild(i).gameObject.GetComponent<NavMeshNode>();
            graph.Add(node);
        }  
    }


    public void UpdateNavmesh()
    {
        navMesh.UpdateNavMesh(navMesh.navMeshData);
    }
}
