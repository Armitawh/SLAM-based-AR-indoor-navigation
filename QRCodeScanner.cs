using UnityEngine;

public class QRScanner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger with: " + other.name);

        QRCodeZone qr = other.GetComponent<QRCodeZone>();
        if (qr != null)
        {
            Debug.Log("📦 Scanned QR Code: " + qr.locationName);

            // Update current location in the path visualizer
            FlexiblePathVisualizer visualizer = FindObjectOfType<FlexiblePathVisualizer>();
            if (visualizer != null)
            {
                visualizer.currentLocation = qr.locationName;
                Debug.Log("✅ Updated current location to: " + qr.locationName);
            }
        }
    }
}
