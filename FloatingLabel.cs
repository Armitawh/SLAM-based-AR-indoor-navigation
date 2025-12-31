using TMPro;
using UnityEngine;

public class FloatingLabel : MonoBehaviour
{
    public string labelText = "Kitchen";
    private TextMeshPro label;

    void Start()
    {
        GameObject go = new GameObject("Label");
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(0, 0.5f, 0); // slightly above
        label = go.AddComponent<TextMeshPro>();
        label.text = labelText;
        label.fontSize = 3;
        label.color = Color.yellow;
        label.alignment = TextAlignmentOptions.Center;
    }

    void Update()
    {
        // Always face camera
        if (Camera.main != null)
            label.transform.rotation = Quaternion.LookRotation(label.transform.position - Camera.main.transform.position);
    }
}
