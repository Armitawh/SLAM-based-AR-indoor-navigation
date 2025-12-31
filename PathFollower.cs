using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFollower : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f; // ⬅️ Rotation speed for turning
    private List<Vector3> pathPoints = new List<Vector3>();
    private int currentIndex = 0;
    private bool isFollowing = false;
    private float initialY;

    public Action onPathComplete;

    public void SetPath(List<Vector3> path)
    {
        pathPoints = path;
        currentIndex = 0;
        isFollowing = true;

        // Lock initial Y height
        initialY = transform.position.y;
    }

    void Update()
    {
        if (!isFollowing || pathPoints == null || currentIndex >= pathPoints.Count)
            return;

        Vector3 target = pathPoints[currentIndex];

        // 🧍 Lock Y height to original
        target.y = initialY;

        Vector3 direction = (target - transform.position).normalized;

        // 🚶 Move toward the current path point
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // 🔁 Rotate smoothly toward movement direction (if not zero vector)
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // ✅ Proceed to next point when close
        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            currentIndex++;

            if (currentIndex >= pathPoints.Count)
            {
                isFollowing = false;
                Debug.Log("✅ Finished path.");

                onPathComplete?.Invoke();
                onPathComplete = null; // prevent re-trigger
            }
        }
    }
}
