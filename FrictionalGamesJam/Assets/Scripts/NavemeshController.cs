using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavemeshController : MonoBehaviour
{
    public NavMeshPlus.Components.NavMeshSurface navMesh;


    public void UpdateNavmesh()
    {
        navMesh.UpdateNavMesh(navMesh.navMeshData);
    }
}
