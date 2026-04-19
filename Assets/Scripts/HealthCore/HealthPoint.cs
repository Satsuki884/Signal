using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] private Color _smallSonarActiveColor = Color.red;
    [SerializeField] private Color _smallSonarInactiveColor = Color.white;

    public void SetHealthPointActive(bool active)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.color = active ? _smallSonarActiveColor : _smallSonarInactiveColor;
        }
    }
}