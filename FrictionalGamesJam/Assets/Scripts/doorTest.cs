using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTest : MonoBehaviour
{
    private SpriteRenderer sprite;
    public NavMeshPlus.Components.NavMeshSurface navMesh;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (sprite.isVisible)
            {
                Debug.Log("Se ve");
            }
            else
            {
                Debug.Log("No se ve");
            }
        }
    }
}
