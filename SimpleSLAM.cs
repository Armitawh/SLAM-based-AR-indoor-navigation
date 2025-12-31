using System.Collections.Generic;
using UnityEngine;

public class SimpleSLAM : MonoBehaviour
{
    [Header("SLAM Settings")]
    public float recordInterval = 0.5f;      // How often to record a position
    public float loopClosureThreshold = 0.3f; // How close before detecting a loop closure

    private List<Vector3> visitedPositions = new List<Vector3>();
    private float timeSinceLastRecord = 0f;

    void Update()
    {
        timeSinceLastRecord += Time.deltaTime;

        if (timeSinceLastRecord >= recordInterval)
        {
            RecordPosition();
            timeSinceLastRecord = 0f;
        }
    }

    void RecordPosition()
    {
        Vector3 currentPos = transform.position;

        // Check for loop closure
        foreach (Vector3 pastPos in visitedPositions)
        {
            if (Vector3.Distance(currentPos, pastPos) < loopClosureThreshold)
            {
                Debug.Log("🔁 Loop closure detected near: " + pastPos);
                break;
            }
        }

        // Record the new position
        visitedPositions.Add(currentPos);
    }

    // Optional: Draw green gizmos in Scene View for debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector3 pos in visitedPositions)
        {
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }
}
