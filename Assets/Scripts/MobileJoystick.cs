using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position);

        position = Vector2.ClampMagnitude(position, background.sizeDelta.x / 2f);
        handle.anchoredPosition = position;

        inputVector = position / (background.sizeDelta.x / 2f);
        Horizontal = inputVector.x;
        Vertical = inputVector.y;
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        Horizontal = 0f;
        Vertical = 0f;
    }
}
