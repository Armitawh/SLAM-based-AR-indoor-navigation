using UnityEngine;

public class CameraNavigator : MonoBehaviour
{
    [Header("UI & Pathing")]
    public GameObject menuPanel;
    public FlexiblePathVisualizer pathVisualizer;
    public Transform cameraTarget;
    public Transform mainCamera;
    public Vector3 cameraOffset = new Vector3(0, 3, -6);
    public float cameraSmoothSpeed = 5f;

    [Header("Autonomous")]
    public AutonomousExplorer autonomousExplorer;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main.transform;

        if (cameraTarget == null && pathVisualizer != null)
            cameraTarget = pathVisualizer.followerCube?.transform;

        if (autonomousExplorer != null)
            autonomousExplorer.enabled = false;
    }

    void LateUpdate()
    {
        if (cameraTarget == null || mainCamera == null) return;

        Vector3 desiredPos = cameraTarget.position + cameraTarget.rotation * cameraOffset;
        Vector3 smoothPos = Vector3.Lerp(mainCamera.position, desiredPos, cameraSmoothSpeed * Time.deltaTime);
        mainCamera.position = smoothPos;
        mainCamera.LookAt(cameraTarget);
    }

    public void MoveToTarget(string destinationKey)
    {
        if (autonomousExplorer != null)
            autonomousExplorer.enabled = false;

        if (pathVisualizer != null)
        {
            pathVisualizer.gameObject.SetActive(true);
            pathVisualizer.ShowPathFromUI(destinationKey);
        }

        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    public void StartAutonomousExploration()
    {
        if (pathVisualizer != null)
            pathVisualizer.gameObject.SetActive(false); // 

        if (autonomousExplorer != null)
            autonomousExplorer.enabled = true;

        if (menuPanel != null)
            menuPanel.SetActive(false);
    }
}
