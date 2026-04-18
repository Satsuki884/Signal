using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class KnobController : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform knob;

    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;

    private float currentAngle = 0f;

    public Action<float> OnValueChanged; // 👈 подія

    public void OnPointerDown(PointerEventData eventData)
    {
        RotateKnob(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RotateKnob(eventData);
    }

    private void RotateKnob(PointerEventData eventData)
    {
        float delta = -eventData.delta.x; // рух миші вверх/вниз

        float sensitivity = 0.5f; // чутливість (підбери під себе)

        currentAngle += delta * sensitivity;

        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        knob.localRotation = Quaternion.Euler(0, 0, currentAngle);

        OnValueChanged?.Invoke(GetValue());
    }

    public float GetValue()
    {
        float t = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);

        return Mathf.Lerp(-80f, 0f, t);

        // t = 1f - t;
        // return Mathf.Lerp(0f, -80f, t);
    }
}