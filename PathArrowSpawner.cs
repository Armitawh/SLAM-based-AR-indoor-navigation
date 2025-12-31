using System.Collections.Generic;
using UnityEngine;

public class PathArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spacing = 5.5f;         // Space between arrows
    public float arrowHeight = 1.5f;     // Height above ground
    public float removalThreshold = 1.5f; // Distance to remove passed arrows

    public Transform cubeTransform;

    private List<GameObject> spawnedArrows = new List<GameObject>();

    public void ClearArrows()
    {
        foreach (var arrow in spawnedArrows)
            Destroy(arrow);
        spawnedArrows.Clear();
    }

    public void SpawnArrowsFromPath(List<Vector3> pathPoints)
    {
        ClearArrows();
        if (pathPoints == null || pathPoints.Count < 2)
            return;

        float distanceSinceLastArrow = 0f;

        for (int i = 1; i < pathPoints.Count; i++)
        {
            Vector3 segmentStart = pathPoints[i - 1];
            Vector3 segmentEnd = pathPoints[i];
            Vector3 segmentDir = (segmentEnd - segmentStart).normalized;
            float segmentLength = Vector3.Distance(segmentStart, segmentEnd);

            float distanceAlong = 0f;

            while (distanceAlong < segmentLength)
            {
                if (distanceSinceLastArrow >= spacing)
                {
                    Vector3 arrowPos = segmentStart + segmentDir * distanceAlong;
                    arrowPos.y = arrowHeight;

                    Quaternion baseRot = arrowPrefab.transform.rotation;
                    Quaternion lookRot = Quaternion.LookRotation(segmentDir);
                    Quaternion finalRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0) * baseRot;

                    GameObject arrow = Instantiate(arrowPrefab, arrowPos, finalRot);
                    spawnedArrows.Add(arrow);

                    distanceSinceLastArrow = 0f;
                }

                float step = Mathf.Min(spacing - distanceSinceLastArrow, segmentLength - distanceAlong);
                distanceAlong += step;
                distanceSinceLastArrow += step;
            }
        }
    }

    void Update()
    {
        if (cubeTransform == null || spawnedArrows.Count == 0)
            return;

        for (int i = spawnedArrows.Count - 1; i >= 0; i--)
        {
            GameObject arrow = spawnedArrows[i];
            if (arrow == null)
            {
                spawnedArrows.RemoveAt(i);
                continue;
            }

            float horizontalDistance = Vector3.Distance(
                new Vector3(cubeTransform.position.x, 0, cubeTransform.position.z),
                new Vector3(arrow.transform.position.x, 0, arrow.transform.position.z)
            );

            if (horizontalDistance < removalThreshold)
            {
                Destroy(arrow);
                spawnedArrows.RemoveAt(i);
            }
        }
    }
}
