using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlexiblePathVisualizer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject menuPanel;
    public GameObject followerCube;
    public PathArrowSpawner arrowSpawner;

    public Transform path_1, path_2, path_3, path_4, path_5, path_6;
    public Transform waypoint_Entrance, waypoint_Kitchen, waypoint_Door1, waypoint_Door2, waypoint_Door3;
    public Transform waypoint_Door4, waypoint_Door5, waypoint_Door6, waypoint_Door7, waypoint_FireExit;

    private GameObject currentArrow;
    private bool isCubeMoving = false;
    public string currentLocation = "Entrance";

    private Dictionary<string, Transform> destinationWaypoints;
    private Dictionary<string, Transform> destinationPaths;

    void Start()
    {
        destinationWaypoints = new Dictionary<string, Transform>()
        {
            { "Entrance", waypoint_Entrance },
            { "Kitchen", waypoint_Kitchen },
            { "Door1", waypoint_Door1 },
            { "Door2", waypoint_Door2 },
            { "Door3", waypoint_Door3 },
            { "Door4", waypoint_Door4 },
            { "Door5", waypoint_Door5 },
            { "Door6", waypoint_Door6 },
            { "Door7", waypoint_Door7 },
            { "FireExit", waypoint_FireExit }
        };

        destinationPaths = new Dictionary<string, Transform>()
        {
            { "Entrance", path_1 },
            { "Kitchen", path_1 },
            { "Door1", path_3 },
            { "Door2", path_4 },
            { "Door3", path_6 },
            { "Door4", path_6 },
            { "Door5", path_5 },
            { "Door6", path_3 },
            { "Door7", path_2 },
            { "FireExit", path_6 }
        };
    }

    public void ShowPathFromUI(string destinationKey)
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        ShowPath(currentLocation, destinationKey);
    }

    public void ShowPath(string sourceKey, string destinationKey)
    {
        if (!destinationPaths.ContainsKey(sourceKey) || !destinationPaths.ContainsKey(destinationKey))
        {
            Debug.LogWarning($"Missing hallway path for: {sourceKey} or {destinationKey}");
            return;
        }

        ClearPath();

        List<Vector3> full3DPath = new List<Vector3>();
        if (followerCube != null)
            full3DPath.Add(followerCube.transform.position);

        Transform fromPath = destinationPaths[sourceKey];
        Transform toPath = destinationPaths[destinationKey];
        Transform destinationWaypoint = destinationWaypoints[destinationKey];

        List<Transform> hallwayOrder = new List<Transform> { path_1, path_2, path_3, path_4, path_5, path_6 };
        int fromIndex = hallwayOrder.IndexOf(fromPath);
        int toIndex = hallwayOrder.IndexOf(toPath);

        if (fromIndex <= toIndex)
        {
            for (int i = fromIndex; i <= toIndex; i++)
                full3DPath.Add(hallwayOrder[i].position);
        }
        else
        {
            for (int i = fromIndex; i >= toIndex; i--)
                full3DPath.Add(hallwayOrder[i].position);
        }

        full3DPath.Add(destinationWaypoint.position);

        // Spawn 3D Arrows
        if (arrowSpawner != null)
            arrowSpawner.SpawnArrowsFromPath(full3DPath);

        // Move Cube
        if (followerCube != null)
        {
            PathFollower follower = followerCube.GetComponent<PathFollower>();
            if (follower != null)
            {
                isCubeMoving = true;
                follower.SetPath(full3DPath);

                follower.onPathComplete = () =>
                {
                    currentLocation = destinationKey;
                    isCubeMoving = false;

                    if (arrowSpawner != null)
                        arrowSpawner.ClearArrows();

                    if (menuPanel != null)
                        menuPanel.SetActive(true);
                };
            }
        }
    }

    private void ClearPath()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }

        if (arrowSpawner != null)
            arrowSpawner.ClearArrows();
    }
}
