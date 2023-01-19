using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarRenderer : MonoBehaviour
{
    public int steps = 20;
    public float radius = 3;
    public float zPosition = -5;


    public LineRenderer circleRenderer;

    private void Update()
    {
        DrawCircle(steps, radius);
    }

    void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps;

        for(int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, zPosition);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
