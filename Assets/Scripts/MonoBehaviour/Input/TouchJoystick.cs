using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class JoystickEvent : UnityEvent<Vector2>
{

}

/// <summary>
/// Most of the script is a copy of OnScreenStick, but instead of using the new input system, I use an event.
/// </summary>
public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private float _movementRange = 50;

    private Vector3 _startPos;
    private Vector2 _pointerDownPos;

    [SerializeField]
    private JoystickEvent _joystickEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _pointerDownPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - _pointerDownPos;

        delta = Vector2.ClampMagnitude(delta, _movementRange);
        ((RectTransform)transform).anchoredPosition = _startPos + (Vector3)delta;

        var newPos = new Vector2(delta.x / _movementRange, delta.y / _movementRange);
        _joystickEvent.Invoke(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((RectTransform)transform).anchoredPosition = _startPos;
        _joystickEvent.Invoke(Vector2.zero);
    }

    private void Start()
    {
        _startPos = ((RectTransform)transform).anchoredPosition;
    }
}
