using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SLAMMapDrawer : MonoBehaviour
{
    public Transform target; 
    public float updateInterval = 0.5f;
    public float minDistance = 0.1f;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private float timer = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (target == null) return;

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            Vector3 pos = target.position;
            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], pos) > minDistance)
            {
                points.Add(pos);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
            }
            timer = 0f;
        }
    }

    void OnApplicationQuit()
    {
        SaveTrajectoryToCSV();
    }

    void SaveTrajectoryToCSV()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "slam_trajectory.csv");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("X,Y,Z");

        foreach (Vector3 point in points)
        {
            sb.AppendLine($"{point.x},{point.y},{point.z}");
        }

        File.WriteAllText(filePath, sb.ToString());
        Debug.Log("Trajectory saved to: " + filePath);
    }
}
