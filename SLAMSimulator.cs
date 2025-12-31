using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SLAMSimulator : MonoBehaviour
{
    public GameObject mapPointPrefab; // Assign a small sphere prefab in the Inspector
    public float mapPointSpacing = 2f; // Distance between simulated map points
    public float saveInterval = 0.5f;  // Seconds between trajectory saves

    private List<Vector3> trajectoryPoints = new List<Vector3>();
    private List<Vector3> mapPoints = new List<Vector3>();

    private float lastSaveTime = 0f;
    private float lastMapPointDistance = 0f;
    private Vector3 lastMapPointPosition;

    void Start()
    {
        lastMapPointPosition = transform.position;
        RegisterMapPoint(transform.position);
    }

    void Update()
    {
        // Save trajectory every 0.5s (or `saveInterval`)
        if (Time.time - lastSaveTime > saveInterval)
        {
            Vector3 currentPosition = transform.position;
            trajectoryPoints.Add(currentPosition);
            lastSaveTime = Time.time;
        }

        // Simulate new map point when moved enough
        float distanceMoved = Vector3.Distance(transform.position, lastMapPointPosition);
        if (distanceMoved >= mapPointSpacing)
        {
            RegisterMapPoint(transform.position);
            lastMapPointPosition = transform.position;
        }
    }

    void RegisterMapPoint(Vector3 point)
    {
        mapPoints.Add(point);

        // Optional: visualize with small sphere
        if (mapPointPrefab != null)
        {
            Instantiate(mapPointPrefab, point, Quaternion.identity);
        }
    }

    // Call this manually (e.g. with a UI button) to save data
    public void SaveToCSV()
    {
        string trajectoryPath = Path.Combine(Application.dataPath, "trajectory.csv");
        string mapPath = Path.Combine(Application.dataPath, "map_points.csv");

        File.WriteAllLines(trajectoryPath, trajectoryPoints.ConvertAll(p => $"{p.x},{p.y},{p.z}"));
        File.WriteAllLines(mapPath, mapPoints.ConvertAll(p => $"{p.x},{p.y},{p.z}"));

        Debug.Log("SLAM data saved.");
    }
}
