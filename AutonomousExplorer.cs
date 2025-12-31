using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutonomousExplorer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rayDistance = 2f;
    public float turnSpeed = 120f;
    public bool isExploring = false;

    private List<Vector3> trajectoryPoints = new List<Vector3>();
    private List<Vector3> hitPoints = new List<Vector3>();
    private string csvPath;

    void Start()
    {
        csvPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "ExplorationData.csv");
        File.WriteAllText(csvPath, "Time,Position.x,Position.y,Position.z,Hit.x,Hit.y,Hit.z\n");
    }

    void Update()
    {
        if (!isExploring) return;

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            hitPoints.Add(hit.point);
            transform.Rotate(Vector3.up, Random.Range(-90f, 90f)); // Turn randomly
        }

        trajectoryPoints.Add(transform.position);

        // Save data
        string line = $"{Time.time},{transform.position.x},{transform.position.y},{transform.position.z},";

        if (hitPoints.Count > 0)
        {
            Vector3 lastHit = hitPoints[hitPoints.Count - 1];
            line += $"{lastHit.x},{lastHit.y},{lastHit.z}\n";
        }
        else
        {
            line += ",,,\n";
        }

        File.AppendAllText(csvPath, line);
    }

    // ✅ Only draws red obstacle dots (optional)
    void OnDrawGizmos()
    {
        if (!isExploring) return;

        // ❌ Removed green trajectory Gizmos
        // ✅ Keeps red hit Gizmos (optional)
        Gizmos.color = Color.red;
        foreach (Vector3 point in hitPoints)
        {
            Gizmos.DrawSphere(point, 0.15f);
        }
    }

    public void StartExploration()
    {
        isExploring = true;
        trajectoryPoints.Clear();
        hitPoints.Clear();
    }

    public void StopExploration()
    {
        isExploring = false;
    }
}
