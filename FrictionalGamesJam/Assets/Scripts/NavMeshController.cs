using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshController : MonoBehaviour
{
    public static NavMeshController navMeshController;
    private NavMeshPlus.Components.NavMeshSurface navMesh;
    [HideInInspector] public List<NavMeshNodes> graph;

    private void Awake()
    {
        if (navMeshController)
        {
            NavMeshController.Destroy(navMeshController.gameObject);
        }
        else
        {
            navMeshController = this;
        }

        DontDestroyOnLoad(navMeshController.gameObject);
    }

    private void Start()
    {
        navMesh = GetComponent<NavMeshPlus.Components.NavMeshSurface>();

        int numOfChildren = transform.childCount;
        for(int i = 0; i < numOfChildren; i++)
        {
            NavMeshNodes node = transform.GetChild(i).gameObject.GetComponent<NavMeshNodes>();
            graph.Add(node);
        }
    }


    public void UpdateNavmesh()
    {
        navMesh.UpdateNavMesh(navMesh.navMeshData);
    }
}
