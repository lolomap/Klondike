using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static DraggableUI Dragged { get; private set; }
    public static bool IsDragging { get; private set; }

    public bool IsDraggable = true; 

    public RectTransform Rect { get; private set; }

    public delegate void DragBeginHandler(PointerEventData data);
    public event DragBeginHandler DragBegin;

    public delegate void DragEndHandler(PointerEventData data);
    public event DragEndHandler DragEnd;

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragBegin?.Invoke(eventData);
        Dragged = this;
        IsDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable)
        {
            OnEndDrag(eventData);
            return;
        }
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Rect, eventData.position, Camera.main, out Vector3 pos);

        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragEnd?.Invoke(eventData);
        IsDragging = false;
    }

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }
}
