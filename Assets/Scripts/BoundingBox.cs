using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoundingBox : MonoBehaviour
{
    public TextMeshProUGUI labelText; // Text component for displaying labels

    public void SetLabel(string label)
    {
        labelText.text = label;
    }

    public void SetPosition(Vector2 min, Vector2 max)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }
}
