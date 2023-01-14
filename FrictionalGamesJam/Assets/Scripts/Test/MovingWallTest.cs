using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallTest : MonoBehaviour
{
    public float speedRate = 1f;
    public NavMeshController navMeshController;

    void Update()
    {
        transform.position += transform.position * Time.deltaTime * speedRate;

        if (Input.GetKeyDown(KeyCode.E))
        {
            navMeshController.UpdateNavmesh();
        }
        
    }

    
}
